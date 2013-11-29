namespace Superscribe.Utils
{
    using System.Collections.Generic;

    using Superscribe.Models;

    public class SuperList : List<GraphNode>
    {
        public static GraphNode operator /(string name, SuperList others)
        {
            var state = new ConstantNode(name);
            foreach (var s in others)
            {
                state.Slash(s);
            }

            return state;
        }

        public static SuperList operator |(SuperList first, SuperList second)
        {
            first.AddRange(second);
            return first;
        }
    }
}
