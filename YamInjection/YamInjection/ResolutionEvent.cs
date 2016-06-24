using System;

namespace YamInjection
{
    internal sealed class ResolutionEvent : ResolutionEventBase
    {
        private readonly Type _concreteType;
        private readonly Type _interfaceType;

        internal ResolutionEvent(InjectionMap map, Type concreteType, Type interfaceType) : base(map)
        {
            _concreteType = concreteType;
            _interfaceType = interfaceType;
        }

        protected override void CompleteMapping(ResolutionEventEnum resolutionEventEnum)
        {
            Map.RegisterMapping(_concreteType, _interfaceType, resolutionEventEnum);
        }
    }
}