namespace Superscribe.Testing.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.WebApi;

    using WebAPI.Testing;

    public class SimplifiedSyntaxTestsBase
    {
        protected static Browser browser;

        protected static HttpResponseMessage response;

        protected Establish context = () =>
        {
            var config = new HttpConfiguration();
            var define = SuperscribeConfig.Register(config);

            // Forms
            var forms = define.Route("api" / (Long)"parentId" / "Forms".Controller());
            define.Get(forms / "VisibleFor" / (String)"appDataId", To.Action("VisibleFor"));
            define.Get(forms / -(Long)"id", To.Action("GetById"));
            define.Get(forms / -(Long)"id" / "Render", To.Action("RenderForm"));
            define.Patch(forms / -(Long)"id", To.Action("Patch"));
            define.Delete(forms / -(Long)"id", To.Action("Delete"));

            browser = new Browser(config);
        };
    }

    public class When_hitting_the_collection : SimplifiedSyntaxTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/123/Forms",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200 = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_correct_action = () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_Forms_123\"");
    }

    public class When_hitting_visible_for : SimplifiedSyntaxTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/123/Forms/VisibleFor/abcd",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200 = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_correct_action = () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"VisibleFor_Forms_123_abcd\"");
    }

    public class When_hitting_an_individual_item : SimplifiedSyntaxTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/123/Forms/321",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200 = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_correct_action = () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_Forms_123_321\"");
    }

    public class When_hitting_render_for_an_individual_item : SimplifiedSyntaxTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/123/Forms/321/Render",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200 = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_correct_action = () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"RenderForm_Forms_123_321\"");
    }

    public class When_deleting_an_individual_item : SimplifiedSyntaxTestsBase
    {
        private Because of = () => response = browser.Delete(
            "/api/123/Forms/321",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200 = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_correct_action = () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Delete_Forms_123_321\"");
    }
}
