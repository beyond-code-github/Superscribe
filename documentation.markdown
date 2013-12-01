---
layout: default
title:  Features
---

<div class="block">
<h2 class="title-divider"><span>Documentation</span>
<small>Information and resources to help you get started with Superscribe</small>
</h2>
  <div class="tabbable tabs-left vertical-tabs bold-tabs row">
    <ul class="nav nav-tabs nav-stacked col-md-4">
      <li class="active"> <a href="#fluentapi" data-toggle="tab">Fluent API<small>Define hierarchical and strongly typed routes with Superscribe</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#dsl" data-toggle="tab">DSL<small>Shorthand syntax for defining routes that are concise and easy to maintain</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#modules" data-toggle="tab">Modules<small>Inspired by NancyFX, a great way to keep your definitions and your handlers together</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#webapi" data-toggle="tab">Superscribe.WebAPI<small>Specific syntax to help you match routes and then invoke controllers and actions</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#owin" data-toggle="tab">Superscribe.OWIN<small>Branch your pipeline during routing and hand control to any OWIN middleware</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
	<div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="fluentapi">
      	<h3 class="visible-phone">Defining routes using superscribe's fluent interface</h3>
      	<p>This section starts with a disclaimer. In practice you won't want to write routes using the Fluent API, as they won't look very nice and will be quite verbose; instead you'll be using the DSL wherever possible. However, to work with Superscribe effectively and to lessen any learning curves, it is useful to understand what the DSL is doing behind the scenes. As a result, this section should be considered required reading before continuing to the later topics</p>
        <h3 class="title visible-phone">The ʃ Class & SuperscribeNode</h3>
        <p>Superscribe's features are all accessed via the ʃ class, a member of the core library. Graph based routing definitions are constructed using strongly typed nodes and then stored as a graph. The <code class="prettyprint lang-cs">ʃ.Base</code> property is the root node of the graph... it matches the root '/' url, and is the parent for any susequent definitions.</p>
        <p>ʃ.Base is of type SuperscribeNode... this is the base class for all route segment definitions and contains Superscribe's bread and butter functionality. If we want to respond to a request to '/', we need to provide ʃ.Base with a Final Function:</p>
        <pre class="prettyprint lang-cs">

    ʃ.Base.FinalFunctions.Add(new FinalFunction("GET", _ => @"
        Welcome to Superscribe 
        &lta href='/Hello/World'&gtSay hello...&lt/a&gt
    "));

    // "/" -> "Welcome to Superscribe..."
		</pre>
		<p>This will respond the the '/' uri with a message and a link. To respond to the link uri, we simply extend our definition:</p>
		<pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

    ʃ.Base.Zip(helloRoute.Base());

    // "/Hello/World" -> "Hello World"
		</pre>
		<p>The above sample exposes three SuperscribeNode functions... Slash, Zip and Base, as well as a new subclass of SuperscribeNode, the ConstantNode. These work as follows:
			<ul>
				<li>The <strong>ConstantNode</strong> as it's name suggests, will only match a route segment that is identical to the value passed to it in the constructor</li>
				<li>The <strong>Slash</strong> function creates an edge between two nodes and then returns the child node.</li>
				<li>The <strong>Base</strong> function returns the topmost parent of a node. (remember, .Slash returns the child "World" node but we want to create an edge in ʃ.Base that points to the "Hello" node.</li>
				<li>Finally, the <strong>Zip</strong> function performs a similar function to .Slash but with one important difference... it will combine any identical notes (See the next code block for an example).</li>
			</ul>
		</p>
		<h3 class="title visible-phone">Optional Nodes</h3>
		<p>Combined, the samples above create an app that will respond to '/' and '/Hello/World', but not 'Hello':</p>
		<pre class="prettyprint lang-cs">

    // "/Hello" -> 404 - Route was incomplete
		</pre>
		<p>This is because the "Hello" node only has one edge, and it is not optional. To change this, we call the fluent method Optional() on the "World" node:
		<pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World")).Optional();
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

    ʃ.Base.Zip(helloRoute.Base());

    // "/Hello" -> "Welcome to Superscribe..."
		</pre>
		<p>This might not be the behavior you'd expect, but if we look at the Graph Based Routing spec all becomes clear: </p>
		<div class="well well-mini pull-center">
          <em>"Once route parsing is complete, the Final function of the last node is executed. If the last node does not provide one, the engine must execute the final function of the last travelled node that did."</em>
        </div>
		<p>This behavior isn't entirely useless... we can use an Action Function to set some values which then become available to our final function and influence it's behavior as we'll see in the next section. A better way of making the Superscribe engine treat 'Hello' as optional in this situation is to assign it a Final Function of it's own:</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

	var justHelloRoute = new ConstantNode("Hello");
	justHelloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello... maybe"));

    ʃ.Base.Zip(helloRoute.Base());
    ʃ.Base.Zip(justHelloRoute.Base());

    // "/Hello" -> "Hello... maybe"
    // "/Hello/World" -> "Hello World"
		</pre>
        <p>Note the usage of Zip() in this sample to combine the two 'Hello' nodes into one. Another except from the Graph Based Routing spec reveals how this change creates the optional behavior.</p>
        <div class="well well-mini pull-center">
          <em>"If the next match is null, the incomplete match flag will be set unless a) there are no edges on the current node or b) all edges on the current node are optional, or c) The current node has a final function defined for the current method"</em>
        </div>
        <h4>Parameters and the RouteData object</h4>
        <p>We've touched on Action Functions briefly above, but their usefulness becomes clear when we need to capture parameters. Superscribe provides a generic node class that provides a ready made parameter capture action. With this subclass of SuperscribeNode we can capture any data type we might need:</p>
        <ul>
        	<li><strong>Integer</strong> ParamNode&lt;int&gt;</li>
        	<li><strong>Long</strong> ParamNode&lt;long&gt;</li>
        	<li><strong>String</strong> ParamNode&lt;string&gt;</li>
        	<li><strong>Boolean</strong> ParamNode&lt;bool&gt;</li>
        	<li><strong>Guid</strong> ParamNode&lt;Guid&gt;</li>
        </ul>
        <p>These nodes also come with an activation function that uses TryParse to determine whether or not the current route segment is a match for our parameter. This is not something that is easy to do using traditional routing constraints, but becomes easy with Graph Based Routing.</p>
        <p>An example of parameter capture with Superscribe is as follows:</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ParamNode<string>("Name"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello " + _.Parameters.Name));

    ʃ.Base.Zip(helloRoute.Base());

    // "/Hello/Kathryn" -> "Hello Kathryn"
		</pre>
		<p>The ParamNode&lt;T&gt; takes a string as a parameter... the captured value is then added to a dynamic dictionary using this string as the key. The parameters dictionary is available to all our final function via the RouteData object, represented as _ in our example. Any querystring parameters are also added to this dictionary.</p>
		<p>The RouteData object is more than just a container for paramters however. It also provides us with dependency injection and model binding functionality, as well as direct access to the response and request context. Finally, it also acts as a place we can store any abritary data that we need to use later on in the routing pipeline.</p>
		<h4>Custom Nodes</h4>
		<p>In addition to leveraging the various subclasses of SuperscribeNode, we can also derive our own custom nodes, giving us full control over it's Action and Activation Functions. Consider the following custom node that matches and captures only even parameters:</p>
		<pre class="prettyprint lang-cs">

	public class EvenNumberNode : SuperscribeNode
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
		</pre>
		<p>The EvenNumberNode duplicates the functionality of ParamNode&lt;int&gt; but with an added match condition. This example is a little contrivied; it's not a particularly useful Activation Function and we could easily derive from ParamNode and re-use it's Action Function instead of adding our own. Having said that, it's a good example of the flexibility of Graph Based Routing.</p>
		<p>Using the node in an app is just the same as with the other nodes we've seen:</p>
		<pre class="prettyprint lang-cs">

    var productIdRoute = new ConstantNode("Products").Slash(new EvenNumberNode("Id"));
    productIdRoute.FinalFunctions.Add(
    	new FinalFunction("GET", _ => "Product info for id: " + _.Parameters.Id));

    ʃ.Base.Zip(productIdRoute.Base());

    // "/Products/12" -> "Product info for id: 12"
    // "/Products/13" -> "404 - Route was incomplete"
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="dsl">
        <h3>Defining routes with the DSL</h3>
        <p>As mentioned in the previous section, defining routing using the fluent API is not ideal for a number of reasons. Built on top of the fluent API is a simple domain specific language... valid C# code made using a combination of lambdas, casts and operator overloads. The syntax provided by the DSL is designed to be terse and minimal, so you can get on with the important business of designing your routes.</p>
        <div class="well well-mini pull-center">
          <em>If you haven't already read the Fluent API section, it is reccomended that you do so before continuing so you are familiar with some of the terms and concepts used.</em>
        </div>
        <h3 class="title visible-phone">Shorthand operators and ʃ.Route</h3>
        <p>When dealing with the fluent API, we defined routes by manually instantiating subclasses of SuperscribeNode, created edges with Slash() and then attached them to the base node using Base() and ʃ.Zip():</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

    ʃ.Base.Zip(helloRoute.Base());

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>The DSL provides a shorthand way of doing this by using the ʃ.Route method:</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

    ʃ.Route(ʅ => ʅ / helloRoute.Base());

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>This isn't much of an improvement but we can still do more:<p>
        <ul> 
            <li>Instead of calling .Slash we can use the shorthand operator '/'</li>
            <li>To apply a final function to the "World node we can use the shorthand operator '*':</li>
            <li>Finally instead of calling Base(), we can use the '+' prefix:</li>
        </ul>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello") / new ConstantNode("World") * (o => "Hello World");
    ʃ.Route(ʅ => ʅ / +helloRoute);

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>We've got one more trick up our sleeve... an implicit cast from string to SuperscribeNode provides a way to simplify the instantiation of our constant nodes:</p>
        <pre class="prettyprint lang-cs">

    ʃ.Route(ʅ => ʅ / "Hello" / "World" * (o => "Hello World"));

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>Much more concise. Note that in this case we don't need to use the '+' prefix or call Base() as the route is defined as one unbroken chain of operators.</p>
        <h3 class="title visible-phone">Route Glue</h3>
        <p>When calling ʃ.Route, the ʅ parameter is known as the Route Glue. It's so called because we can use it to attach nodes to the base instead of doing this directly. In this last example however we have a problem... when using any of the Superscribe shorthand operators, at least one of the operands must be a SuperscribeNode. If we try to assign part of our route to a variable as we did with the fluent API we run into trouble as both operands are strings and the expression won't compile.</p>
        <p>We solve this by using the Route Glue in another way... this time to attach several notes together and form a partial route:</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = ʃ.ʅ / "Hello" / "World" * (o => "Hello World"));
    ʃ.Route(ʅ => ʅ / +helloRoute );

    // "/Hello/World" -> "Hello World"
        </pre>
        <div class="well well-mini pull-center">
          <em>When using Superscribe modules, there's an even more concise way to access the Route Glue. See the section on Modules for more information.</em>
        </div>
        <h3 class="title visible-phone">SuperscribeNode shorthands</h3>
        <p>Creating a ConstantNode from a string isn't the only way we can take advantage of casting to build routes. In the fluent API section we discovered how to capture parameters using ParamNode<T>:</p>
        <pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ParamNode<string>("Name"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello " + _.Parameters.Name));

    ʃ.Base.Zip(helloRoute.Base());

    // "/Hello/Kathryn" -> "Hello Kathryn"
        </pre>
        <p>Now here's the DSL version:</p>
        <pre class="prettyprint lang-cs">

    ʃ.Route(ʅ => ʅ / "Hello" / (ʃString)"Name" * (o => "Hello " + o.Parameters.Name));

    // "/Hello/Kathryn" -> "Hello Kathryn"
        </pre>
        <p>Quite simply, ʃString is a subclass of ParamNode<string> with an extra explicit cast operator... the sole purpose of which is to provide us with this much nicer syntax. There are others for each of the primary data types supported by superscribe:</p>
        <ul>
            <li><strong>Integer</strong> (ʃInt)</li>
            <li><strong>Long</strong> (ʃLong)</li>
            <li><strong>String</strong>(ʃString)</li>
            <li><strong>Boolean</strong> (Bool)</li>
        </ul>
        <p>And of course, because all Superscribe syntax is strongly typed we can create our own shorthands. Here's an example for Guid:</p>
        <pre class="prettyprint lang-cs">

    public class ʃGuid : ParamNode&lt;Guid&gt;
    {
        public ʃGuid(string name) : base(name) { }

        public static explicit operator ʃGuid(string name)
        {
            return new ʃGuid(name);
        }
    }
        </pre>
        <h3 class="title visible-phone">Providing options and making choices</h3>
        <p>Using the fluent API and the Zip() function we were able to compose our route graph by combining several complete routes. This is the preferred approach when routes are defined close to where they are handled, but for those who like to define their routes centrally it can seem like a lot of duplicated code. With the Superscribe DSL we have a new way of building our graph by specifying multiple edges at once:
        </p>
        <pre class="prettyprint lang-cs">

    ʃ.Route(ʅ => 
        ʅ / "Products"      * (o => / "List all products" ) / (
              ʅ / "Sale"            * (o => "List Products on sale")
            | ʅ / "BestSellers"     * (o => "List Bestsellers")
            | ʅ / (ʃInt)"Id"        * (o => "Product #" + o.Params.Id");
    ));

    // "/Products/" -> "List Products"
    // "/Products/Sale" -> "List Products on sale"
    // "/Products/BestSellers" -> "List Bestsellers"
    // "/Products/13" -> "Product #13"
        </pre>
        <p>
            Once again the RouteGlue helps us out in order to attach each of our edges the the parent node, in this case "Products". We've also kept code and noise to a minimum and ended up with some nice neat definitions.
        </p>
	  </div>
       <div class="tab-pane col-sm-12 col-md-12" id="modules">
        <h3 class="visible-phone">Handling routes with Modules</h3>
        <p>Asp.Net Web Api uses controllers and actions because it was originally derived from the MVC framework and although things have moved on since then, it still shares some of the same constructs. With Graph based routing we can break free of these restrictions and handle our routes using whatever classes we wish. A great example of this comes in the form of modules, inspired by the Ruby web framework Sinatra, and the .Net framework NancyFX</p>
        <h3 class="title visible-phone">Assembly Scanning</h3>
        <p>When the module configuration is activated on app startup, Superscribe scans your assemblies for classes that derive from <em>SuperscribeModule</em> and then instantiates them in turn. All route handlers for modules are defined in the constructor, so this process results in all of our route definitions being added to the graph. Modules can be enabled in Superscribe.WebApi with the following config:</p>
        <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.RegisterModules(config);
        }
    }
        </pre>
        <p>A basic module takes the following form:</p>
        <pre class="prettyprint lang-cs">

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = o => "Hello World!";
        }
    }
        </pre>
        <p>The primary difference when routing with modules compared to traditional Web API is that we don't get any of the action selection logic that maps requests to actions, so we have to be prescriptive about our http methods and every definition must be assigned a final function.</p>
        <p>In addition to Get[], SuperscribeModule also exposes properties for Put, Post, Patch and Delete methods. Each one takes an indexer within which we can use a subset of the Superscribe DSL syntax. Note from the previous example, to use these indexers to attach routes to ʃ.Base we simply pass "/".</p>
        <h3 class="title visible-phone">Building the Route Graph</h3>
        <p>One aspect of the DSL that is no longer appropriate when dealing with modules is the ability to specify multiple edges at once using the options syntax. Instead, we have to repeat the whole route for each handler:</p>
        <pre class="prettyprint lang-cs">

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get[ʅ / "Hello" / "World"] = o => "Hello World!";

            this.Get[ʅ / "Hello" / (ʃString)"Name"] = o => "Hello " + o.Parameters.Name;
        }
    }
        </pre>
        <p>Each call to this.Get, or any of the other method helpers ensures that the route graph is still constructed efficiently. Behind the scenes each route is 'Zipped' together (see the Fluent Api section) so that identical segments will be merged. In the above example, we end up with a single "Hello" node with two edges.</p>
        <p>Finally you'll notice we've also got access to the route glue ʅ as a property of SuperscribeModule which works in the usual way.</p>
        <div class="well well-mini pull-center">
          <em>It is still possible to compose routes from parts within modules using the route glue, but doing so can go against the ethos of modules which is to make it easy to see what route a particualr handler serves.</em>
        </div>
        <h3 class="title visible-phone">Model Binding and Dependencies</h3>
        <p>When we used the fluent api or DSL directly, we accessed parameters in our final function via the RouteData object and it's Parameters dictionary. We've still got that feature within our modules, but the RouteData object is now of type ModuleRouteData instead. This gives us more flexibility to do the things that controller/action selection would usually take care of</p>
        <p>One of the most useful of these is model binding... taking data from the request body or querystring and mapping it to a strongly typed model. We're still working on top of Web Api behind the scenes so we can achieve the same result quite easily using the ModuleRouteData .Bind() function:</p>
        <pre class="prettyprint lang-cs">

    public class ProductsModule : SuperscribeModule
    {
        public ProductsModule()
        {
            this.Post[ʅ / "Produts"] = o => 
            {
                var model = o.Bind&lt;Product&gt;();               
                // do something with product
            }
        }
    }
        </pre>
        <p>If we want to actually store our Product then we also need to access the database, and the best way to do that and keep things loosely coupled is to inject a dependency. For that we can use the .Require function which simply wraps up the standard Web Api DepedencyResolver:</p>
         <pre class="prettyprint lang-cs">

    public class ProductsModule : SuperscribeModule
    {
        public ProductsModule()
        {
            this.Post[ʅ / "Produts"] = o => 
            {
                var repository = o.Require&lt;IProductsRepository&gt;();
                var model = o.Bind&lt;Product&gt;();
                repository.create(model);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }
    }
        </pre>
        <p>Now we've got all the ingredients we need to build our app using modules... Web Api is still taking care of content negotiation for us and we can easily build in other aspects such as validation as needed. We can also take control over the response as we need to by returning HttpResponseMessage just as we would in a regular controller.</p>
      </div>
      <div class="tab-pane col-sm-12 col-md-12" id="webapi">
        <h3 class="visible-phone">Replacing Asp.Net Web Api routing with Superscribe</h3>
        <p>Ask most developers what could be improved about Asp.Net Web Api and they'll probably mention the routing. In recent months this has been improved considerably by the introduction of attribute routing in Web Api 2. But attribute routing isn't for everyone, some of us like to define all the routes for our application  one place.Superscribe.WebApi allows us to define routes using the Supersribe DSL along with a few Web Api specific nodes.</p>
        <h3 class="title visible-phone">Controller Selection</h3>
        <p>The following recreates the behavior of the default rouing you get when creating a new project:</p>
        <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(() => "api" / ʃ.Controller / -(ʃInt)"id");
        }
    }
        </pre>
        <p>
            Here we've used a Web Api specific node, ʃ.Controller. This node will match any alpha numeric segment, and then set a parameter called ControllerName accordingly. Once route parsing has completed succesfully, Superscribe will pass this value and any parameters across to Asp.Net and then the process of selecting an action continues as per normal (You can view a <a href="http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-and-action-selection">summary of this logic here</a>)
        </p>
        <p>Web Api is still doing the bulk of the work, Superscribe is just passing it the information it needs; as a result we don't usually need to specify final functions against our nodes. We still can if we want to, but we don't have as much flexibility with the Response as we do when we use Modules.</p>
        <p>Suppose our controller name doesn't match the format "&lt;segment&gt;Controller". In this case we can match a specific segment value and set the controller name ourselves:</p>
        <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(() => "api" / (
                  "Blogs".Controller() / -(ʃInt)"id" / "Tags".Controller("BlogTags"));
        }
    }

    // /api/Blogs/
    // /api/Blogs/123/
    // /api/Blogs/123/Tags
        </pre>
        <p>By specifying "Blogs" to our first controller node, we change it's behavior from a regex match to a literal comparison. Superscribe will still use the segment value to set the ControllerName, but the matching will be much faster. If the engine then goes on to match the "Tags" node, then this will overwrite the ControllerName with "BlogTags" instead.</p>
        <h3 class="title visible-phone">Action Selection</h3>
        <p>ʃ.Controller has a partner in crime - ʃ.Action. This behaves in the same way as it's counterpart but instead it sets a parameter called ActionName. This allows us to be very specific about which action should handle our request in a similar way to you would in MVC.</p>
        <p>Say we have a controller called products that only handles read only resources for a few categories. There aren't many of them and they're all bespoke, so we just want to hard code an action for each:</p>
        <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(() => "api" / "Products".Controller() / ʃ.Action() / -(ʃInt)"id");
        }
    }
      </pre>
      <p>This is a nice stopgap to get us up and running in this situation. Note we can also match literals and override the ActionName just as we did with ControllerName:</p>
      <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(ʅ => "api" / "Products".Controller() / (
                  ʅ / -"BestSellers".Action("GetBestSellers") 
                | ʅ / -"TopTen".Action("GetTopTen"));
        }
    }

    // /api/Products
    // /api/Products/BestSellers
    // /api/Products/TopTen
      </pre>
      <div class="well well-mini pull-center">
          <em>Most of the time you shouldn't need to set the ActionName directly. However there are situations where Web Api is not able to figure out what action should handle our request and we use this approach to be more explicit and give it a helping hand.</em>
      </div>
      <h3 class="title visible-phone">Http Methods</h3>
      <p>This is the area that provides most of the headache when using traditional Web Api routing. Although usually Web Api will select actions based on methods according to convention or attributes, generally things fall down when trying to cater for multiple actions that handle the same verb within a single controller. By making choices based on http methods in our routing pipeline, we can achieve a finer level of control and avoid these issues:</p>
      <pre class="prettyprint lang-cs">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(ʅ => "api" / "Blog".Controller() / (
                    | ʅ["GET"] / (
                          ʅ / "Posts".Action("GetBlogPosts") / -(ʃInt)"id" 
                        | ʅ / "Tags".Action("GetBlogTags") / -(ʃInt)"id")
                    | ʅ["POST"] / (
                          ʅ / "Posts".Action("PostBlogPost") / (ʃInt)"id" 
                        | ʅ / "Tags".Action("PostBlogTag") / (ʃInt)"id")));
        }
    }

    // GET ->  /api/Blog/Posts
    // GET ->  /api/Blog/Posts/12
    // GET ->  /api/Blog/Tags
    // GET ->  /api/Blog/Tags/34
    // POST -> /api/Blog/Posts/56
    // POST -> /api/Blog/Tags/78
        </pre>
      </div>
      <div class="tab-pane col-sm-12 col-md-12" id="owin">
        <h3 class="visible-phone">OWIN Middleware Routing and Request Handling</h3>
        <p>To build a basic functional API, we need several things:</p>
            <ul>
                <li><strong>Hosting</strong></li>
                <li><strong>Routing</strong></li>
                <li><strong>Handlers</strong></li>
                <li><strong>Content Negotation</strong></li>
            </ul>
            <p>By using Owin for hosting, and Superscribe for routing we can get halfway there. But what about the rest? Superscribe.Owin provides the answer  via it's two middleware components, <em>SuperscribeRouter</em> and <em>SuperscribeHandler</em></p>
        <h3 class="visible-phone title">OwinRouter</h3>
        <p>
            OwinRouter is a middleware component that allows the Owin pipeline to invoke the Superscribe engine, and hence hook into all the advantages and syntax of Superscribe (as well as a few extra ones - see below). In it's most basic form, it allows us to direct control to a particular middleware component that will respond to our request:
        </p>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpconfig = new HttpConfiguration();
            httpconfig.Routes.MapHttpRoute(
                "DefaultApi", "api/webapi/", new { controller = "Hello" });

            // Tell Owin to use the SuperscribeRouter middleware
            app.UseSuperscribeRouter(new SuperscribeOwinConfig());

            // Set up a route that will respond via either Web Api or Nancy
            ʃ.Route(ʅ => ʅ / "api" / (
                  ʅ / "webapi" * Pipeline.Action(o => o.UseWebApi(httpconfig))
                | ʅ / "nancy" * Pipeline.Action(o => o.UseNancy())));
        }
    }
        </pre>
        <h3 class="visible-phone title">OwinHandler</h3>
        <p>
            We don't have to ask WebApi or Nancy to respond to our requests of course, and that's where OwinHandler comes in. This middleware component goes at the end of your pipeline and will take the RouteData object, apply content negotiation and then write the appropriate content to the Http Response stream.
        </p>
        <p>The following shows how we can respond to a basic "Hello World" request using both SuperscribeRouter and SuperscribeHandler</p>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add("text/html", new MediaTypeHandler { 
                Write = (env, o) => env.WriteResponse(o.ToString()) });

            app.UseSuperscribeRouter(config)
                .UseSuperscribeHandler(config);

            ʃ.Route(ʅ => ʅ / "Hello" / "World" * (o => "Hello World"));
        }
    }
        </pre>
        <p>We use SuperscribeOwinConfig to wire up any content types that we expect to have to handle. In this example we are dealing just with text/html in a <strong>write-only</strong> capacity.</p>
        <h3 class="visible-phone title">Modules</h3>
        <p>OwinHandler will also scan your assembly for any classes that inherit from <em>SuperscribeOwinModule</em>, and invoke their constructor in the same way that Superscribe.WebApi does when in module mode. Owin modules work in exactly the same way as regular modules (and as such all the regular module documentation applies too).<p>
        <p>Here's a more complicated scenario involving a module, both GET and POST handlers and some json model binding:</p>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add("text/html", new MediaTypeHandler { 
                Write = (env, o) => env.WriteResponse(o.ToString()) });
            config.MediaTypeHandlers.Add("application/json", new MediaTypeHandler {
                Write = (env, o) => env.WriteResponse(JsonConvert.SerializeObject(o)),
                Read = (env, type) =>
                {
                    object obj;
                    using (var reader = new StreamReader(env.GetRequestBody()))
                    {
                        obj = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
                    };

                    return obj;
                }
            });

            app.UseSuperscribeRouter(config)
                .UseSuperscribeHandler(config);
        }
    }
        </pre>
        <pre class="prettyprint">

    public ProductsModule()
    {
        this.Get["Products"] = o => products;

        this.Get["Products" / (ʃInt)"Id"] = o => 
            products.FirstOrDefault(p => o.Parameters.Id == p.Id);

        this.Get["Products" / (ʃString)"Category"] = o => 
            products.Where(p => o.Parameters.Category == p.Category);

        this.Post["Products"] = o =>
            {
                var product = o.Bind<Product>();
                return new { Message = string.Format(
                    "Received product id: {0}, description: {1}", 
                    product.Id, product.Description) };
            };
    }
        </pre>
        <h3 class="visible-phone title">Pipelining and Middleware Routing</h3>
        <p>Hopefully by now you've read about Action Functions and Final Functions and understand their purpose. With great power comes great responsibility... it's possible to some very crazy (bad) things with Superscribe if you're not disciplined. Sometimes if you have very complex sequence of Actions to be taken, it's best to use Action Functions to add these to a collection and execute them *after* routing is complete.</p>
        <p>
            In Superscribe, this process of deferring the execution of Action and Final function behavior is known as Pipelining. If you are familiar with OWIN and think this word sounds familiar then that's for a very good reason. When using Superscribe with OWIN, there is no real difference between your Routing pipeline and your OWIN pipeline.
        </p>
        <p>As an example, consider the following Owin middleware that wraps any response with a given tag:</p>
        <pre class="prettyprint">

    public class PadResponse
    {
        private readonly string tag;

        private readonly Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next;

        public PadResponse(Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next, string tag)
        {
            this.tag = tag;
            this.next = next;
        }

        public async Task Invoke(IDictionary&lt;string, object&gt; environment)
        {
            await environment.WriteResponse("&lt;" + this.tag + "&gt;");
            await this.next(environment);
            await environment.WriteResponse("&lt;" + this.tag + "&gt;");
        }
    }
        </pre>
        <p>
            We can build ourselves a Superscribe.Owin app without pipelining by putting this middleware in the usual OWIN pipeline which will give the response: <h1>Hello World</h1>
        </p>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(o.ToString()) });

            app.UseSuperscribeRouter(config)
                .Use(typeof(PadResponse), "h1")
                .UseSuperscribeHandler(config);

            ʃ.Route(ʅ => ʅ / "Hello" / "World" * (o => "Hello World"));
        }
    }
        </pre>
        <p>
            Now here's the same setup using Pipelining that yeilds exactly the same result:
        </p>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(o.ToString()) });

            app.UseSuperscribeRouter(config)
                .UseSuperscribeHandler(config);

            ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action&lt;PadResponse&gt;("h1") 
                                / "World" * (o => "Hello World"));
        }
    }
        </pre>
        <p>
            The difference with this last example is that we have now linked our Owin pipeline to our Routing pipeline. The pad resonse middleware will only be executed when we hit a route that starts with "Hello". We can see how this works by throwing another route into the mix:
        </p>
        <pre class="prettyprint">
        
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(o.ToString()) });
           
            // Advanced superscribe pipelining example

            app.UseSuperscribeRouter(config)
              .UseSuperscribeHandler(config);

            ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action&lt;PadResponse&gt;("H1") 
                                / "World" * (o => "Hello World"));

            ʃ.Route(ʅ => ʅ / "Mad" * Pipeline.Action&lt;PadResponse&gt;("marquee") 
                                / "World" * (o => "Hello World"));
        }
    }
        </pre>
        <p>Now we get a completely different behavior when we hit the Mad/World route. It's easy to extend this to more useful scenarios, such as providing authentication for certain areas of your API, or to enable debugging temporarily</p>
        <h3 class="visible-phone title">Accessing Parameters and RouteData from middleware</h3>
        <p>
            As discussed in the previous section, when using pipelining the middleware\actions get executed <strong>after</strong> routing has completed and so have access to all the parameters and variables that were set during matching. Superscribe makes these available to subsequent middleware via entries in the Owin environment dictionary:
        </p>
        <ul>
            <li><strong>"route.Parameters"</strong> - Contains just the route\querystring parameters that were captured</li>
            <li><strong>"superscribe.RouteData"</strong> - The complete Superscribe RouteData object</li>            
        </ul>
        <p>
            Any subsequent middleware can then utilise these values, as seen below:
        </p>
        <pre class="prettyprint">

    public class AddName
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public AddName(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await this.next(environment);

            var parameters = environment["route.Parameters"] as IDictionary<string, object>;
            if (parameters != null && parameters.ContainsKey("Name"))
            {
                await environment.WriteResponse(" " + parameters["Name"]);    
            }
        }
    }
        </pre>
        <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(o.ToString()) });

            app.UseSuperscribeRouter(config)
                .Use(typeof(AddName))
                .UseSuperscribeHandler(config);

            ʃ.Route(ʅ => ʅ / "Hello" / (ʃString)"Name" * (o => "Hello"));
        }
    }
        </pre>
      </div>
	</div>
  </div>
</div>