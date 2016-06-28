using System;

namespace YamInjection.Internals
{
    internal sealed class PerRequestInstanceResolver : Resolver
    {
        internal PerRequestInstanceResolver(Type typeToResolveFor) : base(typeToResolveFor)
        {
        }

        internal override ResolutionEventEnum ResolutionEvent => ResolutionEventEnum.EveryRequest;

        internal override object GetInstance(IInjectionScope scope)
        {
            throw new InstanceNotYetResolvedException(TypeToResolveFor);
        }

        internal override void SetInstance(IInjectionScope scope, object instance)
        {
        }

        internal override bool GetIsAlreadyResolved(IInjectionScope scope) => false;
    }
}