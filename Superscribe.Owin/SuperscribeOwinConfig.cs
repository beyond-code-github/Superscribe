namespace Superscribe.Owin
{
    using System.Collections.Generic;

    public class SuperscribeOwinConfig
    {
        public SuperscribeOwinConfig()
        {
            this.MediaTypeHandlers = new Dictionary<string, MediaTypeHandler>();
        }

        public Dictionary<string, MediaTypeHandler> MediaTypeHandlers { get; set; }
    }
}
