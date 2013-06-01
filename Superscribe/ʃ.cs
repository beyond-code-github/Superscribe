namespace Superscribe
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using global::Superscribe.Models;

    public class ʃ
    {
        public static ControllerState Controller
        {
            get
            {
                return new ControllerState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+") };
            }
        }

        public static ActionState Action
        {
            get
            {
                return new ActionState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+") };
            }
        }

        public ʃ()
        {
            this.Transitions = new ConcurrentQueue<ʃ>();
            this.QueryString = new ConcurrentQueue<ʃ>();
        }

        public ConcurrentQueue<ʃ> Transitions { get; set; }

        public bool IsOptional { get; private set; }

        public ConcurrentQueue<ʃ> QueryString { get; set; }

        public static ConstantState Constant(string value, Action<ConstantState> configure)
        {
            var result = new ConstantState(value);
            configure(result);
            return result;
        }

        public static ConstantState Constant(string value)
        {
            return new ConstantState(value);
        }

        public static ʃ Route(Func<ʃ, ʃ> config)
        {
            return config(Superscribe.Base);
        }

        public static ʃ operator /(ʃ state, string other)
        {
            return state.Slash(ʃ.Constant(other));
        }

        public static ʃ operator /(ʃ state, ʃ other)
        {
            return state.Slash(other);
        }

        public static ʃ operator /(ʃ state, List<ʃ> others)
        {
            foreach (var s in others)
            {
                state.Slash(s);
            }

            return state;
        }

        public static ʃ operator &(ʃ state, ʃ other)
        {
            return state.Query(other);
        }

        public static SuperList operator |(ʃ state, ʃ other)
        {
            return new SuperList { state, other };
        }

        public static SuperList operator |(SuperList states, ʃ other)
        {
            states.Add(other);
            return states;
        }

        private static ʃ ParentOrCurrent(ʃ state, ʃ parent)
        {
            if (parent == null)
            {
                return state;
            }

            return ParentOrCurrent(parent, parent.Parent);
        }

        public static ʃ operator -(ʃ state)
        {
            return ParentOrCurrent(state, state.Parent);
        }

        public static ʃ operator ~(ʃ state)
        {
            state.Optional();
            return state;
        }

        public static implicit operator ʃ(string value)
        {
            return ʃ.Constant(value);
        }

        public ʃ Slash(ʃ nextState)
        {
            nextState.Parent = this;
            this.Transitions.Enqueue(nextState);
            return nextState;
        }

        protected ʃ Parent { get; set; }

        public Regex Pattern { get; set; }

        public string Template { get; set; }

        private ʃ Query(ʃ queryState)
        {
            this.QueryString.Enqueue(queryState);
            return queryState;
        }

        public ʃ Optional()
        {
            this.IsOptional = true;
            return this;
        }

        public virtual bool IsMatch(string segment)
        {
            if (this.Pattern != null)
            {
                return this.Pattern.IsMatch(segment);
            }

            return segment == this.Template;
        }
    }
}
