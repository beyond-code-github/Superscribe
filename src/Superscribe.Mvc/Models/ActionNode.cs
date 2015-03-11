namespace Superscribe.WebApi.Models
{
    using Superscribe.Models;

    public class ActionNode : GraphNode
    {
        public ActionNode()
        {
            this.ActionFunctions.Add("Set_action", (data, segment) => data.Environment[Constants.ActionNamePropertyKey] = !string.IsNullOrEmpty(this.ActionName)
                        ? this.ActionName
                        : segment);
        }

        public string ActionName { get; set; }
    }
}
