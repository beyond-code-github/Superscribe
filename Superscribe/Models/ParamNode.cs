namespace Superscribe.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public abstract class ParamNode : GraphNode
    {
        private object value;

        protected ParamNode()
        {
            this.actionFunction = (data, segment) => data.Parameters.Add(this.Name, this.value);

            this.activationFunction = (data, segment) =>
            {
                if (!this.TryParse(segment, out this.value))
                {
                    data.ParamConversionError = true;
                    return false;
                }

                return true;
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
