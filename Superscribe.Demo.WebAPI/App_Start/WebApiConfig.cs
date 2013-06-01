namespace Superscribe.Demo.WebAPI.App_Start
{
    using System.Linq.Expressions;
    using System.Web.Http;

    using global::Superscribe.Utils;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Superscribe.Register(config);

            ʃ.Route(o => o / "api" / "values".Controller() / (
                  ~"id".Int()
                | "(first|last)".Action()
                | -("foruser" / "userId".Int())));

            //var subController = ʃ.Part(o => o / ʃ.Controller / ʃ.Int("id")).Optional();

            //ʃ.Route(o => o / "api" / ʃ.Controller / ʃ.Int("id") / subController);


            var parameter = Expression.Parameter(typeof(Message), "o");
            var getname = Expression.Property(parameter, "Name");

            var isnullorempty = Expression.Not(Expression.Call(typeof(string), "IsNullOrEmpty", null, getname));
            var compare = Expression.Equal(getname, Expression.Constant("thisvalue"));

            var combined = Expression.And(isnullorempty, compare);

            var lambda = Expression.Lambda(typeof(bool), combined, parameter);




        }
    }

    public class Message
    {
        public string Name { get; set; }

        public string Body { get; set; }
    }
}
