using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataModel.Reflection
{
    public sealed class AssemblyPool
    {
        private HashSet<Assembly> assemblies;
        private List<Type> types;

        public AssemblyPool()
        {
            assemblies = new HashSet<Assembly>();
            types = new List<Type>();
        }

        public void LoadAssemblies(AssemblyPool assemblyPool)
        {
            LoadAssemblies(assemblyPool.assemblies.ToArray());
        }

        public void LoadAssemblies(params Assembly[] assemblies)
        {
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (this.assemblies.Contains(assembly))
                    continue;
                this.assemblies.Add(assembly);
                LoadTypes(assembly);
            }
        }

        public IReadOnlyList<Type> GetTypes()
        {
            return types;
        }

        private void LoadTypes(Assembly assembly)
        {
            if (assembly == null)
                return;
            types.AddRange(assembly.GetTypes());
        }
    }
}
