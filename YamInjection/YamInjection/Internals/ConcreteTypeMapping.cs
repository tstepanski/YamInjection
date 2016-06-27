using System;

namespace YamInjection.Internals
{
    internal abstract class MappingBase
    {
        protected internal MappingBase(ResolutionEventEnum resolutionEvent)
        {
            ResolutionEvent = resolutionEvent;
        }

        public ResolutionEventEnum ResolutionEvent { get; }
    }

    internal sealed class ConcreteTypeMapping : MappingBase
    {
        internal ConcreteTypeMapping(Type mappedConcreteType, ResolutionEventEnum resolutionEvent)
            : base(resolutionEvent)
        {
            MappedConcreteType = mappedConcreteType;
        }

        public Type MappedConcreteType { get; }
    }
}