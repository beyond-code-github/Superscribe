---
layout: default
title:  Examples
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
			<li>
				<a href="#unittesting" data-toggle="tab">
					Unit Testing<small>Shows how to unit test Superscribe routes (Generic)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
            <li>
                <a href="#webapibasic" data-toggle="tab">
                    Web Api - Basic<small>How to replicate the default route with Superscribe (Web Api)</small>
                    <i class="icon-angle-right"></i>
                </a>
            </li>
			<li>
				<a href="#webapimultiplecollections" data-toggle="tab">
					Multiple Collections Per Controller<small>Shows how to route multiple collection resources to the same controller (Web Api)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
            <li>
                <a href="#alongsidetraditionalrouting" data-toggle="tab">
                    Combine with other Routing<small>Using Superscribe routes and modules alongside traditional/attribue routing (Web Api)</small>
                    <i class="icon-angle-right"></i>
                </a>
            </li>
            <li>
                <a href="#webapionowin" data-toggle="tab">
                    Web Api on Owin<small>How to set up Superscribe to work with Web Api when using Owin hosting (Web Api, Owin)</small>
                    <i class="icon-angle-right"></i>
                </a>
            </li>
			<li>
				<a href="#owinframeworkhandover" data-toggle="tab">
					Handing control to frameworks<small>Shows how to perform routing and then pass control over to web frameworks accordingly (Owin)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>	
			<li>
				<a href="#owinhelloworld" data-toggle="tab">
					Hello World<small>Basic hello world example (Owin)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>	
			<li>
				<a href="#owinmodules" data-toggle="tab">
					Owin Modules<small>Shows how to set up modules that accept POST requests and perform model binding (Owin)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
			<li>
				<a href="#owinparameters" data-toggle="tab">
					Using Parameters in other Middleware<small>Shows how to use parameters captured during routing in other middleware (Owin)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
			<li>
				<a href="#owinpipelining" data-toggle="tab">
					Routing and Pipelining Middleware<small>Shows how to build a custom pipeline branch based on a route (Owin)</small>
					<i class="icon-angle-right"></i>
				</a>
			</li>
            <li>
                <a href="#owindependencyinjection" data-toggle="tab">
                    Configuring Dependencies per Request<small>Using .Pipeline() to configure dependency injection based on routes (WebApi, Owin)</small>
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

            this.ActionFunctions.Add("Set_" + name, (routeData, value) =>
            {
                int parsed;
                if (int.TryParse(value, out parsed))
                    routeData.Parameters.Add(name, parsed);
            });
        }
    }
			</pre>
			<pre class="prettyprint">

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
        
            // Set up a route that will respond only to even numbers using the fluent api
            var products = new ConstantNode("Products");
            var evenNumber = new EvenNumberNode("id");

            var helloRoute = products.Slash(evenNumber);
            helloRoute.FinalFunctions.Add(
                new FinalFunction("GET", o => "Product id: " + o.Parameters.id));

            define.Route(helloRoute);
            
            app.UseSuperscribeRouter(define)
                .UseSuperscribeHandler(define);
        }
    }
			</pre>
		  	</div>
		  	<div class="tab-pane col-sm-12 col-md-12" id="unittesting">
	  			<h3>Unit testing Superscribe routes</h3>	  			
				<pre class="prettyprint">

	[TestClass]
    public class SuperscribeUnitTests
    {
        private IRouteEngine define;
            
        [TestInitialize]
        public void Setup()
        {
            define = RouteEngineFactory.Create();

            define.Route("Hello/World", o => "Hello World!");
            define.Route("Hello" / (String)"Name", o => "Hello " + o.Parameters.Name);
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
            <div class="tab-pane col-sm-12 col-md-12" id="webapibasic">
                <h3>Using Superscribe with Asp.Net Web Api</h3>
                <pre class="prettyprint">

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var engine = SuperscribeConfig.Register(config);
            engine.Route("api" / Any.Controller / (Int)"id");
        }
    }
                </pre>
                <pre class="prettyprint">

    public class ValuesController : ApiController
    {
        public IEnumerable&lt;string&gt; Get()
        {
            return new[] { "Value1", "Value2" };
        }

        public string Get(int id)
        {
            return "Value" + id;
        }

        public string Post(int id)
        {
            return "Post id: " + id;
        }
    }
                </pre>
            </div>
		  	<div class="tab-pane col-sm-12 col-md-12" id="webapimultiplecollections">
	  			<h3>Multiple collection resources per controller in Asp.Net Web Api</h3>
				<pre class="prettyprint">

	public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            onfig.Formatters.Remove(config.Formatters.XmlFormatter);

            var define = SuperscribeConfig.Register(config);

            var blogs = define.Route("api" / "Blogs".Controller() / (Int)"blogid");

            define.Get(blogs / "Posts", To.Action("GetBlogPosts"));
            define.Get(blogs / "Tags", To.Action("GetBlogTags"));

            define.Post(blogs / "Posts", To.Action("PostBlogPost"));
            define.Post(blogs / "Tags", To.Action("PostBlogTag"));
        }
    }
				</pre>
				<pre class="prettyprint">

	public class BlogsController : ApiController
    {
        public string Get()
        {
            return "Get";
        }

        public string GetById(int blogid)
        {
            return "GetById";
        }

        public string GetBlogPosts()
        {
            return "Blog Posts";
        }

        public string GetBlogTags()
        {
            return "Blog Tags";
        }

        public string GetBlogTags(string query)
        {
            return "Blog Tags - " + query;
        }

        public string PostBlogPost(int blogid)
        {
            return "Post to blog " + blogid;
        }

        public string PostBlogTag(int blogid)
        {
            return "Tag blog " + blogid;
        }
    }
    			</pre>
		  	</div>
			<div class="tab-pane col-sm-12 col-md-12" id="alongsidetraditionalrouting">
                <h3>Using Superscribe routes and modules alongside traditional/attribue routing in Asp.Net Web Api</h3>
                <pre class="prettyprint">

    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }
    }
                </pre>
                <pre class="prettyprint">

    public class MoviesController : ApiController
    {
        [Route("movies")]
        public IEnumerable<string> Get()
        {
            return new[] { "The Matrix", "Lord of the Rings" };
        }
    }
                </pre>
                <pre class="prettyprint">

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = o => new { Message = "Hello World" };

            this.Get["Hello" / (String)"Name"] = 
                o => new { Message = "Hello, " + o.Parameters.Name };
        }
    }
                </pre>                
                <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
            var httpconfig = new HttpConfiguration();

            httpconfig.Formatters.Remove(httpconfig.Formatters.XmlFormatter);
            httpconfig.MapHttpAttributeRoutes();
            httpconfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "values", id = RouteParameter.Optional });

            SuperscribeConfig.RegisterModules(httpconfig, define);

            // Access values controller without falling back to web api routing
            define.Route("values".Controller());
            
            app.UseSuperscribeRouter(define)
                .UseWebApiWithSuperscribe(httpconfig, define);
        }
    }                                    
                </pre>
            </div>
            <div class="tab-pane col-sm-12 col-md-12" id="webapionowin">
                <h3>Using Web Api on Owin with Superscribe Router</h3>    
                <div class="well well-mini pull-center"><em>For this example you'll need to install the <strong>Superscribe.WebApi.Owin</strong> package from nuget.</em></div>
                <pre class="prettyprint">

    public class ValuesController : ApiController
    {
        public IEnumerable&lt;string&gt; Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }
    }
                </pre>
                <pre class="prettyprint">

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
            var httpconfig = new HttpConfiguration();
            
            SuperscribeConfig.RegisterModules(httpconfig, define);

            define.Route("values".Controller());
            
            app.UseSuperscribeRouter(define)
                .UseWebApi(httpconfig)
                .WithSuperscribe(httpconfig, define);
        }
    }            
                </pre>
            </div>
            <div class="tab-pane col-sm-12 col-md-12" id="owinframeworkhandover">
	  			<h3>Handing control from Supersribe to web frameworks for handling requests</h3>	  			
				<pre class="prettyprint">

	public class HelloController : ApiController
    {
        public string Get()
        {
            return "Hello from Web Api";
        }
    }
				</pre>
				<pre class="prettyprint">

	public class HelloModule : NancyModule
    {
        public HelloModule()
            : base("api/nancy")
        {
            this.Get["/"] = _ => "Hello from nancy";
        }
    }
				</pre>
				<pre class="prettyprint">

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
				</pre>
			</div>
		  	<div class="tab-pane col-sm-12 col-md-12" id="owinhelloworld">
	  			<h3>Owin Hello World</h3>	  			
				<pre class="prettyprint">

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();
            
            define.Route("Hello/World", o => "Hello World");
            
            app.UseSuperscribeRouter(define)
                .UseSuperscribeHandler(define);
        }
    }
				</pre>
			</div>
			<div class="tab-pane col-sm-12 col-md-12" id="owinmodules">
	  			<h3>Using modules in Owin to accept and return Json</h3>	  			
				<pre class="prettyprint">

	public class Product
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }
    }
				</pre>
				<pre class="prettyprint">

	public class ProductsModule : SuperscribeOwinModule
    {
        private readonly List&lt;Product&gt; products = new List&lt;Product&gt;
        {
            new Product { Id = 1, Description = "Shoes", Price = 123.50, Category = "Fashion"},
            new Product { Id = 2, Description = "Hats", Price = 55.20, Category = "Fashion"},
            new Product { Id = 3, Description = "iPad", Price = 324.50, Category = "Electronics"},
            new Product { Id = 4, Description = "Kindle", Price = 186.20, Category = "Electronics"},
            new Product { Id = 5, Description = "Dune", Price = 13.50, Category = "Books"}
        };

        public ProductsModule()
        {
            this.Get["Products"] = o => products;

            this.Get["Products" / (Int)"Id"] = o => 
                products.FirstOrDefault(p => o.Parameters.Id == p.Id);

            this.Get["Products" / (String)"Category"] = o => 
                products.Where(p => o.Parameters.Category == p.Category);

            this.Post["Products"] = o =>
                {
                    var product = o.Bind&lt;Product&gt;();
                    return new { Message = string.Format(
                        "Received product id: {0}, description: {1}", 
                        product.Id, product.Description) };
                };
        }
    }
				</pre>
				<pre class="prettyprint">

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new SuperscribeOwinOptions();
            options.MediaTypeHandlers.Remove("text/html");
            options.MediaTypeHandlers.Add("application/json", new MediaTypeHandler
            {
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

            // Replace text/html with json handler so example works in a browser
            options.MediaTypeHandlers.Add("text/html", options.MediaTypeHandlers["application/json"]);

            var engine = OwinRouteEngineFactory.Create(options);

            app.UseSuperscribeRouter(engine)
                .UseSuperscribeHandler(engine);
        }
    }
				</pre>
			</div>
			<div class="tab-pane col-sm-12 col-md-12" id="owinparameters">
	  			<h3>Using parameters captured during routing in other Middleware</h3>	  							
				<pre class="prettyprint">

	public class SayHello
    {
        private readonly Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next;

        public SayHello(Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary&lt;string, object&gt; environment)
        {
            var parameters = environment["route.Parameters"] as IDictionary&lt;string, object&gt;;
            if (parameters != null &amp;&amp; parameters.ContainsKey("Name"))
            {
                await environment.WriteResponse("Hello " + parameters["Name"]);    
            }
        }
    }
				</pre>
				<pre class="prettyprint">

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();

            define.Route("Hello" / (String)"Name", o => "Hello");

            app.UseSuperscribeRouter(define)
                .Use&lt;SayHello&gt;();
        }
    }
				</pre>
			</div>
			<div class="tab-pane col-sm-12 col-md-12" id="owinpipelining">
	  			<h3>Building and executing an Owin pipeline branch during routing</h3>	  			
				<pre class="prettyprint">
				
	public class PadResponse
    {
        private readonly Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next;

        private readonly string tag;

        public PadResponse(Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next, string tag)
        {
            this.next = next;
            this.tag = tag;
        }

        public async Task Invoke(IDictionary&lt;string, object&gt; environment)
        {
            await environment.WriteResponse("&lt;" + tag + "&gt;");
            await this.next(environment);
            await environment.WriteResponse("&lt;/" + tag + "&gt;");
        }
    }
				</pre>
				<pre class="prettyprint">

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var define = OwinRouteEngineFactory.Create();

            define.Pipeline("Hello").Use(typeof(PadResponse), "h1");
            define.Pipeline("Goodbye").Use(typeof(PadResponse), "marquee");
            
            define.Route("Hello/World", o => "Hello World");
            define.Route("Goodbye/World", o => "Goodbye World");

            app.UseSuperscribeRouter(define)
              .UseSuperscribeHandler(define);            
        }
    }
				</pre>
			</div>		
            <div class="tab-pane col-sm-12 col-md-12" id="owindependencyinjection">
                <h3>Use Superscribe with DotNetDoodle.Owin.Dependencies to configure your dependency container based on your routes</h3>              
                <div class="well well-mini pull-center"><em>For this example you'll need to install <strong>DotNetDoodle.Owin.Dependencies</strong> and package from nuget.For more information on DotNetDoodle.Owin.Dependencies, please see <a href="http://www.tugberkugurlu.com/archive/owin-dependencies--an-ioc-container-adapter-into-owin-pipeline">this post by Tuberk Ugurlu.</a></em></div>
                <pre class="prettyprint">	
    public class ValuesController : ApiController
    {
        private readonly IRepository repository;

        public ValuesController(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable&lt;string&gt; Get()
        {
            return this.repository.Values();
        }
    }           
                </pre>
                <pre class="prettyprint">   
    public interface IRepository
    {
        IEnumerable&lt;string&gt; Values();
    }

    public class Repository : IRepository
    {
        public IEnumerable&lt;string&gt; Values()
        {
            return new[] { "value1", "value2" };
        }
    }

    public class ApiRepository : IRepository
    {
        public IEnumerable&lt;string&gt; Values()
        {
            return new[] { "value3", "value4" };
        }
    }
                </pre>
                <pre class="prettyprint">  
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var engine = OwinRouteEngineFactory.Create();

            var httpconfig = new HttpConfiguration();
            httpconfig.Formatters.Remove(httpconfig.Formatters.XmlFormatter);
            SuperscribeConfig.Register(httpconfig, engine);

            engine.Route("Values".Controller());
            engine.Route("Api" / "Values".Controller());

            engine.Pipeline("Api").Use&lt;ApiDependencies&gt;();

            appBuilder.UseAutofacContainer(this.RegisterServices())
                .UseSuperscribeRouter(engine)
                .UseWebApiWithContainer(httpconfig)
                .WithSuperscribe(httpconfig, engine);
        }

        public IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterOwinApplicationContainer();

            builder.RegisterType&lt;Repository&gt;()
                   .As&lt;IRepository&gt;()
                   .InstancePerLifetimeScope();
            
            return builder.Build();
        }
    }
                </pre>
                <pre class="prettyprint">
                
    public class ApiDependencies
    {
        private readonly Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next;

        public ApiDependencies(Func&lt;IDictionary&lt;string, object&gt;, Task&gt; next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary&lt;string, object&gt; environment)
        {
            var container = environment.GetRequestContainer() as ILifetimeScope;
            var newBuilder = new ContainerBuilder();
            newBuilder.RegisterType&lt;ApiRepository&gt;()
                   .As&lt;IRepository&gt;()
                   .InstancePerLifetimeScope();

            newBuilder.Update(container.ComponentRegistry);

            await this.next(environment);
        }
    }                    
                </pre>
            </div>
		</div>
	</div>
</div>