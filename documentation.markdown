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
      <li class="active"> <a href="#tab1" data-toggle="tab">Fluent API<small>Define hierarchical and strongly typed routes with Superscribe</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#tab2" data-toggle="tab">DSL<small>Shorthand syntax for defining routes that are concise and easy to maintain</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#tab3" data-toggle="tab">Glossary<small>Quick reference for Superscribe terminology</small><i class="icon-angle-right"></i></a> </li>
    </ul>    
	<div class="tab-content col-md-8">
      <div class="tab-pane active col-sm-12 col-md-12" id="tab1">
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
	  <div class="tab-pane col-sm-12 col-md-12" id="tab2">
        <h2>Defining routes with the DSL</h2>
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

public class ʃGuid : ParamNode<Guid>
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
      <div class="tab-pane col-sm-12 col-md-12" id="tab3">
        <h3 class="title visible-phone"></h3>
        <pre class="prettyprint lang-cs">
        </pre>
      </div>
	</div>
  </div>
</div>