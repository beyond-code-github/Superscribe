---
layout: default
title:  Documentation
---

<div class="block">
<h2 class="title-divider"><span>Documentation</span>
<small>Information and resources to help you get started with Superscribe</small>
</h2>
  <div class="tabbable tabs-left vertical-tabs bold-tabs row">
    <ul class="nav nav-tabs nav-stacked col-md-4">
      <li class="active"> <a href="#fluentapi" data-toggle="tab">Fluent API<small>Define hierarchical and strongly typed routes with Superscribe</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#dsl" data-toggle="tab">Simple-syntax<small>Shorthands for defining routes that are concise and easy to maintain</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#modules" data-toggle="tab">Modules<small>Inspired by NancyFX, a great way to keep your definitions and your handlers together</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#webapi" data-toggle="tab">Superscribe.WebAPI<small>Specific syntax to help you match routes and then invoke controllers and actions</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#owin" data-toggle="tab">Superscribe.OWIN<small>Branch your pipeline during routing and hand control to any OWIN middleware</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
	<div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="fluentapi">
      	<h3 class="visible-phone">Defining routes using superscribe's fluent interface</h3>
      	<p>This section starts with a disclaimer. In practice you won't want to write routes using the Fluent API, as they won't look very nice and will be quite verbose; instead you'll be using the simple-syntax wherever possible. However, to work with Superscribe effectively and to lessen any learning curves, it is useful to understand what is going on behind the scenes. As a result, this section should be considered required reading before continuing to the later topics</p>
        <h4>The IRouteEngine interface</h4>
        <p>Superscribe's features are all accessed via an instance of a class that implements <em>IRouteEngine</em>. There are currently two Route Engine implementations, one for WebApi, and one for OWIN. You can obtain an instance using the static factory classes provided, as follows:</p>
        <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create(); 
    // or...
    var engine = OwinRouteEngineFactory.Create();
        </pre>
        <p>You can have as many instances of <em>IRouteEngine</em> as you like, and they will operate indenpendently. Usually - as in most examples on this site - only one is needed (although it is possible to transfer control from one to another when moving between frameworks or application layers).</p>
        <h4>The GraphNode class and Final Functions</h4>
        <p>Graph Based Routing definitions are constructed using strongly typed nodes and then stored as a graph. Within each instance of a <em>RouteEngine</em> there is a <strong>Base node</strong> that represents the root '/' url and is also the parent for any susequent route definitions.</p>
        <p>The base node is accesible via the <em>.Base</em> property of the Route Engine, and is an instance of type <em>GraphNode</em>. All other nodes in Superscribe derive from this class, and is the building block of all route definitions.</p>
        <p>For example, if we want to respond to a request to '/', we need to provide the base node with a <strong>Final Function</strong>:</p>
        <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create(); 
    engine.Base.FinalFunctions.Add(new FinalFunction("GET", o => @"
        Welcome to Superscribe 
        &lta href='/Hello/World'&gtSay hello...&lt/a&gt
    "));

    // "/" -> "Welcome to Superscribe..."
		</pre>
		<p>A Final Function is associated with an HTTP method and executed when routing finishes at a particular node. Multiple final functions can be supplied, each responding to a different HTTP method. In this case we have configured Superscribe to respond to a GET request to '/', returning a message and a link.</p>
        <p>To respond to the uri in the link, we need to add some more nodes to our graph:</p>
		<pre class="prettyprint lang-cs">

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello World"));

    engine.Base.Zip(helloRoute.Base());

    // "/Hello/World" -> "Hello World"
		</pre>
		<p>The above sample introduces some of the methods that the <em>GraphNode</em> class provides... <em>Slash</em>, <em>Zip</em> and <em>Base</em>, as well as a new subclass of <em>GraphNode</em>, the <em>ConstantNode</em>. These work as follows:
			<ul>
				<li>The <strong>ConstantNode</strong> as it's name suggests, will only match a route segment that is identical to the value passed to it in the constructor</li>
				<li>The <strong>Slash</strong> function creates an edge between two nodes and then returns the child node.</li>
				<li>The <strong>Base</strong> function returns the topmost parent of a node. (remember, .Slash returns the child "World" node but we want to create an edge in the base node that points to the "Hello" node.</li>
				<li>Finally, the <strong>Zip</strong> function performs a similar function to .Slash but with one important difference... it will combine any equivilent nodes (See the next code block for an example).</li>
			</ul>
		</p>
		<h4>Final Functions and Frameworks/Handlers</h4>
		<p>When running using the Superscribe Handler middleware for OWIN, the samples above create an app that will respond to '/' and '/Hello/World', but not 'Hello':</p>
		<pre class="prettyprint lang-cs">

    // "/Hello" -> 405 - No final function was configured for this method
		</pre>
		<p>This is because the "Hello" node itself does not provide a Final Function, and the Superscribe Handler has no other way of responding to the request. The following snippet shows how we can assign one, and demonstrates the behavior of the <em>.Zip</em> function to combine the two 'Hello' nodes into one:
		</p>
         <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create(); 

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello World"));

    var justHelloRoute = new ConstantNode("Hello");
    justHelloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello... maybe"));

    engine.Base.Zip(helloRoute.Base());
    engine.Base.Zip(justHelloRoute.Base());

    // "/Hello" -> "Hello... maybe"
    // "/Hello/World" -> "Hello World"
        </pre>
        <p>It is worth noting that the Route Engine never responds to requests directly, it is up to the framework or handler middleware to perform that task. This affords the maximum amount of flexibility, for example although a final function is mandatory when we use the Superscribe Owin Handler, when using Superscribe with Web Api we rely on it's internal pipeline to service the request and route to a controller/action accordingly. 
        </p>
        <h4>The RouteData object</h4>
		<p>What the engine does provide however is a set of results and flags in the form of the <em>RouteData</em> object that any framework or middleware is free to make use of as it likes. The <em>RouteData</em> object is an instance of a class implementing the <em>IRouteData</em> interface:</p>
        <pre class="prettyprint lang-cs">

    public interface IRouteData
    {
        string Url { get; set; }
        string Method { get; set; }        
        dynamic Parameters { get; set; }
        IDictionary<string, object> Environment { get; set; }
        object Response { get; set; }        
        bool ExtraneousMatch { get; set; }
        bool FinalFunctionExecuted { get; set; }
        bool NoMatchingFinalFunction { get; set; }
    }
        </pre>
        <p>The <em>RouteData</em> object gets passed in to each final function, providing it with any context it needs to respond to the request. The result of the final function is assigned to the <em>Response</em> property so that the framework or handler can use it when building any response message.</p>
        <p>Some of the properties are self-descriptive, but the others are as follows:</p> 
        <ul>
            <li><strong>Parameters</strong> - A dynamic dictionary containg name/value pairs representing all the parameters and querystring values that were matched during routing.</li>
            <li><strong>Environment</strong> - When dealing directly with Owin, the <em>Environment</em> property will provide a reference to the Owin environment dictionary. In the case of Web Api, it will provide a dictionary exposing any specific properties such as the request context, or resolved action/controller names.</li>
            <li><strong>ExtraneousMatch</strong> - The engine ran out of nodes but did not process all the route segments</li>
            <li><strong>FinalFunctionExecuted</strong> - All segments were consumed, and a final function was matched and executed</li>
            <li><strong>NoMatchingFinalFunction</strong> - Indicates that although all segments were consumed and the find node did provide one or more final functions - none of them matched the request method (GET, POST, etc)</li>
        </ul>
        <h4>Activation Functions</h4>
        <p>All nodes come with an <strong>Activation Function</strong>. An Activation Function returns a boolean that determines whether or not the node is a match for the current segment. During matching, the engine will examine each edge of a node in turn until it finds one that matches the next segment... i.e the Activation Function returns true.
        <p><em>GraphNodes</em> come with a default activation function that caters towards both explicit and regex matches. The following shows the default Activation Function of the <em>GraphNode</em>, along with it's signature. When instantiating a <em>ConstantNode</em> for example, the <em>Template</em> will be set by the constructor. If a <em>Pattern</em> is set on a node, then a Regex match is performed.<p>
        <pre class="prettyprint lang-cs">
        
    this.activationFunction = (routedata, segment) =>
    {
        if (this.Pattern != null)
        {
            return this.Pattern.IsMatch(segment);
        }

        return string.Equals(this.Template, segment, StringComparison.OrdinalIgnoreCase);
    };
        </pre>
        <h4>Action Functions and Parameters</h4>
        <p><strong>Action functions</strong> are a bit like Final Functions, but they are executed as each route segment is matched and are not able to directly influence the response. They do however have access to the <em>RouteData</em> object and are able to write to the <em>Environment</em> dictionary</p>
        <p>A node can have any number of action functions, each associated with a unique string key. Unlike their Final counterparts they are not specific to any HTTP method (although this behavior is easily achieved). When equivilent nodes are combined using <em>.Zip</em>, their sets of Action Functions will also be combined so long as keys remain unique.</p>
        <p>We've already mentioned how Superscribe provides access to parameters, but Action Functions are the mechanism by which they are captured. Superscribe provides a generic class called <em>ParamNode&lt;T&gt;</em> that derives from <em>GraphNode</em> and provides a ready made parameter capture action. We can use this to capture any data type we might need:</p>
        <ul>
        	<li><strong>Integer</strong> ParamNode&lt;int&gt;</li>
        	<li><strong>Long</strong> ParamNode&lt;long&gt;</li>
        	<li><strong>String</strong> ParamNode&lt;string&gt;</li>
        	<li><strong>Boolean</strong> ParamNode&lt;bool&gt;</li>
        	<li><strong>Guid</strong> ParamNode&lt;Guid&gt;</li>
        </ul>
        <p>For efficiency, the <em>ParamNode</em> Activation Function evaluates the current segment using TryParse, and then stores the result for the Action Function to pick up. This is not something that is easy to do using traditional route constraints, but becomes easy with Graph Based Routing.</p>
        <p>An example of parameter capture with Superscribe is as follows:</p>
        <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create(); 

    var helloRoute = new ConstantNode("Hello").Slash(new ParamNode&lt;string&gt;("Name"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello " + o.Parameters.Name));

    engine.Base.Zip(helloRoute.Base());

    // "/Hello/Kathryn" -> "Hello Kathryn"
		</pre>
		<h4>Custom Nodes</h4>
		<p>In addition to leveraging the various subclasses of <em>GraphNode</em>, we can also derive our own custom nodes, giving us full control over it's Action and Activation Functions. Consider the following custom node that matches and captures only even parameters:</p>
		<pre class="prettyprint lang-cs">

	public class EvenNumberNode : SuperscribeNode
    {
        private int parsed;

        public EvenNumberNode(string name)
        {
            this.activationFunction = (routeData, segment) => {
                if (int.TryParse(segment, out this.parsed))
                {
                    return parsed % 2 == 0; // Only match even numbers
                }

                return false;
            };

            this.ActionFunctions.Add("Set_" + name, 
                (routeData, segment) => routeData.Parameters.Add(name, this.parsed);
        }
    }
		</pre>
		<p>The <em>EvenNumberNode</em> duplicates the functionality of <em>ParamNode&lt;int&gt;</em> but with an added match condition. This example is a little contrivied; it's not a particularly useful Activation Function and we could easily derive from <em>ParamNode</em> and re-use it's Action Function instead of adding our own. Having said that, it's a good example of the flexibility of Graph Based Routing.</p>
		<p>Using our custom node in a route definition is just the same as with any of the other node types. Any traditional route constraint can be modelled in this way, along with many more that are not possible with traditional routing.</p>
		<pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create();

    var productIdRoute = new ConstantNode("Products").Slash(new EvenNumberNode("Id"));
    productIdRoute.FinalFunctions.Add(
    	new FinalFunction("GET", o => "Product info for id: " + o.Parameters.Id));

    engine.Base.Zip(productIdRoute.Base());

    // "/Products/12" -> "Product info for id: 12"
    // "/Products/13" -> "404 - Extraneous Match"
		</pre>
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="dsl">
        <h3>Defining routes with Superscribe simple-syntax</h3>
        <p>As mentioned in the previous section, defining routing using the fluent API is not ideal for a number of reasons. Built on top of the fluent API is a simple domain specific language... valid C# code made using a combination of lambdas, casts and operator overloads.</p>
        <p>The simple-syntax provided by Superscribe is designed to be terse and minimal, so you can get on with the important business of designing your routes. In this section we'll translate an example from the Fluent Api to use the simple-syntax, stage by stage to show which statements are equivilent.</p>
        <div class="well well-mini pull-center">
          <em>If you haven't already read the Fluent API section, it is reccomended that you do so before continuing so you are familiar with some of the terms and concepts used.</em>
        </div>
        <h3 class="title visible-phone">The .Route family</h3>
        <p>When using the Fluent API, we defined routes by manually instantiating subclasses of <em>GraphNode</em>, created edges with <em>Slash()</em> and then attached them to the base node using <em>.Base()</em> and <em>.Zip()</em>:</p>
        <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create();

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello World"));

    engine.Base.Zip(helloRoute.Base());

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>First of all, Superscribe provides a shorthand way of attaching routes to the base node using the <em>.Route</em> method:</p>
        <pre class="prettyprint lang-cs">

    // engine.Base.Zip(helloRoute.Base()); becomes ->
    engine.Route(helloRoute);
        </pre>
        <p>As well as being slightly shorter, this method will also call <em>.Base</em> for us. So far so good, but it isn't much of an improvement... we can still do a bit more. To apply a final function matching the 'GET' method, we can supply it as an argument to one of <em>.Route</em>'s sister methods:</p>
        <pre class="prettyprint lang-cs">
    
    var engine = RouteEngineFactory.Create();

    var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
    engine.Get(helloRoute, o => "Hello World");

    // "/Hello/World" -> "Hello World"
        </pre>
        <p><em>.Get</em> performs the same function as <em>.Route</em> but will also add our final function to the end of the chain, and apply the 'GET' modifier to it. Superscribe provides methods for each of the main Http Methods - GET, POST, PUT, PATCH and DELETE. You'll only really want to use <em>.Route</em> if the framework you're using to handle requests also takes care of Http Methods for you (such as Web Api).</p>
        <h4>Shorthand operators</h4>
        <p>Instead of calling <em>.Slash</em> we can use the shorthand operator '/':</p>
        <pre class="prettyprint lang-cs">
    
    var engine = RouteEngineFactory.Create();

    var helloRoute = new ConstantNode("Hello") / new ConstantNode("World");
    engine.Get(helloRoute, o => "Hello World");

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>Next up, an implicit cast from string to <em>GraphNode</em> provides a way to simplify the instantiation of our constant nodes:</p>
        <pre class="prettyprint lang-cs">
    
    var engine = RouteEngineFactory.Create();
    engine.Get(r => r / "Hello" / "World", o => "Hello World"));

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>When calling any of the methods in the <em>.Route</em> family though, we sometimes encounter a problem. In order to use the Superscribe shorthand operators, at least one of the operands must be a <em>GraphNode</em>.</p>
        <p>In the above example, we solve this by using an overload that takes a lambda with an 'r' parameter. This parameter represents the base node, and so allows the operator chain to resolve itself succesfully. This overload is available to every method in the <em>.Route</em> family.</p>
        <p>Finally for this example, Superscribe has one more trick up it's sleeve. Another overload takes a traditional string-based route, parses it, and creates a graph for us. This gives us a really concise final route definition that still behaves exactly like the original:</p>
        <pre class="prettyprint lang-cs">
    
    var define = RouteEngineFactory.Create();
    define.Get("Hello/World", o => "Hello World"));

    // "/Hello/World" -> "Hello World"
        </pre>
        <p>Note that here we've renamed the 'engine' variable to 'define' which gives us the added bonus of reading really nicely. This is optional of course.</p>
        <h3 class="title visible-phone">GraphNode shorthands</h3>
        <p>Creating a <em>ConstantNode</em> from a string isn't the only way we can take advantage of casting to build routes. In the Fluent API section we demonstrated how Superscribe capture parameters using <em>ParamNode&lt;T&gt;</em>:</p>
        <pre class="prettyprint lang-cs">

    var engine = RouteEngineFactory.Create();

    var helloRoute = new ConstantNode("Hello").Slash(new ParamNode&lt;string&gt;("Name"));
    helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Hello " + o.Parameters.Name));

    engine.Base.Zip(helloRoute.Base());

    // "/Hello/Kathryn" -> "Hello Kathryn"
        </pre>
        <p>Now here's the simple-syntax version:</p>
        <pre class="prettyprint lang-cs">
    
    var define = RouteEngineFactory.Create();
    define.Get("Hello" / (String)"Name", o => "Hello " + o.Parameters.Name);

    // "/Hello/Kathryn" -> "Hello Kathryn"
        </pre>
        <p>The Superscribe <em>String</em> class derives from <em>ParamNode&lt;string&gt;</em> and adds an extra explicit cast operator, the sole purpose of which is to provide us with this much nicer syntax. There are other such shorthands for each of the primary data types supported by Superscribe:</p>
        <ul>
            <li><strong>Integer</strong> (Int)</li>
            <li><strong>Long</strong> (Long)</li>
            <li><strong>String</strong> (String)</li>
            <li><strong>Boolean</strong> (Bool)</li>
        </ul>
        <p>And of course, because all Superscribe nodes are strongly typed and derive from <em>GraphNode</em>, it's easy to create our own shorthands. Here's an example for Guid:</p>
        <pre class="prettyprint lang-cs">

    public class GuidNode : ParamNode&lt;Guid&gt;
    {
        public GuidNode(string name) : base(name) { }

        public static explicit operator GuidNode(string name)
        {
            return new GuidNode(name);
        }
    }
        </pre>
        <h3 class="title visible-phone">Beyond the basics</h3>
        <p>Using the Fluent API and the <em>.Zip</em> function we were able to compose our route graph by combining several complete routes. This still works when using the simple-syntax, as each call to one of the <em>.Route</em> family of methods will automatically call <em>.Zip</em> for us. This is demonstrated in this example modified from one of the Web Api samples:</p>
        <pre class="prettyprint lang-cs">

    var define = RouteEngineFactory.Create();
    
    define.Get ("api" / "Blogs" / (Int)"blogid" / "Posts", o => // Handle blogposts get...);
    define.Post("api" / "Blogs" / (Int)"blogid" / "Posts", o => // Handle blogposts post...);
    define.Get ("api" / "Blogs" / (Int)"blogid" / "Tags",  o => // Handle blogtags get...);
    define.Post("api" / "Blogs" / (Int)"blogid" / "Tags",  o => // Handle blogtags post...);
        </pre>
        <p>
            There's another way of achieving this same result. As all route definitions in Superscribe are strongly typed, it's possible to assign routes or parts of routes to variables which allow us to re-use compose definitions in useful and interesting ways:
        </p>
        <pre class="prettyprint lang-cs">

    var define = RouteEngineFactory.Create();
    
    var blog = define.Route("api" / "Blogs" / (Int)"blogid");

    define.Get (blog / "Posts", o => // Handle blogposts get...);
    define.Post(blog / "Posts", o => // Handle blogposts post...);
    define.Get (blog / "Tags",  o => // Handle blogtags get...);
    define.Post(blog / "Tags",  o => // Handle blogtags post...);
        </pre>
        <p>Note also how we're defining the re-usable component of our route definitions using the original <em>.Route</em> method as it allows us to define a route with no Final Functions or method modifiers.</p>
	  </div>
       <div class="tab-pane col-sm-12 col-md-12" id="modules">
        <h3 class="visible-phone">Handling routes with Modules</h3>
        <p>Asp.Net Web Api uses controllers and actions because it was originally derived from the MVC framework and although things have moved on since then, it still shares some of the same constructs. With Graph Based Routing we are free to break away from these restrictions and handle our routes using whatever classes we wish. A great example of this comes in the form of modules, inspired by the Ruby web framework Sinatra, and the .Net framework NancyFX</p>
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
        <p>Each call to this.Get, or any of the other method helpers ensures that the route graph is still constructed efficiently. Behind the scenes each route is 'Zipped' together (see the Fluent Api section) so that equivilent nodes will be merged. In the above example, we end up with a single "Hello" node with two edges.</p>
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