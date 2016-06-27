using System;

namespace YamInjection.Internals
{
    internal sealed class FactorizedMapping : MappingBase
    {
        internal FactorizedMapping(Func<IInjectionScope, object> factory, ResolutionEventEnum resolutionEvent)
            : base(resolutionEvent)
        {
            Factory = factory;
        }

        internal Func<IInjectionScope, object> Factory { get; }
    }
}