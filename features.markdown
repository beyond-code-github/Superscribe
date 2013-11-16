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
    	<li class="active"> <a href="#tab1" data-toggle="tab">Replacement Asp.Net Web API routing<small>Replace existing routes with syntax thats much more concise and easy to manage</small><i class="icon-angle-right"></i></a> </li>
    	<li> <a href="#tab2" data-toggle="tab">Easy unit testing<small>Invoke the superscribe routing engine in isolation from the rest of your app</small><i class="icon-angle-right"></i></a> </li>     	
      	<li> <a href="#tab3" data-toggle="tab">Bring Nancy style modules to Web API<small>All the benefits of the code-centric approach combined with graph based routing</small><i class="icon-angle-right"></i></a> </li>   
      	<li> <a href="#tab4" data-toggle="tab">Serve your data direct from OWIN<small>Create ultra-lightweight services for maximum performance</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
	<div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="tab1">
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
		name: name + "ProjectMediaRoute",
		routeTemplate: "sites/{siteId}/" + name + "/projects/{projectId}/media/{id}",
		defaults: new { controller = name + "projectmedia", id = RouteParameter.Optional }
	);
	 
	config.Routes.MapHttpRoute(
		name: name + "TagsRoute",
		routeTemplate: "sites/{siteId}/" + name + "/tags",
		defaults: new { controller = name + "tags" }
	);
	 
	config.Routes.MapHttpRoute(
		name: name + "ProjectsRoute",
		routeTemplate: "sites/{siteId}/" + name + "/projects/{id}",
		defaults: new { controller = name + "projects", id = RouteParameter.Optional }
	);
	 
	config.Routes.MapHttpRoute(
		name: name + "CategoriesRoute",
		routeTemplate: "sites/{siteId}/" + name + "/categories/{id}",
		defaults: new { controller = name + "categories", id = RouteParameter.Optional }
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

    ʃ.Route(ʅ => ʅ / "sites" / (ʃInt)"siteId" / (
	    ʅ / "blog" / (
	        ʅ / "tags".Controller("blogtags")
	      | ʅ / "posts".Controller("blogposts") / (
	            ʅ / -(ʃInt)"postId" / -"media".Controller("blogpostmedia") / -(ʃInt)"id"
	          | ʅ / "archives".Controller("blogpostarchives") / -(ʃInt)"year" / (ʃInt)"month"))
	  | ʅ / "portfolio" / (
	        ʅ / "projects".Controller("portfolioprojects") / -(ʃInt)"projectId" / -"media".Controller("portfolioprojectmedia") / -(ʃInt)"id"
	      | ʅ / "tags".Controller("portfoliotags")
	      | ʅ / "categories".Controller("portfoliocategories") / -(ʃInt)"id")));
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="tab2">
        <h3>Test your route handlers in isolation</h3>
        <p>The Superscribe engine is built in a way which means it's easy to parse urls without invoking the rest of your application using the Route Walker. Provided you have implemented your handlers in a decoupled fashion, you can add route definitions, perform tests on your matching, then reset back to scratch for the next iteration with minimal fuss</p>
        <pre class="prettyprint lang-cs">

    [TestClass]
    public class SuperscribeUnitTests
    {
        private RouteWalker&lt;RouteData&gt; routeWalker;
            
        [TestInitialize]
        public void Setup()
        {
            routeWalker = new RouteWalker&lt;RouteData&gt;(ʃ.Base);

            ʃ.Route(ʅ => 
                ʅ / "Hello" / (
                    ʅ / "World"         * (o => "Hello World!")
                  | ʅ / (ʃString)"Name" * (o => "Hello " + o.Parameters.Name)));
        }

        [TestCleanup]
        public void Cleanup()
        {
            ʃ.Reset();
        }
        
        [TestMethod]
        public void Test_Hello_World_Get()
        {
            var routeData = new RouteData();
            routeWalker.WalkRoute("/Hello/World", "GET", routeData);

            Assert.AreEqual("Hello World!", routeData.Response);
        }

        [TestMethod]
        public void Test_Hello_Name_Get()
        {
            var routeData = new RouteData();
            routeWalker.WalkRoute("/Hello/Kathryn", "GET", routeData);

            Assert.AreEqual("Kathryn", routeData.Parameters.Name);
            Assert.AreEqual("Hello Kathryn", routeData.Response);
        }
    }
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="tab3">
        <h3>Module style route handlers in Asp.Net Web Api</h3>
        <p>Attribute routing in Asp.Net is pretty useful, but it still constrains you to the traditional construct of Controller\Action. With very little effort you can now break the mold and use Nancy\Sinatra style modules, complete with model binding and dependency injection. If you like to define your routes close to where they're handled then this is the solution for you, and of course you still get all the benefits of Graph Based Routing.</p>
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
            this.Get["/"] = ʅ => "Hello World!";

        	this.Get["Hello" / (ʃString)"Name"] = 
          		ʅ => string.Format("Hello {0}", ʅ.Parameters.Name);
        }
    }
		</pre>
	  </div>	
      <div class="tab-pane col-sm-12 col-md-12" id="tab4">
        <h3>Superscribe and OWIN, pipeline to pipeline</h3>
        <p></p>
        <pre class="prettyprint lang-cs">
		</pre>
	  </div>
	</div>
  </div>
</div>