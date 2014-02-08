namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class MethodSet<T>
    {
        private readonly Action<Func<GraphNode>> binding;

        private readonly string method;

        public MethodSet(Action<Func<GraphNode>> binding, string method)
        {
            this.binding = binding;
            this.method = method;
        }

        public Func<T, object> this[string s]
        {   
            set
            {
                if (s == "/")
                {
                    Define.Base.FinalFunctions.Add(new FinalFunction(this.method, f => value(f)));
                }
                else
                {
                    var node = new ConstantNode(s);
                    this.binding(() => node * (f => value(f)));    
                }
            }
        }

        public Func<T, object> this[GraphNode s]
        {
            set
            {
                s = s * (f => value(f));
                this.binding(() => s.Base());
            }
        }
    }
}
