namespace Superscribe.Models
{
    using System.Text.RegularExpressions;

    public class ActionState : ʃ
    {
        public ActionState()
        {
            this.Pattern = new Regex("([a-z]|[A-Z]|[0-9])+");
        }
    }
}
