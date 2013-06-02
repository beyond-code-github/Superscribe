namespace Superscribe
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Superscribe.WebAPI;

    using global::Superscribe.Models;

    /// <summary>
    /// Superscribe base class
    /// </summary>
    public class ʃ
    {
        #region Static Methods

        /// <summary>
        /// Matches any valid identifier and sets ControllerName
        /// </summary>
        public static ControllerState Controller
        {
            get
            {
                return new ControllerState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+") };
            }
        }

        /// <summary>
        /// Matches any valid identifier and sets ActionName
        /// </summary>
        public static ActionState Action
        {
            get
            {
                return new ActionState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+") };
            }
        }

        /// <summary>
        /// Matches a string constant and performs no action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        public static ConstantState Constant(string value)
        {
            return new ConstantState(value);
        }

        /// <summary>
        /// Matches a string constant and performs a custom action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        /// <param name="configure">Action to execute</param>
        public static ConstantState Constant(string value, Action<ConstantState> configure)
        {
            var result = new ConstantState(value);
            configure(result);
            return result;
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="id">Parameter name</param>
        public static ʃ Int(string id)
        {
            return new ParamState<int>("id");
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static ʃ Route(Func<ʃ, ʃ> config)
        {
            return config(SuperscribeConfig.Base);
        }

        /// <summary>
        /// Works backwards up the state\transition chain and returns the topmost state
        /// </summary>
        /// <param name="state">State to start from</param>
        public static ʃ Base(ʃ state)
        {
            return Base(state, state.Parent);
        }

        /// <summary>
        /// Recursive component of <see cref="Base"/>
        /// </summary>
        /// <param name="state">The current state</param>
        /// <param name="parent">The parent of the current state</param>
        private static ʃ Base(ʃ state, ʃ parent)
        {
            if (parent == null)
            {
                return state;
            }

            return Base(parent, parent.Parent);
        }

        #endregion

        /// <summary>
        /// Base constructor for superscribe states
        /// </summary>
        public ʃ()
        {
            this.Transitions = new ConcurrentQueue<ʃ>();
            this.QueryString = new ConcurrentQueue<ʃ>();
        }

        #region Properties

        /// <summary>
        /// The parent state
        /// </summary>
        protected ʃ Parent { get; set; }

        /// <summary>
        /// Boolean flag indicating if the transition into this state is optional
        /// </summary>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// Concurrent queue of available transitions from this state
        /// </summary>
        public ConcurrentQueue<ʃ> Transitions { get; set; }

        /// <summary>
        /// Concurrent queue of available querystring transitions
        /// </summary>
        public ConcurrentQueue<ʃ> QueryString { get; set; }

        /// <summary>
        /// Regex pattern to use when matching
        /// </summary>
        public Regex Pattern { get; set; }

        /// <summary>
        /// String template to use when matching
        /// </summary>
        public string Template { get; set; }

        #endregion

        #region Methods

        public ʃ Slash(ʃ nextState)
        {
            nextState.Parent = this;
            this.Transitions.Enqueue(nextState);
            return nextState;
        }

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

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Shorthand for .Slash between a state and a transition to a ConstantState
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="other">String used create constant state</param>
        public static ʃ operator /(ʃ state, string other)
        {
            return state.Slash(ʃ.Constant(other));
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a transition
        /// </summary>
        /// <param name="state">Any state</param>
        /// <param name="other">Any transition</param>
        public static ʃ operator /(ʃ state, ʃ other)
        {
            return state.Slash(other);
        }

        /// <summary>
        /// Shorthand for .Slash between a state and its possible transitions
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="others">IEnumerble of transitions</param>
        public static ʃ operator /(ʃ state, IEnumerable<ʃ> others)
        {
            foreach (var s in others)
            {
                state.Slash(s);
            }

            return state;
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a Querystring transition
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="other">Querystring transition</param>
        public static ʃ operator &(ʃ state, ʃ other)
        {
            return state.Query(other);
        }

        /// <summary>
        /// Shorthand for creating a transition list from two alternatives
        /// </summary>
        /// <param name="state">First transition</param>
        /// <param name="other">Second transition</param>
        /// <returns>New list of transitions</returns>
        public static SuperList operator |(ʃ state, ʃ other)
        {
            return new SuperList { state, other };
        }

        /// <summary>
        /// Shorthand for adding a transition to an existing list 
        /// </summary>
        /// <param name="states">List of transitions</param>
        /// <param name="other">Alternative transition</param>
        /// <returns>Modified list of transitions</returns>
        public static SuperList operator |(SuperList states, ʃ other)
        {
            states.Add(other);
            return states;
        }

        /// <summary>
        /// Shorthand for calling .Base
        /// </summary>
        /// <param name="state">Current state</param>
        /// <returns>The state at the base of the current transition chain</returns>
        public static ʃ operator -(ʃ state)
        {
            return Base(state, state.Parent);
        }

        /// <summary>
        /// Shorthand for calling .Optional
        /// </summary>
        /// <param name="state">Transition to be made optional</param>
        public static ʃ operator ~(ʃ state)
        {
            state.Optional();
            return state;
        }

        /// <summary>
        /// Implicit conversion between string and a ConstantState
        /// </summary>
        /// <param name="value">String to create ConstantState from</param>
        public static implicit operator ʃ(string value)
        {
            return ʃ.Constant(value);
        }

        #endregion
    }
}
