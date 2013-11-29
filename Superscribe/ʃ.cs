namespace Superscribe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Superscribe.Models;
    using Superscribe.Utils;

    /// <summary>
    /// Superscribe helper class
    /// </summary>
    public class ʃ
    {
        public static GraphNode Base = new GraphNode();

        #region Static Methods

        /// <summary>
        /// Matches any valid identifier and sets ControllerName
        /// </summary>
        public static ControllerNode Controller
        {
            get
            {
                return new ControllerNode { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }

        /// <summary>
        /// Matches any valid identifier and sets ActionName
        /// </summary>
        public static ActionNode Action
        {
            get
            {
                return new ActionNode { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }

        public static RouteGlue ʅ
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
