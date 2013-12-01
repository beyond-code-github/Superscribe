---
layout: default
title:  Features
---

<div class="block">
	<h2 class="title-divider"><span>Examples</span>
	<small>Learn by example with our comprehensive code samples and demo projects</small>
	</h2>
	<div class="well well-mini pull-center">
		<em>All samples shown here can be downloaded in project form from <a href="https://github.com/Roysvork/Superscribe.Samples">The Superscribe.Samples Github Repo</a>. Samples showing generic functionality will be hosted using Owin but are applicable to all Superscribe project types.</em>
  	</div>
	<div class="tabbable tabs-left vertical-tabs bold-tabs row">
		<ul class="nav nav-tabs nav-stacked col-md-4">
			<li class="active">
				<a href="#fluentapi" data-toggle="tab">
					Fluent Api<small>Shows how to define routes and custom nodes using the Fluent Api (Generic)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
		</ul>    
		<div class="tab-content col-md-8">
		  <div class="tab-pane active col-sm-12 col-md-12" id="fluentapi">
		  	<h3>Defining routes with the Fluent Api</h3>
			<pre class="prettyprint">

	public class EvenNumberNode : GraphNode
    {
        public EvenNumberNode(string name)
        {
            this.activationFunction = (routeData, value) =>
            {
                int parsed;
                if (int.TryParse(value, out parsed))
                    return parsed % 2 == 0; // Only match even numbers

                return false;
            };

            this.actionFunction = (routeData, value) =>
            {
                int parsed;
                if (int.TryParse(value, out parsed))
                    routeData.Parameters.Add(name, parsed);
            };
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
                new MediaTypeHandler
                    {
                        Write = (env, o) => env.WriteResponse(o.ToString())
                    });

            app.UseSuperscribeRouter(config)
                .UseSuperscribeHandler(config);

            // Set up a route that will respond only to even numbers using the fluent api

            var helloRoute = new ConstantNode("Products").Slash(new EvenNumberNode("id"));
            helloRoute.FinalFunctions.Add(new FinalFunction("GET", o => "Product id: " + o.Parameters.id));

            ʃ.Base.Zip(helloRoute.Base());
        }
    }
			</pre>
		  </div>
		</div>
	</div>
</div>