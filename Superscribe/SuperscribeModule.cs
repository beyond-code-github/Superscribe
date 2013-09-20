namespace Superscribe
{
    using Superscribe.Models;

    public class SuperscribeModule<T>
    {
        public SuperscribeModule()
        {
            this.ʅ = new RouteGlue();

            this.Get = new MethodSet<T>(o => ʃ.Get(o), "GET");
            this.Put = new MethodSet<T>(o => ʃ.Put(o), "PUT");
            this.Post = new MethodSet<T>(o => ʃ.Post(o), "POST");
            this.Patch = new MethodSet<T>(o => ʃ.Patch(o), "PATCH");
            this.Delete = new MethodSet<T>(o => ʃ.Delete(o), "DELETE");
        }

        public RouteGlue ʅ { get; set; }

        public MethodSet<T> Get { get; private set; }

        public MethodSet<T> Put { get; private set; }

        public MethodSet<T> Post { get; private set; }

        public MethodSet<T> Patch { get; private set; }

        public MethodSet<T> Delete { get; private set; }
    }
}
