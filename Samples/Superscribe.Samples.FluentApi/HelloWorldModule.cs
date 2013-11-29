namespace Superscribe.Samples.FluentApi
{
    using Superscribe.Models;
    using Superscribe.Owin;

    public class EvenNumberNode : GraphNode
    {
        public EvenNumberNode(string name)
        {
            this.activationFunction = (routeData, value) => {
                int parsed;
                if (int.TryParse(value, out parsed))
                    return parsed % 2 == 0; // Only match even numbers

                return false;
            };

            this.actionFunction = (routeData, value) => {
                int parsed;
                if (int.TryParse(value, out parsed))
                    routeData.Parameters.Add(name, parsed);                
            };
        }
    }

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
            var helloRoute = new ConstantNode("Products").Slash(new EvenNumberNode("id"));
            helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Product id: " + _.Parameters.id));

            ʃ.Base.Zip(helloRoute.Base());
        }
    }
}