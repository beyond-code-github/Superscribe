namespace Superscribe.ScriptCS
{
    using System;

    using global::Superscribe.Owin;

    using Microsoft.Owin.Hosting;

    using ScriptCs.Contracts;

    public class Superscribe : IScriptPackContext
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
