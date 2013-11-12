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
      <li class="active"> <a href="#tab1" data-toggle="tab">Fluent API & DSL<small>Two simple ways to define hierarchical & strongly typed route definitions</small><i class="icon-angle-right"></i></a> </li>
       <li> <a href="#tab2" data-toggle="tab">Easy unit testing<small>Invoke the superscribe routing engine in isolation from the rest of your app</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#tab3" data-toggle="tab">Asp.Net Web API routing<small>Replace existing routes with syntax thats much more concise and easy to manage</small><i class="icon-angle-right"></i></a> </li>
      <li> <a href="#tab4" data-toggle="tab">Bring Nancy style modules to Web API<small>All the benefits of the code-centric approach combined with graph based routing</small><i class="icon-angle-right"></i></a> </li>   
      <li> <a href="#tab5" data-toggle="tab">Serve your data direct from OWIN<small>Create ultra-lightweight services for maximum performance</small><i class="icon-angle-right"></i></a> </li>
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
		<p>This behavior isn't entirely useless... we can use an Action Function to set some values which then become available to our final function and influence it's behavior. Another way of making the Superscribe engine treat 'Hello' as optional is to assign it a Final Function of it's own:</p>
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
        
	  </div>
	  <div class="tab-pane col-sm-12 col-md-12" id="tab2">
        <h3 class="title visible-phone"></h3>
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
      <div class="tab-pane col-sm-12 col-md-12" id="tab5">
        <h3 class="title visible-phone"></h3>
        <pre class="prettyprint lang-cs">
		</pre>
	  </div>
	</div>
  </div>
</div>