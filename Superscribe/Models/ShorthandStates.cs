namespace Superscribe.Models
{
    using System;

    using Superscribe.Utils;

    public class NonConsumingState : SuperscribeState
    {

    }

    public class NonConsumingState<T> : NonConsumingState
    {
        public static NonConsumingState<T> operator ^(NonConsumingState<T> state, DecisionList<T> other)
        {
            foreach (var decision in other)
            {
                decision.Parent = state;
                state.Transitions.Enqueue(decision);
            }

            return state;
        }

        public static NonConsumingState<T> operator ^(NonConsumingState<T> state, Action<RouteData> other)
        {
            state.OnComplete = other;
            return state;
        }

        public static DecisionList<T> operator |(NonConsumingState<T> state, NonConsumingState<T> other)
        {
            return new DecisionList<T> { state, other };
        }

        public void SetMatchFromParentValue(Predicate<T> other)
        {
            this.IsMatch = s => other((T)this.Parent.Result);
        }
    }

    public class NullState
    {
        public static SuperscribeState operator /(NullState state, string other)
        {
            return ʃ.Constant(other);
        }

        public static SuperscribeState operator /(NullState state, SuperscribeState other)
        {
            return other.Base();
        }

        public static NonConsumingState<double> operator -(NullState state, Func<RouteData, string, double> other)
        {
            var nonConsuming = new NonConsumingState<double>();
            nonConsuming.IsMatch = s => true;
            nonConsuming.Command = (data, segment) => nonConsuming.Result = other(data, segment);
            return nonConsuming;
        }

        public static NonConsumingState<double> operator -(NullState state, Predicate<double> other)
        {
            var nonConsuming = new NonConsumingState<double>();
            nonConsuming.SetMatchFromParentValue(other);
            return nonConsuming;
        }
    }

    public class ʃInt : ParamState<int>
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

    public class ʃLong : ParamState<long>
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

    public class ʃBool : ParamState<bool>
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

    public class ʃString : ParamState<string>
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
