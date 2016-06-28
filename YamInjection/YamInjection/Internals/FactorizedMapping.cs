using System;

namespace YamInjection.Internals
{
    internal interface IFactorizedMapping
    {
        Func<IInjectionScope, object> Factory { get; }
    }

    internal sealed class FactorizedMapping<TConcrete> : MappingBase, IFactorizedMapping
    {
        internal FactorizedMapping(Func<IInjectionScope, TConcrete> factory, ResolutionEventEnum resolutionEvent)
            : base(resolutionEvent, typeof(TConcrete))
        {
            Factory = scope => factory(scope);
        }

        public Func<IInjectionScope, object> Factory { get; }
    }
}