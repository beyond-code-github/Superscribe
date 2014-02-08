namespace Superscribe.ScriptCS
{
    using System.ComponentModel.Composition;
    using System.Linq;

    using ScriptCs.Contracts;

    public class ScriptPack : IScriptPack
    {
        [ImportingConstructor]
        public ScriptPack()
        {
        }

        IScriptPackContext IScriptPack.GetContext()
        {
            return new Superscribe();
        }

        void IScriptPack.Initialize(IScriptPackSession session)
        {
            session.AddReference("System.Net.Http");
            var namespaces = new[]
                {
                    "System.Web.Http",
                    "System.Web.Http.SelfHost",
                    "System.Web.Http.Dispatcher",
                    "Owin"
                }.ToList();

            namespaces.ForEach(session.ImportNamespace);
        }

        void IScriptPack.Terminate()
        {
        }
    }
}
