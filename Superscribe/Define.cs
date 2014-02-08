namespace Superscribe
{
    using System;
    using System.Text.RegularExpressions;

    using Superscribe.Models;
    using Superscribe.Utils;

    /// <summary>
    /// Superscribe shorthand
    /// </summary>
    public class ʃ : Define
    {
    }

    /// <summary>
    /// Superscribe helper class
    /// </summary>
    public class Define
    {
        public static GraphNode Base = new GraphNode();

        #region Static Methods

        public static RouteGlue Glue
        {
            get
            {
                return new RouteGlue();
            }
        }
        
        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static GraphNode Route(Func<GraphNode> config)
        {
            var leaf = config();
            Base.Zip(leaf.Base());
            return leaf;
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static GraphNode Route(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue());
            Base.Zip(leaf.Base());
            return leaf;
        }

        public static GraphNode Route(Func<RouteGlue, SuperList> config)
        {
            var list = config(new RouteGlue());
            foreach (var node in list)
            {
                Base.Zip(node.Base());    
            }
            
            return Base;
        }

        public static GraphNode Route(Func<RouteGlue, Func<dynamic, object>> config)
        {
            var final = config(new RouteGlue());
            Base.FinalFunctions.Add(new FinalFunction { Function = final });
            return Base;
        }

        public static GraphNode Get(Func<GraphNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("GET");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Get(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("GET"));
            leaf.AddAllowedMethod("GET");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Post(Func<GraphNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("POST");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Post(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("POST"));
            leaf.AddAllowedMethod("POST");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Put(Func<GraphNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("PUT");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Put(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("PUT"));
            leaf.AddAllowedMethod("PUT");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Patch(Func<GraphNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("PATCH");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Patch(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("PATCH"));
            leaf.AddAllowedMethod("PATCH");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Delete(Func<GraphNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("DELETE");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static GraphNode Delete(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("DELETE"));
            leaf.AddAllowedMethod("DELETE");
            Base.Zip(leaf.Base());

            return leaf;
        }

        #endregion

        public static void Reset()
        {
            Base = new GraphNode();
        }
    }
}
