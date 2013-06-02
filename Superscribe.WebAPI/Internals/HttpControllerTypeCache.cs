namespace Superscribe.WebAPI.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    public class HttpControllerTypeCache
    {
        private readonly HttpConfiguration configuration;

        private readonly Lazy<Dictionary<string, ILookup<string, Type>>> cache;

        public HttpControllerTypeCache(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.configuration = configuration;
            this.cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(this.InitializeCache);
        }

        internal Dictionary<string, ILookup<string, Type>> Cache
        {
            get { return this.cache.Value; }
        }

        public ICollection<Type> GetControllerTypes(string controllerName)
        {
            if (String.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }

            var matchingTypes = new HashSet<Type>();

            ILookup<string, Type> namespaceLookup;
            if (this.cache.Value.TryGetValue(controllerName, out namespaceLookup))
            {
                foreach (var namespaceGroup in namespaceLookup)
                {
                    matchingTypes.UnionWith(namespaceGroup);
                }
            }

            return matchingTypes;
        }

        private Dictionary<string, ILookup<string, Type>> InitializeCache()
        {
            var assembliesResolver = this.configuration.Services.GetAssembliesResolver();
            var controllersResolver = this.configuration.Services.GetHttpControllerTypeResolver();

            var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            var groupedByName = controllerTypes.GroupBy(
                t => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length),
                StringComparer.OrdinalIgnoreCase);

            return groupedByName.ToDictionary(
                g => g.Key,
                g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
