using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YamInjection.Internals
{
    internal sealed class ScopedInstanceResolver : Resolver
    {
        private readonly IDictionary<IInjectionScope, object> _instancesPerScope;

        public ScopedInstanceResolver(Type typeToResolveFor) : base(typeToResolveFor)
        {
            _instancesPerScope = new ConcurrentDictionary<IInjectionScope, object>();
        }

        internal override ResolutionEventEnum ResolutionEvent => ResolutionEventEnum.OncePerScope;

        internal override object GetInstance(IInjectionScope scope)
        {
            ThrowIfNotYetRegistered(scope);

            return _instancesPerScope[scope];
        }

        internal override void SetInstance(IInjectionScope scope, object instance)
        {
            if (!GetIsAlreadyResolved(scope))
            {
                _instancesPerScope.Add(scope, instance);
            }
            else
            {
                _instancesPerScope[scope] = instance;
            }
        }

        internal override bool GetIsAlreadyResolved(IInjectionScope scope) => _instancesPerScope.ContainsKey(scope);
    }
}