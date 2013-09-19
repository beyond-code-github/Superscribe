namespace Superscribe
{
    public class SuperscribeModule<T>
    {
        public SuperscribeModule()
        {
            this.Get = new MethodSet<T>(o => ʃ.Get(o));
            this.Put = new MethodSet<T>(o => ʃ.Put(o));
            this.Post = new MethodSet<T>(o => ʃ.Post(o));
            this.Patch = new MethodSet<T>(o => ʃ.Patch(o));
            this.Delete = new MethodSet<T>(o => ʃ.Delete(o));
        }

        public MethodSet<T> Get { get; private set; }

        public MethodSet<T> Put { get; private set; }

        public MethodSet<T> Post { get; private set; }

        public MethodSet<T> Patch { get; private set; }

        public MethodSet<T> Delete { get; private set; }
    }
}
