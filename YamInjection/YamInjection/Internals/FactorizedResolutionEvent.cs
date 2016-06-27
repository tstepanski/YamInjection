using System;

namespace YamInjection.Internals
{
    internal sealed class FactorizedResolutionEvent<TConcrete> : ResolutionEventBase
    {
        private readonly Func<IInjectionScope, TConcrete> _factory;

        internal FactorizedResolutionEvent(InjectionMap map, Func<IInjectionScope, TConcrete> factory) : base(map)
        {
            _factory = factory;
        }

        protected override void CompleteMapping(ResolutionEventEnum resolutionEventEnum)
        {
            var type = typeof(TConcrete);

            MapRegistrar.RegisterFactory(Map, type, _factory, resolutionEventEnum);
        }
    }
}