namespace Superscribe.Owin
{
    using System.Collections.Generic;

    public class SuperscribeOwinConfig
    {
        public SuperscribeOwinConfig()
        {
            this.ContentHandlers = new Dictionary<string, ContentHandler>();
        }

        public Dictionary<string, ContentHandler> ContentHandlers { get; set; }
    }
}
