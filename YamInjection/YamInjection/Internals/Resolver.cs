using System;

namespace YamInjection.Internals
{
    internal abstract class Resolver
    {
        protected readonly Type TypeToResolveFor;

        protected Resolver(Type typeToResolveFor)
        {
            TypeToResolveFor = typeToResolveFor;
        }

        internal abstract ResolutionEventEnum ResolutionEvent { get; }
        internal abstract object GetInstance(IInjectionScope scope);
        internal abstract void SetInstance(IInjectionScope scope, object instance);
        internal abstract bool GetIsAlreadyResolved(IInjectionScope scope);

        protected virtual void ThrowIfNotYetRegistered(IInjectionScope scope)
        {
            if (!GetIsAlreadyResolved(scope))
            {
                throw new InstanceNotYetResolvedException(TypeToResolveFor);
            }
        }
    }
}