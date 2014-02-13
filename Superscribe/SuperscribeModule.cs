namespace Superscribe
{
    using Superscribe.Engine;

    public class SuperscribeModule : SuperscribeModule<IModuleRouteData>
    {
    }

    public class SuperscribeModule<T>
        where T : IModuleRouteData
    {
        public SuperscribeModule()
        {
            this.Get = new MethodSet<T>("GET");
            this.Put = new MethodSet<T>("PUT");
            this.Post = new MethodSet<T>("POST");
            this.Patch = new MethodSet<T>("PATCH");
            this.Delete = new MethodSet<T>("DELETE");
        }

        public void Initialise(IRouteEngine engine)
        {
            this.Get.Initialise(engine);
            this.Put.Initialise(engine);
            this.Post.Initialise(engine);
            this.Patch.Initialise(engine);
            this.Delete.Initialise(engine);
        }

        public MethodSet<T> Get { get; private set; }

        public MethodSet<T> Put { get; private set; }

        public MethodSet<T> Post { get; private set; }

        public MethodSet<T> Patch { get; private set; }

        public MethodSet<T> Delete { get; private set; }
    }
}
