namespace Superscribe.ScriptCS
{
    using System;

    using Microsoft.Owin.Hosting;

    using ScriptCs.Contracts;

    public class SuperscribeCS : IScriptPackContext
    {
        public void Listen()
        {
            var options = new StartOptions
            {
                ServerFactory = "Nowin",
                Port = 1280
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running a http server on port 1280");
                Console.ReadKey();
            }
        }
    }
}
