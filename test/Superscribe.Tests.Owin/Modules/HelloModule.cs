namespace Superscribe.Tests.Owin.Modules
{
    public class Module : SuperscribeOwinModule
    {
        public Module()
        {
            this.Get["Hello"] = o => "Hello World";
        }
    }
}