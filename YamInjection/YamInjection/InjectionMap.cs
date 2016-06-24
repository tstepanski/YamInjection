using System;
using System.Collections.Generic;
using System.Reflection;

namespace YamInjection
{
    public interface IInjectionMap
    {
        IInjectionMap Map(IInjectionMap injectionMap);
        IMapTo Map<TConcrete>() where TConcrete : class;
        IResolutionEvent MapAssembly(Assembly assemblyToScan);
        IReadOnlyDictionary<Type, IEnumerable<ConcreteTypeMapping>> Mappings { get; }
    }

    internal sealed class InjectionMap : IInjectionMap
    {
        private readonly Dictionary<Type, IEnumerable<ConcreteTypeMapping>> _mappings;

        private InjectionMap()
        {
            _mappings = new Dictionary<Type, IEnumerable<ConcreteTypeMapping>>();
        }

        public IReadOnlyDictionary<Type, IEnumerable<ConcreteTypeMapping>> Mappings => _mappings;

        public IInjectionMap Map(IInjectionMap injectionMap)
        {
            foreach (var mapping in injectionMap.Mappings)
            {
                var interfaceType = mapping.Key;

                foreach (var typeAndResolutionProtocol in mapping.Value)
                {
                    var concreteType = typeAndResolutionProtocol.MappedConcreteType;
                    var resolutionEventEnum = typeAndResolutionProtocol.ResolutionEvent;

                    RegisterMapping(concreteType, interfaceType, resolutionEventEnum);
                }
            }

            return this;
        }

        public IMapTo Map<TConcrete>() where TConcrete : class
        {
            var concreteType = typeof(TConcrete);

            return new ConcreteMap(this, concreteType);
        }

        internal static InjectionMap NewMap()
        {
            return new InjectionMap();
        }

        public IResolutionEvent MapAssembly(Assembly assemblyToScan)
        {
            var concreteAndInterfacePairs = AssemblyScanner.GetAllTypes(assemblyToScan);

            return new AssemblyMappedResolutionEvent(this, concreteAndInterfacePairs);
        }

        internal void RegisterMapping<TConcrete, TInterface>(ResolutionEventEnum resolutionEventEnum)
            where TConcrete : class where TInterface : class
        {
            var concreteType = typeof(TConcrete);
            var interfaceType = typeof(TInterface);

            RegisterMapping(concreteType, interfaceType, resolutionEventEnum);
        }

        internal void RegisterAllMappings(
            IEnumerable<ConcreteAndInterfacePair> concreteAndInterfacePairs,
            ResolutionEventEnum resolutionEventEnum)
        {
            foreach (var concreteAndInterfacePair in concreteAndInterfacePairs)
            {
                var concreteType = concreteAndInterfacePair.ConcreteType;
                var interfaceType = concreteAndInterfacePair.InterfaceType;

                RegisterMapping(concreteType, interfaceType, resolutionEventEnum);
            }
        }

        internal void RegisterMapping(Type concreteType, Type interfaceType, ResolutionEventEnum resolutionEventEnum)
        {
            if (!_mappings.ContainsKey(interfaceType))
            {
                _mappings.Add(interfaceType, new HashSet<ConcreteTypeMapping>());
            }

            var currentlyMappedConcreteTypes = (HashSet<ConcreteTypeMapping>) _mappings[interfaceType];

            var typeAndResolutionProtocol = new ConcreteTypeMapping(concreteType, resolutionEventEnum);

            currentlyMappedConcreteTypes.Add(typeAndResolutionProtocol);
        }
    }
}