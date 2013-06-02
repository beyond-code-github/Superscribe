namespace Superscribe.Models
{
    using System.Text.RegularExpressions;

    public class ConstantState : SuperscribeState
    {
        public ConstantState(string value)
        {
            this.Pattern = new Regex(value);
        }
    }
}
