namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class MethodSet<T>
    {
        private readonly Action<Func<SuperscribeNode, SuperscribeNode>> binding;

        public MethodSet(Action<Func<SuperscribeNode, SuperscribeNode>> binding)
        {
            this.binding = binding;
        }

        public Func<T, object> this[string s]
        {   
            set
            {
                if (s == "/")
                {
                    this.binding(o => o * (f => value(f)));   
                }
                else
                {
                    this.binding(o => o / s * (f => value(f)));    
                }
            }
        }

        public Func<dynamic, object> this[SuperscribeNode s]
        {
            set
            {
                ʃ.Get(o => o / s.Base() * value);
            }
        }
    }
}
