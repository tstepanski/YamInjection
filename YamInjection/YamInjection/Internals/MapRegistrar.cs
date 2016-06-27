﻿using System;
using System.Collections.Generic;

namespace YamInjection.Internals
{
    internal static class MapRegistrar
    {
        internal static void MergeMapInto(IInjectionMap source, IInjectionMap target)
        {
            var injectionMapCasted = (InjectionMap) source;

            foreach (var mapping in injectionMapCasted.Mappings)
            {
                var interfaceType = mapping.Key;

                foreach (var typeAndResolutionProtocol in mapping.Value)
                {
                    AddMappingForType(injectionMapCasted, interfaceType, typeAndResolutionProtocol);
                }
            }
        }

        internal static void RegisterMapping<TConcrete, TInterface>(IInjectionMap injectionMap,
            ResolutionEventEnum resolutionEventEnum) where TConcrete : class where TInterface : class
        {
            var concreteType = typeof(TConcrete);
            var interfaceType = typeof(TInterface);

            RegisterMapping(injectionMap, concreteType, interfaceType, resolutionEventEnum);
        }

        internal static void RegisterAllMappings(IInjectionMap injectionMap,
            IEnumerable<ConcreteAndInterfacePair> concreteAndInterfacePairs,
            ResolutionEventEnum resolutionEventEnum)
        {
            foreach (var concreteAndInterfacePair in concreteAndInterfacePairs)
            {
                var concreteType = concreteAndInterfacePair.ConcreteType;
                var interfaceType = concreteAndInterfacePair.InterfaceType;

                RegisterMapping(injectionMap, concreteType, interfaceType, resolutionEventEnum);
            }
        }

        internal static void RegisterMapping(IInjectionMap injectionMap, Type concreteType, Type interfaceType,
            ResolutionEventEnum resolutionEventEnum)
        {
            var typeAndResolutionProtocol = new ConcreteTypeMapping(concreteType, resolutionEventEnum);

            AddMappingForType(injectionMap, interfaceType, typeAndResolutionProtocol);
        }

        private static void AddMappingForType(IInjectionMap injectionMap, Type interfaceType,
            MappingBase typeAndResolutionProtocol)
        {
            var mapCasted = (InjectionMap) injectionMap;

            var mappings = (Dictionary<Type, IEnumerable<MappingBase>>) mapCasted.Mappings;

            if (!mappings.ContainsKey(interfaceType))
            {
                mappings.Add(interfaceType, new HashSet<MappingBase>());
            }

            var mappingsHashSet = (HashSet<MappingBase>) mappings[interfaceType];

            mappingsHashSet.Add(typeAndResolutionProtocol);
        }

        internal static void RegisterFactory<TConcrete>(IInjectionMap injectionMap, Type interfaceType,
            Func<IInjectionScope, TConcrete> factory, ResolutionEventEnum resolutionEventEnum)
        {
            Func<IInjectionScope, object> wrappedFunction = scope => factory(scope);

            var mapping = new FactorizedMapping(wrappedFunction, resolutionEventEnum);

            AddMappingForType(injectionMap, interfaceType, mapping);
        }
    }
}