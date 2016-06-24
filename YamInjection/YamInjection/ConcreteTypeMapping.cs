using System;

namespace YamInjection
{
    public class ConcreteTypeMapping
    {
        internal ConcreteTypeMapping(Type mappedConcreteType, ResolutionEventEnum resolutionEvent)
        {
            MappedConcreteType = mappedConcreteType;
            ResolutionEvent = resolutionEvent;
        }

        public Type MappedConcreteType { get; }
        internal ResolutionEventEnum ResolutionEvent { get; }
    }
}