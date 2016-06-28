using System;

namespace YamInjection.Internals
{
    internal sealed class SingleInstanceResolver : Resolver
    {
        private object _instance;
        private bool _isResolved;

        internal SingleInstanceResolver(Type typeToResolveFor) : base(typeToResolveFor)
        {
            _isResolved = false;
        }

        internal override ResolutionEventEnum ResolutionEvent => ResolutionEventEnum.OneInstance;
        internal override object GetInstance(IInjectionScope scope)
        {
            ThrowIfNotYetRegistered(scope);

            return _instance;
        }

        internal override bool GetIsAlreadyResolved(IInjectionScope scope) => _isResolved;

        internal override void SetInstance(IInjectionScope scope, object instance)
        {
            _instance = instance;
            _isResolved = true;
        }
    }
}