namespace Superscribe.Models
{
    public class ActionState : SuperscribeState
    {
        public ActionState()
        {
            this.Command = (data, segment) => data.ActionName = !string.IsNullOrEmpty(this.ActionName)
                        ? this.ActionName
                        : segment;
        }

        public string ActionName { get; set; }
    }
}
