namespace Superscribe.Models
{
    public class ActionNode : SuperscribeNode
    {
        public ActionNode()
        {
            this.ActionFunction = (data, segment) => data.ActionName = !string.IsNullOrEmpty(this.ActionName)
                        ? this.ActionName
                        : segment;
        }

        public string ActionName { get; set; }
    }
}
