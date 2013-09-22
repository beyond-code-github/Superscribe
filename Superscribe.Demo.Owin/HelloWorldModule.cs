namespace Superscribe.Demo.Owin
{
    using Superscribe.Owin;

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = _ => "Hello world";

            this.Get["Test"] = _ => new { Message = "Hello World" };

            this.Post["Test"] = _ =>
                {
                    var product = _.Bind<Product>();

                    _.StatusCode = 201;
                    return new { Message = string.Format("Received product {0}", product.Name) };
                };
        }
    }
}