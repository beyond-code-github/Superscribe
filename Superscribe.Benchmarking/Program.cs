namespace Superscribe.Testing.Console
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Web.Http;

    using global::WebAPI.Testing;

    class Program
    {
        static void Main(string[] args)
        {
            //var output = new CsvFileWriter("output-regular.csv");
            //var outputSuper = new CsvFileWriter("output-super.csv");

            for (var i = 0; i < 200; i++)
            {
                HitSuper("/sites/123/portfolio/projects");

                //var row = new CsvRow();

                //row.Add(Hit("/sites/123/portfolio/projects"));
                //row.Add(Hit("/sites/123/portfolio/projects/123"));
                //row.Add(Hit("/sites/123/portfolio/projects/123/media"));
                //row.Add(Hit("/sites/123/portfolio/projects/123/media/123"));
                //row.Add(Hit("/sites/123/portfolio/tags"));
                //row.Add(Hit("/sites/123/portfolio/categories"));
                //row.Add(Hit("/sites/123/portfolio/categories/123"));
                //row.Add(Hit("/sites/123/blog/posts/"));
                //row.Add(Hit("/sites/123/blog/posts/123"));
                //row.Add(Hit("/sites/123/blog/posts/123/media"));
                //row.Add(Hit("/sites/123/blog/posts/123/media/123"));
                //row.Add(Hit("/sites/123/blog/tags"));
                //row.Add(Hit("/sites/123/blog/posts/archives"));
                //row.Add(Hit("/sites/123/blog/posts/archives/2013/12"));

                //output.WriteRow(row);

                var superrow = new CsvRow();

                superrow.Add(HitSuper("/sites/123/portfolio/projects"));
                superrow.Add(HitSuper("/sites/123/portfolio/projects/123"));
                superrow.Add(HitSuper("/sites/123/portfolio/projects/123/media"));
                superrow.Add(HitSuper("/sites/123/portfolio/projects/123/media/123"));
                superrow.Add(HitSuper("/sites/123/portfolio/tags"));
                superrow.Add(HitSuper("/sites/123/portfolio/categories"));
                superrow.Add(HitSuper("/sites/123/portfolio/categories/123"));
                superrow.Add(HitSuper("/sites/123/blog/posts/"));
                superrow.Add(HitSuper("/sites/123/blog/posts/123"));
                superrow.Add(HitSuper("/sites/123/blog/posts/123/media"));
                superrow.Add(HitSuper("/sites/123/blog/posts/123/media/123"));
                superrow.Add(HitSuper("/sites/123/blog/tags"));
                superrow.Add(HitSuper("/sites/123/blog/posts/archives"));
                superrow.Add(HitSuper("/sites/123/blog/posts/archives/2013/12"));

                //outputSuper.WriteRow(superrow);
            }

            //output.Dispose();
            //outputSuper.Dispose();
        }

        private static string Hit(string path)
        {
            var config = new HttpConfiguration();
            RouteConfig.Register(config);

            return Hit(config, path);
        }

        private static string HitSuper(string path)
        {
            var config = new HttpConfiguration();
            SuperscribeTestConfig.Register(config);

            return Hit(config, path);
        }

        private static string Hit(HttpConfiguration config, string path)
        {
            var browser = new Browser(config);

            //var timer = new Stopwatch();
            //timer.Start();

            var response = browser.Get(path,
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            //timer.Stop();
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    //Console.WriteLine(timer.ElapsedTicks);
            //}

            return string.Empty; //timer.ElapsedTicks.ToString();
        }
    }
}
