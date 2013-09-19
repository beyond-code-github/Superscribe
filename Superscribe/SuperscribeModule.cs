namespace Superscribe
{
    public class SuperscribeModule
    {
        public SuperscribeModule()
        {
            this.Get = new MethodSet(o => ʃ.Get(o));
            this.Put = new MethodSet(o => ʃ.Put(o));
            this.Post = new MethodSet(o => ʃ.Post(o));
            this.Patch = new MethodSet(o => ʃ.Patch(o));
            this.Delete = new MethodSet(o => ʃ.Delete(o));
        }

        public MethodSet Get { get; private set; }

        public MethodSet Put { get; private set; }

        public MethodSet Post { get; private set; }

        public MethodSet Patch { get; private set; }
        
        public MethodSet Delete { get; private set; }
    }
}
