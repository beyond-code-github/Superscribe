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
                return new ControllerNode { Pattern = new Regex("^[a-zA-Z0-9_]*$") };
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
