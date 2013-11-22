namespace Superscribe.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Superscribe.Utils;

    public class SuperscribeNode : IEquatable<SuperscribeNode>
    {
        protected Func<dynamic, string, bool> activationFunction;

        protected Action<dynamic, string> actionFunction;

        /// <summary>
        /// Base constructor for superscribe states
        /// </summary>
        public SuperscribeNode()
        {
            this.Edges = new ConcurrentQueue<SuperscribeNode>();
            this.QueryString = new ConcurrentQueue<SuperscribeNode>();
            this.AllowedMethods = new ConcurrentQueue<string>();
            this.FinalFunctions = new List<FinalFunction>();

            this.activationFunction = (routedata, segment) =>
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

        public Action<dynamic, string> ActionFunction
        {
            get
            {
                return this.actionFunction;
            }
            set
            {
                this.actionFunction = value;
            }
        }

        public List<FinalFunction> FinalFunctions { get; set; }

        public bool ActivationFunctionChanged { set; get; }

        public Func<dynamic, string, bool> ActivationFunction
        {
            get
            {
                return activationFunction;
            }
            set
            {
                this.ActivationFunctionChanged = true;
                this.activationFunction = value;
            }
        }

        public ConcurrentQueue<string> AllowedMethods { get; set; }

        #endregion

        #region Methods

        public SuperscribeNode Zip(SuperscribeNode nextNode)
        {
            nextNode.Parent = this;
            foreach (var existingNode in this.Edges)
            {
                if (existingNode.Equals(nextNode))
                {
                    foreach (var nextEdge in nextNode.Edges)
                    {
                        existingNode.Zip(nextEdge);
                    }

                    return existingNode;
                }
            }

            this.Edges.Enqueue(nextNode);
            return nextNode;
        }

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

        public virtual SuperscribeNode ʃ(Action<dynamic, string> command)
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
            return this.Base(this, this.Parent);
        }

        public void AddAllowedMethod(string method)
        {
            if (!this.AllowedMethods.Contains(method))
            {
                this.AllowedMethods.Enqueue(method);    
            }
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
            return node.Slash(new ConstantNode(other));
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
        /// Allows us to chain activation functions inline with routes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="activation"></param>
        /// <returns></returns>
        public static NodeFuture operator /(SuperscribeNode node, Func<dynamic, string, bool> activation)
        {
            return new NodeFuture { Parent = node, ActivationFunction = activation };
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

        public static SuperscribeNode operator *(SuperscribeNode node, Func<dynamic, object> final)
        {
            node.FinalFunctions.Add(new FinalFunction { Function = o => o.Response = final(o) });
            return node;
        }

        public static SuperscribeNode operator *(SuperscribeNode node, FinalFunctionList finals)
        {
            node.FinalFunctions.AddRange(finals);
            foreach (var final in finals)
            {
                if (!string.IsNullOrEmpty(final.Method))
                {
                    node.AddAllowedMethod(final.Method);
                }
            }

            return node;
        }

        /// <summary>
        /// Implicit conversion between string and a ConstantState
        /// </summary>
        /// <param name="value">String to create ConstantState from</param>
        public static implicit operator SuperscribeNode(string value)
        {
            return new ConstantNode(value);
        }

        #endregion

        #region IEquatable Members

        public bool Equals(SuperscribeNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != this.GetType())
            {
                return false;
            }

            if (this.ActivationFunctionChanged || other.ActivationFunctionChanged)
            {
                return false;
            }
            
            var equal = string.Equals(
                this.Pattern != null ? this.Pattern.ToString() : string.Empty,
                other.Pattern != null ? other.Pattern.ToString() : string.Empty)
                && string.Equals(this.Template, other.Template)
                && this.AllowedMethods.SequenceEqual(other.AllowedMethods)
                && this.FinalFunctions.Select(o => o.Method).SequenceEqual(other.FinalFunctions.Select(o => o.Method));

            return equal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((SuperscribeNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Pattern != null ? this.Pattern.GetHashCode() : 0) * 397) ^ (this.Template != null ? this.Template.GetHashCode() : 0);
            }
        }

        #endregion

    }
}
