namespace Superscribe
{
    using System;
    using System.Collections.Generic;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Models.Filters;
    using Superscribe.Utils;

    public class MethodSet<T>
        where T : IModuleRouteData
    {
        private readonly List<Func<GraphNode>> bindings = new List<Func<GraphNode>>();

        private readonly List<FinalFunction> baseFinals = new List<FinalFunction>();

        private readonly string method;

        private IRouteEngine engine;

        private IStringRouteParser parser;

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
                    this.baseFinals.Add(new ExclusiveFinalFunction(f => value(f), new MethodFilter(this.method)));
                    
                }
                else
                {
                    this.bindings.Add(
                        () =>
                            {
                                var node = this.parser.MapToGraph(s);
                                node.FinalFunctions.Add(new ExclusiveFinalFunction(f => value(f), new MethodFilter(this.method)));
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

        public void Initialise(IRouteEngine routeEngine)
        {
            this.engine = routeEngine;
            this.parser = routeEngine.Config.StringRouteParser;

            foreach (var final in this.baseFinals)
            {
                this.engine.Base.FinalFunctions.Add(final);
            }

            foreach (var binding in this.bindings)
            {
                this.ApplyBinding(binding);    
            }
        }

        private void ApplyBinding(Func<GraphNode> o)
        {
            var leaf = o();
            this.engine.Base.Zip(leaf.Base());
        }
    }
}
