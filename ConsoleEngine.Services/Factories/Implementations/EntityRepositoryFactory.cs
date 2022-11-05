using ConsoleEngine.Services.Repositories.Entity;
using DataModel.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Services.Factories.Implementations
{
    internal sealed class EntityRepositoryFactory : IEntityRepositoryFactory
    {
        private readonly IServiceProvider kernel;
        private readonly Dictionary<Type, Type> typeMap;
        private readonly AssemblyPool assemblyPool;

        public EntityRepositoryFactory(IServiceProvider kernel, AssemblyPool assemblyPool)
        {
            this.kernel = kernel;
            this.assemblyPool = assemblyPool;
            typeMap = new Dictionary<Type, Type>();
            PopulateTypeMap();
        }

        private void PopulateTypeMap()
        {
            var repositoryTypes = assemblyPool.GetTypes().Where(x =>
            x.IsInterface &&
            typeof(IEntityRepository).IsAssignableFrom(x) &&
            x != typeof(IEntityRepository) &&
            !x.IsGenericType);

            foreach (var repositoryType in repositoryTypes)
            {
                var genericType = repositoryType.GetInterface("IEntityRepository`1");
                if (genericType == null)
                    continue;
                var componentType = genericType.GetGenericArguments()[0];
                typeMap.Add(componentType, repositoryType);
            }
        }

        public Dictionary<Type, IEntityRepository> CreateRepositories()
        {
            var dict = new Dictionary<Type, IEntityRepository>();
            foreach (var pair in typeMap)
            {
                var repository = kernel.Resolve(pair.Value) as IEntityRepository;
                dict.Add(pair.Key, repository);
            }
            return dict;
        }
    }
}
