namespace Superscribe.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Superscribe.Utils;

    public class SuperscribeState
    {
        /// <summary>
        /// Base constructor for superscribe states
        /// </summary>
        public SuperscribeState()
        {
            this.Transitions = new ConcurrentQueue<SuperscribeState>();
            this.QueryString = new ConcurrentQueue<SuperscribeState>();
        }

        #region Properties

        /// <summary>
        /// The parent state
        /// </summary>
        protected SuperscribeState Parent { get; set; }

        /// <summary>
        /// Boolean flag indicating if the transition into this state is optional
        /// </summary>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// Concurrent queue of available transitions from this state
        /// </summary>
        public ConcurrentQueue<SuperscribeState> Transitions { get; set; }

        /// <summary>
        /// Concurrent queue of available querystring transitions
        /// </summary>
        public ConcurrentQueue<SuperscribeState> QueryString { get; set; }

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

        public SuperscribeState Slash(SuperscribeState nextState)
        {
            nextState.Parent = this;
            this.Transitions.Enqueue(nextState);
            return nextState;
        }

        private SuperscribeState Query(SuperscribeState queryState)
        {
            this.QueryString.Enqueue(queryState);
            return queryState;
        }

        public SuperscribeState Optional()
        {
            this.IsOptional = true;
            return this;
        }

        private SuperscribeState MatchAsRegex()
        {
            this.Pattern = new Regex(this.Template);
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

        /// <summary>
        /// Works backwards up the state\transition chain and returns the topmost state
        /// </summary>
        public SuperscribeState Base()
        {
            return Base(this, this.Parent);
        }

        /// <summary>
        /// Recursive component of <see cref="Base"/>
        /// </summary>
        /// <param name="state">The current state</param>
        /// <param name="parent">The parent of the current state</param>
        private SuperscribeState Base(SuperscribeState state, SuperscribeState parent)
        {
            if (parent == null)
            {
                return state;
            }

            return Base(parent, parent.Parent);
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Shorthand for .Slash between a state and a transition to a ConstantState
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="other">String used create constant state</param>
        public static SuperscribeState operator /(SuperscribeState state, string other)
        {
            return state.Slash(ʃ.Constant(other));
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a transition
        /// </summary>
        /// <param name="state">Any state</param>
        /// <param name="other">Any transition</param>
        public static SuperscribeState operator /(SuperscribeState state, SuperscribeState other)
        {
            return state.Slash(other);
        }

        /// <summary>
        /// Shorthand for .Slash between a state and its possible transitions
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="others">IEnumerble of transitions</param>
        public static SuperscribeState operator /(SuperscribeState state, IEnumerable<SuperscribeState> others)
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
        public static SuperscribeState operator &(SuperscribeState state, SuperscribeState other)
        {
            return state.Query(other);
        }

        /// <summary>
        /// Shorthand for creating a transition list from two alternatives
        /// </summary>
        /// <param name="state">First transition</param>
        /// <param name="other">Second transition</param>
        /// <returns>New list of transitions</returns>
        public static SuperList operator |(SuperscribeState state, SuperscribeState other)
        {
            return new SuperList { state, other };
        }

        /// <summary>
        /// Shorthand for adding a transition to an existing list 
        /// </summary>
        /// <param name="states">List of transitions</param>
        /// <param name="other">Alternative transition</param>
        /// <returns>Modified list of transitions</returns>
        public static SuperList operator |(SuperList states, SuperscribeState other)
        {
            states.Add(other);
            return states;
        }

        /// <summary>
        /// Shorthand for calling .Base
        /// </summary>
        /// <param name="state">Current state</param>
        /// <returns>The state at the base of the current transition chain</returns>
        public static SuperscribeState operator +(SuperscribeState state)
        {
            return state.Base();
        }

        /// <summary>
        /// Shorthand for calling .Optional
        /// </summary>
        /// <param name="state">Transition to be made optional</param>
        public static SuperscribeState operator -(SuperscribeState state)
        {
            state.Optional();
            return state;
        }

        /// <summary>
        /// Shorthand for calling .MatchAsRegex
        /// </summary>
        /// <param name="state">Transition to be made optional</param>
        public static SuperscribeState operator ~(SuperscribeState state)
        {
            state.MatchAsRegex();
            return state;
        }

        /// <summary>
        /// Implicit conversion between string and a ConstantState
        /// </summary>
        /// <param name="value">String to create ConstantState from</param>
        public static implicit operator SuperscribeState(string value)
        {
            return ʃ.Constant(value);
        }

        #endregion
    }
}
