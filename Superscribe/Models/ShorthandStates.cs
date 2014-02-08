namespace Superscribe.Models
{
    public class Int : ParamNode<int>
    {
        public Int(string name)
            : base(name)
        {
        }

        public static explicit operator Int(string name)
        {
            return new Int(name);
        }
    }

    public class Long : ParamNode<long>
    {
        public Long(string name)
            : base(name)
        {
        }

        public static explicit operator Long(string name)
        {
            return new Long(name);
        }
    }

    public class Bool : ParamNode<bool>
    {
        public Bool(string name)
            : base(name)
        {
        }

        public static explicit operator Bool(string name)
        {
            return new Bool(name);
        }
    }

    public class String : ParamNode<string>
    {
        public String(string name)
            : base(name)
        {
        }

        public static explicit operator String(string name)
        {
            return new String(name);
        }
    }
}
