namespace Superscribe.Models
{
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
