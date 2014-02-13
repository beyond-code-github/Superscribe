namespace Superscribe.WebApi.Models
{
    using Superscribe.Models;

    public class ActionNode : GraphNode
    {
        public ActionNode()
        {
            this.ActionFunction = (data, segment) => data.Environment[Constants.ActionNamePropertyKey] = !string.IsNullOrEmpty(this.ActionName)
                        ? this.ActionName
                        : segment;
        }

        public string ActionName { get; set; }
    }
}
