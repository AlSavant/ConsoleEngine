using ConsoleEngine.Services.AssetManagement.Strategies;
using DataModel.Attributes;
using IServiceProvider = DependencyInjection.IServiceProvider;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEngine.Services.Factories.Implementations
{
    internal sealed class SerializationStrategyFactory : ISerializationStrategyFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SerializationStrategyFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Dictionary<string, ISerializationStrategy> CreateInstances()
        {
            var implementations = typeof(ISerializationStrategy).Assembly.GetTypes().Where(x =>
                typeof(ISerializationStrategy).IsAssignableFrom(x) &&
                !x.IsInterface &&
                !x.IsAbstract);
            var dict = new Dictionary<string, ISerializationStrategy>();
            foreach(var type in implementations)
            {
                var attributes = type.GetCustomAttributes(typeof(ResourceExtensionAttribute), true);
                if (attributes == null || attributes.Length <= 0)
                    continue;
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType == null)
                    continue;
                foreach(var att in attributes)
                {
                    var extensionAttribute = (ResourceExtensionAttribute)att;
                    foreach(var extension in extensionAttribute.extensions)
                    {
                        if (dict.ContainsKey(extension))
                            continue;
                        dict.Add(extension, (ISerializationStrategy)serviceProvider.Resolve(interfaceType));
                    }
                }
            }
            return dict;
        }
    }
}
