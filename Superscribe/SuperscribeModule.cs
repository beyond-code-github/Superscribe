namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class SuperscribeModule
    {
        protected void Get(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            ʃ.Route(config);
        }

        protected void Post(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            ʃ.Route(config);
        }

        protected void Put(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            ʃ.Route(config);
        }

        protected void Delete(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            ʃ.Route(config);
        }

        protected void Patch(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            ʃ.Route(config);
        }
    }
}
