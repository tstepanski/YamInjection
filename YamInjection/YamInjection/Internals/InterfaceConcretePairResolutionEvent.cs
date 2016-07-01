using System;
using System.Collections.Generic;

namespace YamInjection.Internals
{
    internal sealed class MultiInterfaceConcretePairResolutionEvent : ResolutionEventBase
    {
        private readonly IEnumerable<Type> _concreteTypes;
        private readonly IEnumerable<Type> _interfaceTypes;

        internal MultiInterfaceConcretePairResolutionEvent(InjectionMap map, 
            IEnumerable<Type> concreteType, IEnumerable<Type> interfaceTypes) : base(map)
        {
            _concreteTypes = concreteType;
            _interfaceTypes = interfaceTypes;
        }

        protected override void CompleteMapping(ResolutionEventEnum resolutionEventEnum)
        {
            foreach (var concreteType in _concreteTypes)
            {
                RegisterMappingTypeAndAllInterfaces(resolutionEventEnum, concreteType);
            }
        }

        private void RegisterMappingTypeAndAllInterfaces(ResolutionEventEnum resolutionEventEnum, Type concreteType)
        {
            foreach (var interfaceType in _interfaceTypes)
            {
                MapRegistrar.RegisterMapping(Map, concreteType, interfaceType, resolutionEventEnum);
            }
        }
    }
}