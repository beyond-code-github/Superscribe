namespace Superscribe.Owin
{
    using System.Collections.Generic;

    public class SuperscribeOwinOptions : SuperscribeOptions
    {
        public SuperscribeOwinOptions()
        {
            this.MediaTypeHandlers = new Dictionary<string, MediaTypeHandler>();
            this.ScanForModules = true;
        }

        public Dictionary<string, MediaTypeHandler> MediaTypeHandlers { get; set; }

        public bool ScanForModules { get; set; }
    }
}
