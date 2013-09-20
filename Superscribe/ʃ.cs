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
        public static SuperscribeNode Base = new SuperscribeNode();

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
        
        public static object ʅ
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Matches a string constant and performs no action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        public static ConstantNode Constant(string value)
        {
            return new ConstantNode(value);
        }

        /// <summary>
        /// Matches a string constant and performs a custom action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        /// <param name="configure">Action to execute</param>
        public static ConstantNode Constant(string value, Action<ConstantNode> configure)
        {
            var result = new ConstantNode(value);
            configure(result);
            return result;
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃInt Int(string name)
        {
            return new ʃInt(name) { Pattern = new Regex("[0-9]+", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃLong Long(string name)
        {
            return new ʃLong(name) { Pattern = new Regex("[0-9]+", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a boolean and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃBool Bool(string name)
        {
            return new ʃBool(name) { Pattern = new Regex("(true|false)", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a string and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃString String(string name)
        {
            return new ʃString(name) { Pattern = new Regex("", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static SuperscribeNode Route(Func<SuperscribeNode> config)
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
        public static SuperscribeNode Route(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue());
            Base.Zip(leaf.Base());
            return leaf;
        }

        public static SuperscribeNode Route(Func<RouteGlue, SuperList> config)
        {
            var list = config(new RouteGlue());
            foreach (var node in list)
            {
                Base.Zip(node.Base());    
            }
            
            return Base;
        }

        public static SuperscribeNode Route(Func<RouteGlue, Func<dynamic, object>> config)
        {
            var final = config(new RouteGlue());
            Base.FinalFunctions.Add(new FinalFunction { Function = final });
            return Base;
        }

        public static SuperscribeNode Get(Func<SuperscribeNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("GET");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Get(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue("GET"));
            leaf.AddAllowedMethod("GET");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Post(Func<SuperscribeNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("POST");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Post(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue("POST"));
            leaf.AddAllowedMethod("POST");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Put(Func<SuperscribeNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("PUT");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Put(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue("PUT"));
            leaf.AddAllowedMethod("PUT");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Patch(Func<SuperscribeNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("PATCH");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Patch(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue("PATCH"));
            leaf.AddAllowedMethod("PATCH");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Delete(Func<SuperscribeNode> config)
        {
            var leaf = config();
            leaf.AddAllowedMethod("DELETE");
            Base.Zip(leaf.Base());

            return leaf;
        }

        public static SuperscribeNode Delete(Func<RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(new RouteGlue("DELETE"));
            leaf.AddAllowedMethod("DELETE");
            Base.Zip(leaf.Base());

            return leaf;
        }

        #endregion

        public static void Reset()
        {
            Base = new SuperscribeNode();
        }
    }
}
