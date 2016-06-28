using System;

namespace YamInjection.Internals
{
    internal interface IConcreteTypeMapping
    {
        Type MappedConcreteType { get; }
    }

    internal sealed class ConcreteTypeMapping : MappingBase, IConcreteTypeMapping
    {
        internal ConcreteTypeMapping(Type mappedConcreteType, ResolutionEventEnum resolutionEvent)
            : base(resolutionEvent, mappedConcreteType)
        {
            MappedConcreteType = mappedConcreteType;
        }

        public Type MappedConcreteType { get; }
    }
}