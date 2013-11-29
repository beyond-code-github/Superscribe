namespace Superscribe.Models
{
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
