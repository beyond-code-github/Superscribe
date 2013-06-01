Superscribe
===========

Traditional Web API routing relies heavily on pattern matching an entire url which can be slow with complex requirements. Routes are cumbersome to define and don't provide much scope for reusabilty or programatic generation.

Attribute routing goes someway to addressing some of these issues, but can still be hard to manage as your route definitions are somewhat verbose, and split across multiple files. It is also still limited to the action\controller selection mechanisms of standard routing, which although extensible are not particularly powerful out of the box.

Superscribe addresses these issues and more by offering:

* Hierarchical routes - stored as a tree to reduce processing
* Composite definitions - build nested routes using others as a base
* Programatical definition - store parts of routes as variables and build your routes using custom algorithms
* Advanced action matching - match actions based on differing parameter names
* Concise syntax - specify nice clean routes without the weight of config.Routes.MapHttpRoute(..)
* State-Machine based routing - an entirely new concept which gives absolute control in how routes are processed

###Basic usage:

A basic superscribe route definition looks like this:

    ʃ.Route(o => o / "api" / ʃ.Controller / ~"id".Int());
    
This is equivilent to the basic route we get when creating a new API project:

    config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
    );

The ~ before the id parameter defines this segment as optional.

###Defining hierarchical routes

In Superscribe, we can store parts of routes in variables, and then use these to compose other routes. Consider the following contrived example:

    var site = ʃ.Route(o => o / "sites" / "siteId".Int());
    
    var blog = ʃ.Route(o => site / "blog"); 
    var portfolio = ʃ.Route(o => site / "portfolio");
    
    ʃ.Route(o => portfolio / "categories".Controller("portfoliocategories"));
    ʃ.Route(o => portfolio / "tags".Controller("portfoliotags"));
    ʃ.Route(o => blog / "categories".ʃ(i => i.ControllerName = "blogcategories"));
    ʃ.Route(o => blog / "tags".ʃ(i => i.ControllerName = "blogtags"));

This provides the following routes:

    http://api/sites/blog/categories
    http://api/sites/blog/tags
    http://api/sites/portfolio/categories
    http://api/sites/portfolio/tags

###Programtical definition

In the previous example, we re-used the site part of the route as a base for blog and portfolio, and then extendd both of these again. We didn't gain much through re-use, but the definitions are much more readable than the existing Web API equivilent.

Because routes are defined programatically, we can take this one step further and re-write the example as follows for the same results. This opens up a world of possibilities as we are free to define routes via whatever algorithms we choose, for example by traversing an API hierarchy.

    var site = ʃ.Route(o => o / "sites" / "siteId".Int());
    
    var blog =  
    var portfolio = ʃ.Route(o => site / "portfolio");
    
    foreach (var routeName in new [] { "blog", "portfolio" }) {
        var route = ʃ.Route(o => site / routeName);
        ʃ.Route(o => route / "categories".ʃ(i => i.ControllerName = routeName + "categories"));
        ʃ.Route(o => route / "tags".ʃ(i => i.ControllerName = routeName + "tags"));
    }
    
###Concise syntax
    
###State machine based routing

Superscribe introduces a brand new concept... 'state machine based routing'. As the name suggests, routes are parsed by means of a finitie state machine. Defined routes are stored as a tree structure, with each node representing a potential *route segment*. The children of each node become the possible 'transitions' in the state machine.

Urls are broken down into *route segments*, denoted by each / in the path. We then attempt to find a match for each segment in turn, according to the valid transitions as determined by the state machine. For example:

    // Url: http://localhost/api/values/2
    
    ʃ.Route(o => o / "api" / ʃ.Controller / ~"id".Int());
    
This url is broken down into it's three segments "api", "values", 2. The definition produces a very simple state machine, with only one transition at each state:






When defining routes, superscribe lets you specify an action delegate for each segment in the tree. Just as in a traditional finite state machine, we can control which state we transition to next. This allows for extremely complex routing strategies, even based on aggregated values from multiple route segments if needed.

