using System;

namespace YamInjection.Internals
{
    internal abstract class MappingBase
    {
        internal Resolver Resolver { get; }

        protected internal MappingBase(ResolutionEventEnum resolutionEvent, Type typeToResolve)
        {
            Resolver = ResolverFactory.GetResolver(resolutionEvent, typeToResolve);
        }

        internal ResolutionEventEnum ResolutionEvent => Resolver.ResolutionEvent;
    }
}