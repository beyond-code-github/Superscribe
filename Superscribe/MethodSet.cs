namespace Superscribe
{
    using System;
    using System.Collections.Generic;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Utils;

    public class MethodSet<T>
        where T: IModuleRouteData
    {
        private readonly string method;

        private IRouteEngine engine;

        private IStringRouteParser parser;

        private readonly List<Func<GraphNode>> bindings = new List<Func<GraphNode>>();

        private readonly List<FinalFunction> baseFinals = new List<FinalFunction>();

        public void Initialise(IRouteEngine routeEngine)
        {
            this.engine = routeEngine;
            this.parser = routeEngine.Config.StringRouteParser;

            foreach (var final in this.baseFinals)
            {
                engine.Base.FinalFunctions.Add(final);
            }

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
                    this.baseFinals.Add(new ExclusiveFinalFunction(this.method, f => value(f)));
                    
                }
                else
                {
                    this.bindings.Add(
                        () =>
                            {
                                var node = parser.MapToGraph(s);
                                node.FinalFunctions.Add(new ExclusiveFinalFunction(this.method, f => value(f)));
                                return node;
                            });
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
            this.engine.Base.Zip(leaf.Base());
        }
    }
}
