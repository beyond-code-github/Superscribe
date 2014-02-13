namespace Superscribe.WebApi.Models
{
    using Superscribe.Models;

    public class ControllerNode : GraphNode
    {
        public ControllerNode()
        {
            this.ActionFunction = (data, segment) => data.Environment[Constants.ControllerNamePropertyKey] = !string.IsNullOrEmpty(this.ControllerName)
                        ? this.ControllerName
                        : segment;
        }

        public string ControllerName { get; set; }
    }
}
