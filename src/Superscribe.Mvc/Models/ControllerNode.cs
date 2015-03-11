namespace Superscribe.WebApi.Models
{
    using Superscribe.Models;

    public class ControllerNode : ConstantNode
    {
        public ControllerNode(string value)
            : base(value)
        {
            this.ActionFunctions.Add("Set_controler", (data, segment) => data.Environment[Constants.ControllerNamePropertyKey] = !string.IsNullOrEmpty(this.ControllerName)
                        ? this.ControllerName
                        : segment);
        }

        public string ControllerName { get; set; }
    }
}
