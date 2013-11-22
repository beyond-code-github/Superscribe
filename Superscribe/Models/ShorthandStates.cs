namespace Superscribe.Models
{
    using System;

    using Superscribe.Utils;

    public class NonConsumingNode : SuperscribeNode
    {

    }

    public class NonConsumingNode<T> : NonConsumingNode
    {
        public static NonConsumingNode<T> operator *(NonConsumingNode<T> node, DecisionList<T> other)
        {
            foreach (var decision in other)
            {
                decision.Parent = node;
                node.Edges.Enqueue(decision);
            }

            return node;
        }

        public static NonConsumingNode<T> operator *(NonConsumingNode<T> node, Func<dynamic, object> other)
        {
            node.FinalFunctions.Add(new FinalFunction { Function = other });
            return node;
        }

        public static DecisionList<T> operator |(NonConsumingNode<T> node, NonConsumingNode<T> other)
        {
            return new DecisionList<T> { node, other };
        }

        public void SetMatchFromParentValue(Predicate<T> other)
        {
            this.ActivationFunction = (routedata, s) => other((T)this.Parent.Result);
        }
    }

    public class ʃInt : ParamNode<int>
    {
        public ʃInt(string name)
            : base(name)
        {
        }

        public static explicit operator ʃInt(string name)
        {
            return new ʃInt(name);
        }
    }

    public class ʃLong : ParamNode<long>
    {
        public ʃLong(string name)
            : base(name)
        {
        }

        public static explicit operator ʃLong(string name)
        {
            return new ʃLong(name);
        }
    }

    public class ʃBool : ParamNode<bool>
    {
        public ʃBool(string name)
            : base(name)
        {
        }

        public static explicit operator ʃBool(string name)
        {
            return new ʃBool(name);
        }
    }

    public class ʃString : ParamNode<string>
    {
        public ʃString(string name)
            : base(name)
        {
        }

        public static explicit operator ʃString(string name)
        {
            return new ʃString(name);
        }
    }
}
