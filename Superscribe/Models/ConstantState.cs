namespace Superscribe.Models
{
    public class ConstantState : SuperscribeState
    {
        public ConstantState(string value)
        {
            this.Template = value;
        }
    }
}
