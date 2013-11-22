namespace Superscribe.WebApi2.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class ActionSelectorCacheItem
    {
        private readonly HttpControllerDescriptor controllerDescriptor;

        private readonly ReflectedHttpActionDescriptor[] actionDescriptors;

        private readonly IDictionary<ReflectedHttpActionDescriptor, string[]> actionParameterNames = new Dictionary<ReflectedHttpActionDescriptor, string[]>();

        private readonly ILookup<string, ReflectedHttpActionDescriptor> actionNameMapping;

        private readonly HttpMethod[] cacheListVerbKinds = new[] { HttpMethod.Get, HttpMethod.Put, HttpMethod.Post };

        private readonly ReflectedHttpActionDescriptor[][] cacheListVerbs;

        public ActionSelectorCacheItem(HttpControllerDescriptor controllerDescriptor)
        {
            Contract.Assert(controllerDescriptor != null);

            // Initialize the cache entirely in the ctor on a single thread.
            this.controllerDescriptor = controllerDescriptor;

            var allMethods = this.controllerDescriptor.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var validMethods = Array.FindAll(allMethods, IsValidActionMethod);

            this.actionDescriptors = new ReflectedHttpActionDescriptor[validMethods.Length];
            for (var i = 0; i < validMethods.Length; i++)
            {
                var method = validMethods[i];
                var actionDescriptor = new ReflectedHttpActionDescriptor(this.controllerDescriptor, method);
                this.actionDescriptors[i] = actionDescriptor;
                var actionBinding = actionDescriptor.ActionBinding;

                // Building an action parameter name mapping to compare against the URI parameters coming from the request. Here we only take into account required parameters that are simple types and come from URI.
                this.actionParameterNames.Add(
                    actionDescriptor,
                    actionBinding.ParameterBindings
                        .Where(binding => !binding.Descriptor.IsOptional && IsSimpleUnderlyingType(binding.Descriptor.ParameterType) && binding.WillReadUri())
                        .Select(binding => binding.Descriptor.Prefix ?? binding.Descriptor.ParameterName).ToArray());
            }

            this.actionNameMapping = this.actionDescriptors.ToLookup(actionDesc => actionDesc.ActionName, StringComparer.OrdinalIgnoreCase);

            // Bucket the action descriptors by common verbs. 
            var len = this.cacheListVerbKinds.Length;
            this.cacheListVerbs = new ReflectedHttpActionDescriptor[len][];
            for (var i = 0; i < len; i++)
            {
                this.cacheListVerbs[i] = this.FindActionsForVerbWorker(this.cacheListVerbKinds[i]);
            }
        }

        public HttpControllerDescriptor HttpControllerDescriptor
        {
            get { return this.controllerDescriptor; }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing of response instance.")]
        public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext, IEnumerable<string> parameterNames, string actionName = "")
        {
            ReflectedHttpActionDescriptor[] actionsFoundByHttpMethods;

            var useActionName = !string.IsNullOrEmpty(actionName);
            var incomingMethod = controllerContext.Request.Method;

            // First get an initial candidate list. 
            if (useActionName)
            {
                // We have an explicit action value, do traditional binding. Just lookup by actionName
                var actionsFoundByName = this.actionNameMapping[actionName].ToArray();

                // Throws HttpResponseException with NotFound status because no action matches the Name
                if (actionsFoundByName.Length == 0)
                {
                    throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format("No action was found on the controller '{0}' that matches the name '{1}'.", this.controllerDescriptor.ControllerName, actionName)));
                }

                // This filters out any incompatible verbs from the incoming action list
                actionsFoundByHttpMethods = actionsFoundByName.Where(actionDescriptor => actionDescriptor.SupportedHttpMethods.Contains(incomingMethod)).ToArray();
            }
            else
            {
                // No {action} parameter, infer it from the verb.
                actionsFoundByHttpMethods = this.FindActionsForVerb(incomingMethod);
            }

            // Throws HttpResponseException with MethodNotAllowed status because no action matches the Http Method
            if (actionsFoundByHttpMethods.Length == 0)
            {
                throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.MethodNotAllowed,
                    string.Format("The requested resource does not support http method '{0}'.", incomingMethod)));
            }

            // Make sure the action parameter matches the route and query parameters. Overload resolution logic is applied when needed.
            var actionsFoundByParams = this.FindActionUsingParameters(actionsFoundByHttpMethods, parameterNames);
            var selectedActions = RunSelectionFilters(actionsFoundByParams);

            switch (selectedActions.Count)
            {
                case 0:
                    throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format("No action was found on the controller '{0}' that matches the request.", this.controllerDescriptor.ControllerName)));
                case 1:
                    return selectedActions[0];
                default:
                    var verbMatches =
                        selectedActions.Where(
                            o => o.ActionName.Equals(incomingMethod.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

                    if (verbMatches.Count == 1)
                    {
                        return verbMatches.First();
                    }

                    if (verbMatches.Count > 1)
                    {
                        throw new InvalidOperationException(string.Format("Multiple actions were found that match the request: {0}", CreateAmbiguousMatchList(verbMatches)));
                    }

                    throw new InvalidOperationException(string.Format("Multiple actions were found that match the request: {0}", CreateAmbiguousMatchList(selectedActions)));
            }
        }

        public ILookup<string, HttpActionDescriptor> GetActionMapping()
        {
            return new LookupAdapter { Source = this.actionNameMapping };
        }

        private IEnumerable<ReflectedHttpActionDescriptor> FindActionUsingParameters(IEnumerable<ReflectedHttpActionDescriptor> actionsFound, IEnumerable<string> parameterNames)
        {
            var routeParameterNames = parameterNames.ToList();

            if (routeParameterNames.Any())
            {
                // action parameters is a subset of route parameters and query parameters
                actionsFound = actionsFound.Where(descriptor => IsSubset(this.actionParameterNames[descriptor], routeParameterNames)).ToList();

                if (actionsFound.Count() > 1)
                {
                    // select the results that match the most number of required parameters 
                    actionsFound = actionsFound
                        .GroupBy(descriptor => this.actionParameterNames[descriptor].Length)
                        .OrderByDescending(g => g.Key)
                        .First();
                }
            }
            else
            {
                // return actions with no parameters
                actionsFound = actionsFound.Where(descriptor => this.actionParameterNames[descriptor].Length == 0).ToList();
            }

            return actionsFound;
        }

        private static bool IsSubset(IEnumerable<string> actionParameters, ICollection<string> routeAndQueryParameters)
        {
            return actionParameters.All(routeAndQueryParameters.Contains);
        }

        private static List<ReflectedHttpActionDescriptor> RunSelectionFilters(IEnumerable<HttpActionDescriptor> descriptorsFound)
        {
            var matchesWithoutSelectionAttributes = new List<ReflectedHttpActionDescriptor>();

            foreach (ReflectedHttpActionDescriptor actionDescriptor in descriptorsFound)
            {
                var attrs = actionDescriptor.GetCustomAttributes<NonActionAttribute>();
                if (attrs.Count == 0)
                {
                    matchesWithoutSelectionAttributes.Add(actionDescriptor);
                }
            }

            return matchesWithoutSelectionAttributes;
        }

        // This is called when we don't specify an Action name
        // Get list of actions that match a given verb. This can match by name or IActionHttpMethodSelecto
        private ReflectedHttpActionDescriptor[] FindActionsForVerb(HttpMethod verb)
        {
            // Check cache for common verbs. 
            for (var i = 0; i < this.cacheListVerbKinds.Length; i++)
            {
                // verb selection on common verbs is normalized to have object reference identity. 
                // This is significantly more efficient than comparing the verbs based on strings. 
                if (ReferenceEquals(verb, this.cacheListVerbKinds[i]))
                {
                    return this.cacheListVerbs[i];
                }
            }

            // General case for any verbs. 
            return this.FindActionsForVerbWorker(verb);
        }

        // This is called when we don't specify an Action name
        // Get list of actions that match a given verb. This can match by name or IActionHttpMethodSelector.
        // Since this list is fixed for a given verb type, it can be pre-computed and cached.   
        // This function should not do caching. It's the helper that builds the caches. 
        private ReflectedHttpActionDescriptor[] FindActionsForVerbWorker(HttpMethod verb)
        {
            var listMethods = new List<ReflectedHttpActionDescriptor>();

            foreach (var descriptor in this.actionDescriptors)
            {
                if (descriptor.SupportedHttpMethods.Contains(verb))
                {
                    listMethods.Add(descriptor);
                }
            }

            return listMethods.ToArray();
        }

        private static string CreateAmbiguousMatchList(IEnumerable<HttpActionDescriptor> ambiguousDescriptors)
        {
            var exceptionMessageBuilder = new StringBuilder();
            foreach (var methodInfo in from ReflectedHttpActionDescriptor descriptor in ambiguousDescriptors select descriptor.MethodInfo)
            {
                exceptionMessageBuilder.AppendLine();
                if (methodInfo.DeclaringType != null)
                {
                    exceptionMessageBuilder.Append(
                        string.Format("{0} on type {1}", methodInfo, methodInfo.DeclaringType.FullName));
                }
            }

            return exceptionMessageBuilder.ToString();
        }

        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsSpecialName)
            {
                // not a normal method, e.g. a constructor or an event
                return false;
            }

            var declaringType = methodInfo.GetBaseDefinition().DeclaringType;
            return declaringType != null && !declaringType.IsAssignableFrom(typeof(ApiController));
        }

        internal static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Decimal) ||
                   type == typeof(Guid) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan);
        }

        internal static bool IsSimpleUnderlyingType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }

            return IsSimpleType(type);
        }
    }

    class LookupAdapter : ILookup<string, HttpActionDescriptor>
    {
        public ILookup<string, ReflectedHttpActionDescriptor> Source;

        public int Count
        {
            get { return this.Source.Count; }
        }

        public IEnumerable<HttpActionDescriptor> this[string key]
        {
            get { return this.Source[key]; }
        }

        public bool Contains(string key)
        {
            return this.Source.Contains(key);
        }

        public IEnumerator<IGrouping<string, HttpActionDescriptor>> GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
