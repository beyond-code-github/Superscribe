---
layout: default
title:  Features
---

<div class="block">
<h2 class="title-divider"><span>Key <span class="de-em">features</span></span>
<small>Superscribe comes with a host of features to help you get the most out of your routing</small>
</h2>
  <div class="tabbable tabs-left vertical-tabs bold-tabs row">
    <ul class="nav nav-tabs nav-stacked col-md-4">
    	<li class="active"> <a href="#webapi" data-toggle="tab">Replacement Asp.Net Web API routing<small>Replace existing routes with syntax thats much more concise and easy to manage</small><i class="icon-angle-right"></i></a> </li>
    	<li> <a href="#testing" data-toggle="tab">Easy unit testing<small>Invoke the superscribe routing engine in isolation from the rest of your app</small><i class="icon-angle-right"></i></a> </li>     	
      	<li> <a href="#modules" data-toggle="tab">Bring Nancy style modules to Web API<small>All the benefits of the code-centric approach combined with graph based routing</small><i class="icon-angle-right"></i></a> </li>   
        <li> <a href="#routingasmiddleware" data-toggle="tab">Routing as middleware<small>Perform routing once, then re-use the results in any compatible middleware or framework</small><i class="icon-angle-right"></i></a> </li>
      	<li> <a href="#owinpipeline" data-toggle="tab">OWIN pipeline branching<small>Let your routing choose which middleware to include</small><i class="icon-angle-right"></i></a> </li>
      	<li> <a href="#owinhandler" data-toggle="tab">Handle requests directly in OWIN<small>Implement lean endpoints with no need for a web framework</small><i class="icon-angle-right"></i></a> </li>
    </ul>
	<div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="webapi">
      	<h3>Simplify your Asp.Net Web API Routes</h3>
      	<p>Routing in Web API is based on legacy MVC logic, and although Attribute Routing improves things greatly it's still not a catch-all fix. Many route combinations are very difficult to implement, such as multiple actions, with the same parameters, mapped to the same http verbs. Superscribe solves all these problems by allowing you to be more descriptive with much less code.</p>
      	<p>Here's a comparison of Web API verus Superscribe for an app that serves the following urls:</p>
      	<pre class="prettyprint lang-cs">

	// /sites/{siteId}/portfolio/projects
	// /sites/{siteId}/portfolio/projects/{projectId}
	// /sites/{siteId}/portfolio/projects/{projectId}/media
	// /sites/{siteId}/portfolio/projects/{projectId}/media/{id}
	// /sites/{siteId}/portfolio/tags
	// /sites/{siteId}/portfolio/categories
	// /sites/{siteId}/portfolio/categories/{id}
	// /sites/{siteId}/blog/posts/
	// /sites/{siteId}/blog/posts/{postId}
	// /sites/{siteId}/blog/posts/{postId}/media
	// /sites/{siteId}/blog/posts/{postId}/media/{id}
	// /sites/{siteId}/blog/tags
	// /sites/{siteId}/blog/posts/archives
	// /sites/{siteId}/blog/posts/archives/{year}/{month}
		</pre>
		<h3 class="title visible-phone">Traditional Web API:</h3>
		<pre class="prettyprint lang-cs">
	 
	config.Routes.MapHttpRoute(
		name: "PortfolioTagsRoute",
		routeTemplate: "sites/{siteId}/portfolio/tags",
		defaults: new { controller = "portfoliotags" }
	);
	 
	config.Routes.MapHttpRoute(
		name: "PortfolioProjectsRoute",
		routeTemplate: "sites/{siteId}/portfolio/projects/{id}",
		defaults: new { controller = "portfolioprojects", id = RouteParameter.Optional }
	);

	config.Routes.MapHttpRoute(
		name: "PortfolioProjectMediaRoute",
		routeTemplate: "sites/{siteId}/portfolio/projects/{projectId}/media/{id}",
		defaults: new { controller = "portfolioprojectmedia", id = RouteParameter.Optional }
	);
	 
	config.Routes.MapHttpRoute(
		name: "PortfolioCategoriesRoute",
		routeTemplate: "sites/{siteId}/portfolio/categories/{id}",
		defaults: new { controller = "portfoliocategories", id = RouteParameter.Optional }
	);
	 
	config.Routes.MapHttpRoute(
		name: "BlogPostMediaRoute",
		routeTemplate: "sites/{siteId}/blog/posts/{postId}/media/{id}",
		defaults: new { controller = "blogpostmedia", id = RouteParameter.Optional }
	);
	 
	config.Routes.MapHttpRoute(
		name: "BlogTagsRoute",
		routeTemplate: "sites/{siteId}/blog/tags",
		defaults: new { controller = "blogtags" }
	);
	 
	config.Routes.MapHttpRoute(
		name: "BlogPostArchiveRoute",
		routeTemplate: "sites/{siteId}/blog/posts/archives/{year}/{month}",
		defaults: new { controller = "blogpostarchives" }
	);
	 
	config.Routes.MapHttpRoute(
		name: "BlogPostArchivesRoute",
		routeTemplate: "sites/{siteId}/blog/posts/archives",
		defaults: new { controller = "blogpostarchives" }
	);
	 
	config.Routes.MapHttpRoute(
		name: "BlogPostsRoute",
		routeTemplate: "sites/{siteId}/blog/posts/{id}",
		defaults: new { controller = "blogposts", id = RouteParameter.Optional }
	);
		</pre>
		<h3 class="title visible-phone">Superscribe:</h3>
        <pre class="prettyprint lang-cs">

    var blog        = engine.Route(r => r / "sites" / (Int)"siteId" / "blog");
    var portfolio   = engine.Route(r => r / "sites" / (Int)"siteId" / "portfolio");
    var blogposts   = engine.Route(blog / "posts".Controller("blogposts"));

    engine.Route(portfolio / "tags".Controller("portfoliotags"));
    engine.Route(portfolio / "categories".Controller("portfoliocategories") / (Int)"id");
    engine.Route(portfolio / "projects".Controller("portfolioprojects") 
        / (Int)"projectId" / "media".Controller("portfolioprojectmedia") / (Int)"id");

    engine.Route(blog / "tags".Controller("blogtags"));

    engine.Route(blogposts / (Int)"postId" / "media".Controller("blogpostmedia") 
        / (Int)"id");

    engine.Route(blogposts / "archives".Controller("blogpostarchives") / (Int)"year" 
        / (Int)"month");

		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="testing">
        <h3>Test your route handlers in isolation</h3>
        <p>The Superscribe engine is built in a way which means it's easy to parse urls without invoking the rest of your application using the Route Walker. Provided you have implemented your handlers in a decoupled fashion, you can add route definitions, perform tests on your matching, then reset back to scratch for the next iteration with minimal fuss</p>
        <pre class="prettyprint lang-cs">

    [TestClass]
    public class SuperscribeUnitTests
    {
        private IRouteEngine define;
            
        [TestInitialize]
        public void Setup()
        {
            define = RouteEngineFactory.Create();

            define.Get("Hello/World", o => "Hello World!");
            define.Get("Hello" / (String)"Name", o => "Hello " + o.Parameters.Name);
        }
        
        [TestMethod]
        public void Test_Hello_World_Get()
        {
            var routeWalker = define.Walker();
            var data = routeWalker.WalkRoute("/Hello/World", "Get", new RouteData());

            Assert.AreEqual("Hello World!", data.Response);
        }

        [TestMethod]
        public void Test_Hello_Name_Get()
        {
            var routeWalker = define.Walker();
            var data = routeWalker.WalkRoute("/Hello/Kathryn", "Get", new RouteData());

            Assert.AreEqual("Kathryn", data.Parameters.Name);
            Assert.AreEqual("Hello Kathryn", data.Response);
        }
    }
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="modules">
        <h3>Module style route handlers in Asp.Net Web Api</h3>
        <p>Attribute routing in Asp.Net is pretty useful, but it still constrains you to the traditional construct of Controller\Action. With very little effort you can now break the mold and use Nancy\Sinatra style modules, complete with model binding and dependency injection.</p>
        <p>If you like to define your routes close to where they're handled then this is the solution for you, and of course you still get all the benefits of both Web Api and Graph Based Routing.</p>
        <h3 class="title visible-phone">Setup</h3>
        <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.RegisterModules(config);
        }
    }
		</pre>
		<h3 class="title visible-phone">Add a module</h3>
        <pre class="prettyprint lang-cs">

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = o => "Hello World!";

        	this.Get["Hello" / (String)"Name"] = 
          		o => string.Format("Hello {0}", o.Parameters.Name);

            this.Post["Save"] = o =>
            {
                var wrapper = o.Bind&lt;MessageWrapper&gt;();
                return new { Message = "You entered - " + wrapper.Message };
            };
        }
    }
		</pre>
	  </div>	
      <div class="tab-pane col-sm-12 col-md-12" id="routingasmiddleware">
        <h3>Don't do any more route parsing than neccesary</h3>
        <p>
            Modern web applications are composed of multiple components, sometimes even multiple frameworks and many of these need to perform routing of some kind. With Superscribe, you can perform your routing once at the start of your pipeline and then re-use the results in your application wherever you need them.
        </p>
        <pre class="prettyprint lang-cs">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
            var httpconfig = new HttpConfiguration();

            SuperscribeConfig.Register(httpconfig, define);

            define.Route("Values".Controller());
            define.Route("Admin" / "Users".Controller());
            define.Route("Admin" / "Teams".Controller());
            
            // Use Superscribe to perform routing at the start of the pipeline...
            app.UseSuperscribeRouter(define)
                .UseWebApi(httpconfig)
                .WithSuperscribe(httpconfig, define);
            // ...then tell web api or other middleware to reuse the results
        }
    }
        </pre></div>    
      <div class="tab-pane col-sm-12 col-md-12" id="owinpipeline">
        <h3>Combine your routing and OWIN pipelines</h3>
        <p>
        	With the Superscribe.Owin package, your routing routing pipeline and your OWIN pipeline become one. Superscribe provides <em>Superscribe Router</em> for middleware routing and branching.
        </p>
        <h3 class="title">Superscribe Router</h3>
        <pre class="prettyprint lang-cs">

 	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			var httpconfig = new HttpConfiguration();
            httpconfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/webapi/",
                defaults: new { controller = "Hello" }
            );

            var define = OwinRouteEngineFactory.Create();

            // Set up a route that will forward requests to either web api or nancy
            define.Pipeline("api/webapi").UseWebApi(httpconfig);
            define.Pipeline("api/nancy").UseNancy();
            
            app.UseSuperscribeRouter(define);
		}
	}
		</pre></div>	
      <div class="tab-pane col-sm-12 col-md-12" id="owinhandler">
        <h3>Handle requests directly in OWIN</h3>
        <p>
        	For those situations when simplicity and raw speed are key, <em>Superscribe Handler</em> allows you to respond to requests without the need for a bulky web framework.
        </p>
		<h3 class="title">Superscribe Handler</h3>
        <pre class="prettyprint lang-cs">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        	var define = OwinRouteEngineFactory.Create();
            
            define.Get("Hello/World", o => "Hello World");
            
            app.UseSuperscribeRouter(define)
                .UseSuperscribeHandler(define);
		}
	}
		</pre>
        <p>
            You can use modules to respond to OWIN requests too:
        </p>
        <pre class="prettyprint lang-cs">

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
          this.Get["/"] = o => "Hello OWIN!";
        }
    }
        </pre>	  
        <pre class="prettyprint lang-cs">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
                        
            app.UseSuperscribeRouter(define)
                .UseSuperscribeHandler(define);
        }
    }
        </pre>
        </div>
	</div>
  </div>
</div>