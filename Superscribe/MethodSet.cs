namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class MethodSet<T>
    {
        private readonly Action<Func<SuperscribeNode>> binding;

        private readonly string method;

        public MethodSet(Action<Func<SuperscribeNode>> binding, string method)
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
                    ʃ.Base.FinalFunctions.Add(new FinalFunction { Function = f => value(f), Method = this.method });
                }
                else
                {
                    var node = ʃ.Constant(s);
                    this.binding(() => node * (f => value(f)));    
                }
            }
        }

        public Func<T, object> this[SuperscribeNode s]
        {
            set
            {
                s = s * (f => value(f));
                this.binding(() => s.Base());
            }
        }
    }
}
