namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class MethodSet
    {
        private readonly Action<Func<SuperscribeNode, SuperscribeNode>> binding;

        public MethodSet(Action<Func<SuperscribeNode, SuperscribeNode>> binding)
        {
            this.binding = binding;
        }

        public Action<RouteData> this[string s]
        {
            set
            {
                if (s == "/")
                {
                    this.binding(o => o * value);   
                }
                else
                {
                    this.binding(o => o / s * value);    
                }
            }
        }

        public Action<RouteData> this[SuperscribeNode s]
        {
            set
            {
                ʃ.Get(o => o / s.Base() * value);
            }
        }
    }
}
