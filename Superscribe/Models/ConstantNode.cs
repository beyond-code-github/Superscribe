namespace Superscribe.Models
{
    public class ConstantNode : SuperscribeNode
    {
        public ConstantNode(string value)
        {
            this.Template = value;
        }
    }
}
