namespace Superscribe.WebApi
{
    using System.Text.RegularExpressions;

    using Superscribe.WebApi.Models;

    public static class Any
    {
        public static ControllerNode Controller
        {
            get
            {
                var node = new ControllerNode("^[a-zA-Z0-9_]*$");
                node.MatchAsRegex();

                return node;
            }
        }

        public static ActionNode Action
        {
            get
            {
                return new ActionNode { Pattern  = new Regex("^[a-zA-Z0-9_]*$") };
            }
        }
    }
}
