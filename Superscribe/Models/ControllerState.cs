namespace Superscribe.Models
{
    public class ControllerState : SuperscribeState
    {
        public ControllerState()
        {
            this.Command = (data, segment) => data.ControllerName = !string.IsNullOrEmpty(this.ControllerName)
                        ? this.ControllerName
                        : segment;
        }

        public string ControllerName { get; set; }
    }
}
