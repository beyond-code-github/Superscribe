namespace Superscribe.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public abstract class ParamNode : SuperscribeNode
    {
        protected ParamNode()
        {
            this.ActionFunction = (data, segment) =>
            {
                object value;
                var success = this.TryParse(segment, out value);

                if (success)
                {
                    data.Parameters.Add(this.Name, value);
                }
                else
                {
                    data.ParamConversionError = true;
                }
            };
        }

        public abstract bool TryParse(string value, out object obj);

        public abstract Type Type { get; }

        public string Name { get; set; }
    }

    public class ParamNode<T> : ParamNode
    {
        protected ParamNode(string name)
        {
            Name = name;
        }

        public override bool TryParse(string value, out object obj)
        {
            try
            {
                var tc = TypeDescriptor.GetConverter(this.Type);
                obj = tc.ConvertFromString(null, CultureInfo.InvariantCulture, value);
                return true;
            }
            catch (Exception ex)
            {
                obj = null;
                return false;
            }
        }

        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }
    }
}
