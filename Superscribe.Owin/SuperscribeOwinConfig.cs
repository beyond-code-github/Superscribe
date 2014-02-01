namespace Superscribe.Owin
{
    using System.Collections.Generic;

    public class SuperscribeOwinConfig
    {
        public SuperscribeOwinConfig()
        {
            this.MediaTypeHandlers = new Dictionary<string, MediaTypeHandler>();
            this.ScanForModules = true;
        }

        public Dictionary<string, MediaTypeHandler> MediaTypeHandlers { get; set; }

        public bool ScanForModules { get; set; }
    }
}
