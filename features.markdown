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
      	<h2>Simplify your Asp.Net Web API Routes</h3>
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
        <h2>Test your route handlers in isolation</h3>
        <p>The Superscribe engine is built in a way which means it's easy to parse urls without invoking the rest of your application using the Route Walker. Provided you have implemented your handlers in a decoupled fashion, you can add route definitions, perform tests on your matching, then reset back to scratch for the next iteration with minimal fuss</p>
        <pre class="prettyprint lang-cs">
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="tab3">
        <h3 class="title visible-phone"></h3>
        <pre class="prettyprint lang-cs">
		</pre>
	  </div>	
      <div class="tab-pane col-sm-12 col-md-12" id="tab4">
        <h3 class="title visible-phone"></h3>
        <pre class="prettyprint lang-cs">
		</pre>
	  </div>
	</div>
  </div>
</div>