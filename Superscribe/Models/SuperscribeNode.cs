namespace Superscribe.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Superscribe.Utils;

    public class SuperscribeNode
    {
        private Predicate<string> activationFunction;

        /// <summary>
        /// Base constructor for superscribe states
        /// </summary>
        public SuperscribeNode()
        {
            this.Edges = new ConcurrentQueue<SuperscribeNode>();
            this.QueryString = new ConcurrentQueue<SuperscribeNode>();

            this.ActivationFunction = segment =>
            {
                if (this.Pattern != null)
                {
                    return this.Pattern.IsMatch(segment);
                }

                return string.Equals(this.Template, segment, StringComparison.OrdinalIgnoreCase);
            };
        }

        #region Properties

        /// <summary>
        /// The parent state
        /// </summary>
        protected SuperscribeNode Parent { get; set; }

        /// <summary>
        /// Boolean flag indicating if the transition into this state is optional
        /// </summary>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// Concurrent queue of available transitions from this state
        /// </summary>
        public ConcurrentQueue<SuperscribeNode> Edges { get; set; }

        /// <summary>
        /// Concurrent queue of available querystring transitions
        /// </summary>
        public ConcurrentQueue<SuperscribeNode> QueryString { get; set; }

        /// <summary>
        /// Regex pattern to use when matching
        /// </summary>
        public Regex Pattern { get; set; }

        /// <summary>
        /// String template to use when matching
        /// </summary>
        public string Template { get; set; }

        public object Result { get; set; }

        public Action<RouteData, string> ActionFunction { get; set; }

        public Action<RouteData> FinalFunction { get; set; }

        public Predicate<string> ActivationFunction
        {
            get
            {
                return this.activationFunction;
            }
            set
            {
                this.activationFunction = value;
            }
        }

        #endregion

        #region Methods

        public SuperscribeNode Slash(SuperscribeNode nextNode)
        {
            nextNode.Parent = this;
            this.Edges.Enqueue(nextNode);
            return nextNode;
        }

        private SuperscribeNode Query(SuperscribeNode queryNode)
        {
            this.QueryString.Enqueue(queryNode);
            return queryNode;
        }

        public SuperscribeNode Optional()
        {
            this.IsOptional = true;
            return this;
        }

        public virtual SuperscribeNode ʃ(Action<RouteData, string> command)
        {
            this.ActionFunction = command;
            return this;
        }

        private SuperscribeNode MatchAsRegex()
        {
            this.Pattern = new Regex(this.Template);
            return this;
        }

        /// <summary>
        /// Works backwards up the state\transition chain and returns the topmost state
        /// </summary>
        public SuperscribeNode Base()
        {
            return Base(this, this.Parent);
        }

        /// <summary>
        /// Recursive component of <see cref="Base"/>
        /// </summary>
        /// <param name="node">The current state</param>
        /// <param name="parent">The parent of the current state</param>
        private SuperscribeNode Base(SuperscribeNode node, SuperscribeNode parent)
        {
            if (parent == null)
            {
                return node;
            }

            return Base(parent, parent.Parent);
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Shorthand for .Slash between a state and a transition to a ConstantState
        /// </summary>
        /// <param name="node">State</param>
        /// <param name="other">String used create constant state</param>
        public static SuperscribeNode operator /(SuperscribeNode node, string other)
        {
            return node.Slash(Superscribe.ʃ.Constant(other));
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a transition
        /// </summary>
        /// <param name="node">Any state</param>
        /// <param name="other">Any transition</param>
        public static SuperscribeNode operator /(SuperscribeNode node, SuperscribeNode other)
        {
            return node.Slash(other);
        }

        /// <summary>
        /// Shorthand for .Slash between a state and its possible transitions
        /// </summary>
        /// <param name="node">State</param>
        /// <param name="others">IEnumerble of transitions</param>
        public static SuperscribeNode operator /(SuperscribeNode node, IEnumerable<SuperscribeNode> others)
        {
            foreach (var s in others)
            {
                node.Slash(s.Base());
            }

            return node;
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a Querystring transition
        /// </summary>
        /// <param name="node">State</param>
        /// <param name="other">Querystring transition</param>
        public static SuperscribeNode operator &(SuperscribeNode node, SuperscribeNode other)
        {
            return node.Query(other);
        }

        /// <summary>
        /// Shorthand for creating a transition list from two alternatives
        /// </summary>
        /// <param name="node">First transition</param>
        /// <param name="other">Second transition</param>
        /// <returns>New list of transitions</returns>
        public static SuperList operator |(SuperscribeNode node, SuperscribeNode other)
        {
            return new SuperList { node, other };
        }

        /// <summary>
        /// Shorthand for adding a transition to an existing list 
        /// </summary>
        /// <param name="states">List of transitions</param>
        /// <param name="other">Alternative transition</param>
        /// <returns>Modified list of transitions</returns>
        public static SuperList operator |(SuperList states, SuperscribeNode other)
        {
            states.Add(other);
            return states;
        }

        /// <summary>
        /// Shorthand for calling .Base
        /// </summary>
        /// <param name="node">Current state</param>
        /// <returns>The state at the base of the current transition chain</returns>
        public static SuperscribeNode operator +(SuperscribeNode node)
        {
            return node.Base();
        }

        /// <summary>
        /// Shorthand for calling .Optional
        /// </summary>
        /// <param name="node">Transition to be made optional</param>
        public static SuperscribeNode operator -(SuperscribeNode node)
        {
            node.Optional();
            return node;
        }

        /// <summary>
        /// Shorthand for calling .MatchAsRegex
        /// </summary>
        /// <param name="node">Transition to be made optional</param>
        public static SuperscribeNode operator ~(SuperscribeNode node)
        {
            node.MatchAsRegex();
            return node;
        }

        public static SuperscribeNode operator *(SuperscribeNode node, Action<RouteData> action)
        {
            node.FinalFunction = action;
            return node;
        }

        /// <summary>
        /// Implicit conversion between string and a ConstantState
        /// </summary>
        /// <param name="value">String to create ConstantState from</param>
        public static implicit operator SuperscribeNode(string value)
        {
            return Superscribe.ʃ.Constant(value);
        }

        #endregion
    }
}
