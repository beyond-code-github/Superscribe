namespace Superscribe.WebApi2.Internals
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskHelpersExtensions
    {
        internal static Task<TOuterResult> Then<TInnerResult, TOuterResult>(this Task<TInnerResult> task, Func<TInnerResult, TOuterResult> continuation, CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = false)
        {
            return task.ThenImpl(t => TaskHelpers.FromResult(continuation(t.Result)), cancellationToken, runSynchronously);
        }

        private static Task<TOuterResult> ThenImpl<TTask, TOuterResult>(this TTask task, Func<TTask, Task<TOuterResult>> continuation, CancellationToken cancellationToken, bool runSynchronously)
            where TTask : Task
        {
            // Stay on the same thread if we can
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    return TaskHelpers.FromErrors<TOuterResult>(task.Exception.InnerExceptions);
                }
                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                {
                    return TaskHelpers.Canceled<TOuterResult>();
                }
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        return continuation(task);
                    }
                    catch (Exception ex)
                    {
                        return TaskHelpers.FromError<TOuterResult>(ex);
                    }
                }
            }

            // Split into a continuation method so that we don't create a closure unnecessarily
            return ThenImplContinuation(task, continuation, cancellationToken, runSynchronously);
        }

        private static Task<TOuterResult> ThenImplContinuation<TOuterResult, TTask>(TTask task, Func<TTask, Task<TOuterResult>> continuation, CancellationToken cancellationToken, bool runSynchronously = false)
            where TTask : Task
        {
            SynchronizationContext syncContext = SynchronizationContext.Current;

            TaskCompletionSource<Task<TOuterResult>> tcs = new TaskCompletionSource<Task<TOuterResult>>();

            task.ContinueWith(innerTask =>
            {
                if (innerTask.IsFaulted)
                {
                    tcs.TrySetException(innerTask.Exception.InnerExceptions);
                }
                else if (innerTask.IsCanceled || cancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    if (syncContext != null)
                    {
                        syncContext.Post(state =>
                        {
                            try
                            {
                                tcs.TrySetResult(continuation(task));
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                            }
                        }, state: null);
                    }
                    else
                    {
                        try
                        {
                            tcs.TrySetResult(continuation(task));
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                    }
                }
            }, runSynchronously ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None);

            return tcs.Task.FastUnwrap();
        }

        internal static Task<TResult> Catch<TResult>(this Task<TResult> task, Func<CatchInfo<TResult>, CatchInfo<TResult>.CatchResult> continuation, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Fast path for successful tasks, to prevent an extra TCS allocation
            if (task.Status == TaskStatus.RanToCompletion)
            {
                return task;
            }
            return task.CatchImpl(() => continuation(new CatchInfo<TResult>(task)).Task, cancellationToken);
        }

        private static Task<TResult> CatchImpl<TResult>(this Task task, Func<Task<TResult>> continuation, CancellationToken cancellationToken)
        {
            // Stay on the same thread if we can
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    try
                    {
                        Task<TResult> resultTask = continuation();
                        if (resultTask == null)
                        {
                            // Not a resource because this is an internal class, and this is a guard clause that's intended
                            // to be thrown by us to us, never escaping out to end users.
                            throw new InvalidOperationException("You must set the Task property of the CatchInfo returned from the TaskHelpersExtensions.Catch continuation.");
                        }

                        return resultTask;
                    }
                    catch (Exception ex)
                    {
                        return TaskHelpers.FromError<TResult>(ex);
                    }
                }
                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                {
                    return TaskHelpers.Canceled<TResult>();
                }

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
                    tcs.TrySetFromTask(task);
                    return tcs.Task;
                }
            }

            // Split into a continuation method so that we don't create a closure unnecessarily
            return CatchImplContinuation(task, continuation);
        }

        private static Task<TResult> CatchImplContinuation<TResult>(Task task, Func<Task<TResult>> continuation)
        {
            SynchronizationContext syncContext = SynchronizationContext.Current;

            TaskCompletionSource<Task<TResult>> tcs = new TaskCompletionSource<Task<TResult>>();

            // this runs only if the inner task did not fault
            task.ContinueWith(innerTask => tcs.TrySetFromTask(innerTask), TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            // this runs only if the inner task faulted
            task.ContinueWith(innerTask =>
            {
                if (syncContext != null)
                {
                    syncContext.Post(state =>
                    {
                        try
                        {
                            Task<TResult> resultTask = continuation();
                            if (resultTask == null)
                            {
                                throw new InvalidOperationException("You cannot return null from the TaskHelpersExtensions.Catch continuation. You must return a valid task or throw an exception.");
                            }

                            tcs.TrySetResult(resultTask);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                    }, state: null);
                }
                else
                {
                    try
                    {
                        Task<TResult> resultTask = continuation();
                        if (resultTask == null)
                        {
                            throw new InvalidOperationException("You cannot return null from the TaskHelpersExtensions.Catch continuation. You must return a valid task or throw an exception.");
                        }

                        tcs.TrySetResult(resultTask);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return tcs.Task.FastUnwrap();
        }

        internal static Task<TResult> FastUnwrap<TResult>(this Task<Task<TResult>> task)
        {
            Task<TResult> innerTask = task.Status == TaskStatus.RanToCompletion ? task.Result : null;
            return innerTask ?? task.Unwrap();
        }
    }

    internal abstract class CatchInfoBase<TTask>
        where TTask : Task
    {
        private Exception _exception;
        private TTask _task;

        protected CatchInfoBase(TTask task)
        {
            Contract.Assert(task != null);
            this._task = task;
            this._exception = this._task.Exception.GetBaseException();  // Observe the exception early, to prevent tasks tearing down the app domain
        }

        /// <summary>
        /// The exception that was thrown to cause the Catch block to execute.
        /// </summary>
        public Exception Exception
        {
            get { return this._exception; }
        }

        /// <summary>
        /// Returns a CatchResult that re-throws the original exception.
        /// </summary>
        public CatchResult Throw()
        {
            return new CatchResult { Task = this._task };
        }

        /// <summary>
        /// Represents a result to be returned from a Catch handler.
        /// </summary>
        internal struct CatchResult
        {
            /// <summary>
            /// Gets or sets the task to be returned to the caller.
            /// </summary>
            internal TTask Task { get; set; }
        }
    }

    internal class CatchInfo<T> : CatchInfoBase<Task<T>>
    {
        public CatchInfo(Task<T> task)
            : base(task)
        {
        }

        public CatchResult Handled(T returnValue)
        {
            return new CatchResult { Task = TaskHelpers.FromResult(returnValue) };
        }

        public CatchResult Task(Task<T> task)
        {
            return new CatchResult { Task = task };
        }

        public CatchResult Throw(Exception ex)
        {
            return new CatchResult { Task = TaskHelpers.FromError<T>(ex) };
        }
    }
}
