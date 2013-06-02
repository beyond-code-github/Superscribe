namespace Superscribe.Utils
{
    using System.Collections.Generic;

    using Superscribe.Models;

    public class SuperList : List<SuperscribeState>
    {
        public static SuperscribeState operator /(string name, SuperList others)
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
