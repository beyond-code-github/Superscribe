namespace Superscribe.Models
{
    public class ConstantNode : GraphNode
    {
        public ConstantNode(string value)
        {
            this.Template = value;
        }
    }
}
