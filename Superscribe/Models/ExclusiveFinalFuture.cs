namespace Superscribe.Models
{
    using System;

    public class ExclusiveFinalFuture
    {
        public SuperscribeNode Node { get; set; }

        public static ExclusiveFinalFuture operator *(SuperscribeNode node, ExclusiveFinalFuture final)
        {
            final.Node = node;
            return final;
        }
        
        public static SuperscribeNode operator *(ExclusiveFinalFuture future, Func<dynamic, object> final)
        {
            future.Node.FinalFunctions.Add(new ExclusiveFinalFunction { Function = o => o.Response = final(o) });
            return future.Node;
        }
    }

    public static class Final
    {
        public static ExclusiveFinalFuture Exclusive { get { return new ExclusiveFinalFuture(); } }
    }
}
