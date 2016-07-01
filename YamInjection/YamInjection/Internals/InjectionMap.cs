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

        internal IReadOnlyDictionary<Type, IEnumerable<MappingBase>> Mappings => _mappings;

        protected Assembly MyAssembly => Assembly.GetAssembly(GetType());

        public IMapTo Map(IEnumerable<Type> concreteTypes) => new ConcreteMap(this, concreteTypes);

        public abstract void Register();

        public IInjectionMap Map(IInjectionMap injectionMap)
        {
            MapRegistrar.MergeMapInto(injectionMap, this);

            return this;
        }

        public IMapTo<TConcrete> Map<TConcrete>() where TConcrete : class => new ConcreteMap<TConcrete>(this);

        public IMapTo MapAssembly(Assembly assemblyToScan)
        {
            var allTypesToMapFrom = AssemblyScanner.GetAllInstantiatableTypesFromMethod(assemblyToScan);

            return Map(allTypesToMapFrom);
        }

        public IMapTo Map(Type concreteType) => Map(new[] {concreteType});

        private void BeginRegistration() => Register();
    }
}