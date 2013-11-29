namespace Superscribe.Models
{
    public class ControllerNode : GraphNode
    {
        public ControllerNode()
        {
            this.ActionFunction = (data, segment) => data.ControllerName = !string.IsNullOrEmpty(this.ControllerName)
                        ? this.ControllerName
                        : segment;
        }

        public string ControllerName { get; set; }
    }
}
