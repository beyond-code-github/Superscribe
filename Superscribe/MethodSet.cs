namespace Superscribe
{
    using System;
    using System.Collections.Generic;

    using Superscribe.Engine;
    using Superscribe.Models;

    public class MethodSet<T>
        where T: IModuleRouteData
    {
        private readonly string method;

        private IRouteEngine engine;

        private readonly List<Func<GraphNode>> bindings = new List<Func<GraphNode>>();

        public void Initialise(IRouteEngine routeEngine)
        {
            this.engine = routeEngine;

            foreach (var binding in bindings)
            {
                this.ApplyBinding(binding);    
            }
        }

        public MethodSet(string method)
        {
            this.method = method;
        }

        public Func<T, object> this[string s]
        {   
            set
            {
                if (s == "/")
                {
                    engine.Base.FinalFunctions.Add(new FinalFunction(this.method, f => value(f)));
                }
                else
                {
                    var node = new ConstantNode(s);
                    this.bindings.Add(() => node * (f => value(f)));
                }
            }
        }

        public Func<T, object> this[GraphNode s]
        {
            set
            {
                s = s * (f => value(f));
                this.bindings.Add(() => s.Base());
            }
        }

        private void ApplyBinding(Func<GraphNode> o)
        {
            var leaf = o();
            leaf.AddAllowedMethod(this.method);
            this.engine.Base.Zip(leaf.Base());
        }
    }
}
