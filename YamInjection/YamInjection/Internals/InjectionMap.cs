using System;
using System.Collections.Generic;
using System.Reflection;

namespace YamInjection.Internals
{
    public abstract class InjectionMap : IInjectionMap
    {
        private readonly Dictionary<Type, IEnumerable<MappingBase>> _mappings;

        protected InjectionMap()
        {
            _mappings = new Dictionary<Type, IEnumerable<MappingBase>>();

            BeginRegistration();
        }

        private void BeginRegistration() => Register();

        public abstract void Register();

        internal IReadOnlyDictionary<Type, IEnumerable<MappingBase>> Mappings => _mappings;

        protected Assembly MyAssembly => Assembly.GetAssembly(GetType());

        public IInjectionMap Map(IInjectionMap injectionMap)
        {
            MapRegistrar.MergeMapInto(injectionMap, this);

            return this;
        }

        public IMapTo<TConcrete> Map<TConcrete>() where TConcrete : class => new ConcreteMap<TConcrete>(this);

        public IResolutionEvent MapAssembly(Assembly assemblyToScan)
        {
            var concreteAndInterfacePairs = AssemblyScanner.GetAllTypes(assemblyToScan);

            return new AssemblyMappedResolutionEvent(this, concreteAndInterfacePairs);
        }
    }
}