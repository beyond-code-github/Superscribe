namespace Superscribe.Models
{
    using System;

    using Superscribe.Utils;

    public class NonConsumingNode : SuperscribeNode
    {

    }

    public class NonConsumingNode<T> : NonConsumingNode
    {
        public static NonConsumingNode<T> operator ^(NonConsumingNode<T> node, DecisionList<T> other)
        {
            foreach (var decision in other)
            {
                decision.Parent = node;
                node.Edges.Enqueue(decision);
            }

            return node;
        }

        public static NonConsumingNode<T> operator ^(NonConsumingNode<T> node, Action<RouteData> other)
        {
            node.FinalFunction = other;
            return node;
        }

        public static DecisionList<T> operator |(NonConsumingNode<T> node, NonConsumingNode<T> other)
        {
            return new DecisionList<T> { node, other };
        }

        public void SetMatchFromParentValue(Predicate<T> other)
        {
            this.ActivationFunction = s => other((T)this.Parent.Result);
        }
    }

    public class RouteGlue
    {
        public static SuperscribeNode operator /(RouteGlue state, string other)
        {
            return ʃ.Constant(other);
        }

        public static SuperscribeNode operator /(RouteGlue state, SuperscribeNode other)
        {
            return other.Base();
        }

        public static NonConsumingNode<double> operator -(RouteGlue state, Func<RouteData, string, double> other)
        {
            var nonConsuming = new NonConsumingNode<double>();
            nonConsuming.ActivationFunction = s => true;
            nonConsuming.ActionFunction = (data, segment) => nonConsuming.Result = other(data, segment);
            return nonConsuming;
        }

        public static NonConsumingNode<double> operator -(RouteGlue state, Predicate<double> other)
        {
            var nonConsuming = new NonConsumingNode<double>();
            nonConsuming.SetMatchFromParentValue(other);
            return nonConsuming;
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
            return Superscribe.ʃ.Int(name);
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
            return Superscribe.ʃ.Long(name);
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
            return Superscribe.ʃ.Bool(name);
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
            return Superscribe.ʃ.String(name);
        }
    }
}
