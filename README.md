Superscribe
===========

Traditional Web API routing relies heavily on pattern matching an entire url which can be slow with complex requirements. Routes are cumbersome to define and don't provide much scope for reusabilty or programatic generation.

Attribute routing goes someway to addressing some of these issues, but can still be hard to manage as your route definitions are somewhat verbose, and split across multiple files. It is also still limited to the action\controller selection mechanisms of standard routing, which although extensible are not particularly powerful out of the box.

Superscribe addresses these issues and more by offering:

* Hierarchical routes - stored as a tree to reduce processing
* Composite definitions - build nested routes using others as a base
* Programatical definition - store parts of routes as variables and build your routes using custom algorithms
* Improved action matching - match actions based on parameter names as well as types
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


We use the ʃ.Route() method to register routes and partial routes with the framework. The ~ before the id parameter defines this segment as optional.

####Choosing controllers

We can specify that a route segment represents a controller in three ways:

    // Match any valid identifier and set ControllerName to segment text
    ʃ.Route(o => o / "api" / ʃ.Controller);
    // Match a specific string and set ControllerName to it
    ʃ.Route(o => o / "api" / "blog".Controller());
    // Match a specific string and set ControllerName to the specified text
    ʃ.Route(o => o / "api" / "blog".Controller("blogposts"));

When route parsing is complete, the ControllerSelector will use whatever value is set for ControllerName. Routes can include multiple controller segments and the value will be overwritten. This is useful for defining nested routes, for example:

   ʃ.Route(o => o / "blog" / "posts".Controller("blogposts") / ~"id".int() / ~"tags".controller("blogposttags"));
   
Due to the optional nature of the later segments, this will match the following routes:

    http://localhost/blog/posts            -> blogposts controller
    http://localhost/blog/posts/{id}       -> blogposts controller
    http://localhost/blog/posts/{id}/tags  -> blogposttags controller

####Choosing actions

We can also have our route specify action names explicitly in an MVC style if needed:

    // Match any valid identifier and set ActionName to segment text
    ʃ.Route(o => o / "api" / ʃ.Controller);
    // Match a specific string and set ControllerName to it
    ʃ.Route(o => o / "api" / "blog".Controller());
    // Match a specific string and set ControllerName to the specified text
    ʃ.Route(o => o / "api" / "blog".Controller("blogposts"));

Just like controllers, the ActionSelector will use the final value set by any action segments.

####Parameters and Querystring values
    
####Alternatives

###Defining hierarchical routes

In Superscribe, we can store parts of routes in variables, and then use these to compose other routes. Consider the following example:

    var site = ʃ.Route(o => o / "sites" / "siteId".Int());
    
    var blog = ʃ.Route(o => site / "blog"); 
    var portfolio = ʃ.Route(o => site / "portfolio");
    
    ʃ.Route(o => portfolio / "categories".Controller("portfoliocategories"));
    ʃ.Route(o => portfolio / "tags".Controller("portfoliotags"));
    ʃ.Route(o => blog / "categories".Controller("blogcategories"));
    ʃ.Route(o => blog / "tags".Controller("blogtags"));

This provides the following routes:

    http://api/sites/{id}/blog/categories
    http://api/sites/{id}/blog/tags
    http://api/sites/{id}/portfolio/categories
    http://api/sites/{id}/portfolio/tags

###Programatical definition

In the previous example, we re-used the site part of the route as a base for blog and portfolio, and then extend both of these again. We didn't gain much through re-use, but the definitions are much more readable than the existing Web API equivilent.

Because routes are defined programatically, we can take this one step further and re-write the example as follows for the same results. This opens up a world of possibilities as we are free to define routes via whatever algorithms we choose, for example by traversing an API hierarchy... something which was technically possible yet onerous with traditional routing.

    var site = ʃ.Route(o => o / "sites" / "siteId".Int());
    
    foreach (var routeName in new [] { "blog", "portfolio" }) {
        var route = ʃ.Route(o => site / routeName);
        ʃ.Route(o => route / "categories".Controller(routeName + "categories"));
        ʃ.Route(o => route / "tags".Controller(routeName + "tags"));
    }
    
###State machine based routing

Superscribe introduces a brand new concept... 'state machine based routing'. As the name suggests, routes are parsed by means of a finitie state machine. Defined routes are stored as a tree structure, with each node defining a potential segment of the route. The children of each node define the possible transitions in the state machine.

Urls are broken down into *route segments*, denoted by each / in the path. We then attempt to find a match for each segment in turn, according to the valid transitions as determined by the state machine. If we find a match and progress to the next state, an action can be taken based on the value of that route segment.

For example:

    ʃ.Route(o => o / "api" / ʃ.Controller / ~"id".Int());

The definition produces a very simple state machine, with only one valid transition at each state apart from the optional nature of the id parameter:
![alt text](https://raw.github.com/Roysvork/Superscribe/master/Documentation/Images/basicstatemachine.png "Basic state machine")

Lets say we access the following url in our API: *http://localhost/api/values/2*
  
The url is broken down into it's three segments "api", "values", 2. Our first transition is valid, as the initial segment matches the text "api". As this is just a route literal, there is no action to be taken and we simply move on to the next state and the next segment. 

The next part of the definition, ʃ.Controller has an implicit regex condition specifying a valid controller identifier, which 'values' satisifes. In addition, ʃ.Controller also contains an implicit action - to set the ControllerName property to the value of the segment. This is then passed to the ControllerSelector once route processing is complete. 

The final state is the optional id parameter. In our case, we do have an id parameter, so when we enter the penultimate state this value will be added to the Parameters dictionary and then later used in Action Selection. If the parameter was omitted, we would just progress straight to the sucess state. For all non-optional states, a missing or invalid segment will cause us to jump to the Error state instead.

###Syntax in-depth

At it's core, superscribe route definition is built on a fluent API, which is encapsulated by several layers of abstraction, using extention methods, operator overloading and implicit casts to make syntax as concise as possible. The following are all equivilent:

    Superscribe.Base.Transitions.Enqueue(
                new ConstantState("api")
                .Slash(new ControllerState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+") })
                .Slash(new ParamState<int>("id")).Optional());

    ʃ.Route(o => o.Slash(ʃ.Constant("api")).Slash(ʃ.Controller).Slash(ʃ.Int("id")).Optional());

    ʃ.Route(o => o / "api" / ʃ.Controller / ~"id".Int());
    
In these last two, we are using ʃ.Route to attach route segment definitions to the list of base transitions. Notice how we are utilising the 'o' parameter of the lambda expression... this causes the chain of segments we define to be attached to the base object.

We can also use ʃ.Route **without** referencing 'o' to establish derived routes. The following blocks are equivilent:

    var sites = ʃ.Constant("api")
        .Slash(ʃ.Constant("sites"))
        .Slash(ʃ.Int("id"));
    sites.Slash(ʃ.Constant("blog"))
        .Slash(ʃ.Controller)
    Superscribe.Base.Transitions.Enqueue(sites);
    
    var sites = ʃ.Route(o => o / "api" / "sites" / "id".Int());
    ʃ.Route(o => sites / "blog" / "posts".Controller());
    
    var sites = ʃ.Route(o => o / "api" / "sites" / "id".Int());
    var blogposts = sites / "blog" / "posts".Controller());
       
It is a little harder to see how these are all equivilent, particularly as the third features an assignment rather than a call to ʃ.Route, so how does the partial definition end up attached to the rest of the chain in the framework?

The reason this works is that the / operators are equivilent to Slash() method chains, but a side effect of this syntax however is that we must assign the result of the / expression to a variable otherwise it is not valid c#. So the assignment in this case is purely there to appease the compiler, and the same effect can be achieved by using a lambda as in the second block.

