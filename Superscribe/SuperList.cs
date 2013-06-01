namespace Superscribe
{
    using System;
    using System.Collections.Generic;

    using global::Superscribe.Models;

    public class SuperList : List<ʃ>
    {
        public static ʃ operator /(string name, SuperList others)
        {
            var state = new ConstantState(name);
            foreach (var s in others)
            {
                state.Slash(s);
            }

            return state;
        }
    }
}
