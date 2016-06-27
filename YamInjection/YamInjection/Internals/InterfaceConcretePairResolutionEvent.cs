using System;

namespace YamInjection.Internals
{
    internal sealed class InterfaceConcretePairResolutionEvent : ResolutionEventBase
    {
        private readonly Type _concreteType;
        private readonly Type _interfaceType;

        internal InterfaceConcretePairResolutionEvent(InjectionMap map, Type concreteType, Type interfaceType)
            : base(map)
        {
            _concreteType = concreteType;
            _interfaceType = interfaceType;
        }

        protected override void CompleteMapping(ResolutionEventEnum resolutionEventEnum)
        {
            MapRegistrar.RegisterMapping(Map, _concreteType, _interfaceType, resolutionEventEnum);
        }
    }
}