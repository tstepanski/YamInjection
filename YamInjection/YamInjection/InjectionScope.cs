using System;
using System.Collections.Generic;
using System.Linq;

namespace YamInjection
{
    internal sealed class InjectionScope : IInjectionScope
    {
        private readonly ICollection<IInjectionScope> _childrenScopes;
        private IInjectionMap _injectionMap;

        internal InjectionScope()
        {
            IsDisposed = false;

            _childrenScopes = new HashSet<IInjectionScope>();

            _injectionMap = InjectionMap.NewMap();
        }

        public void Dispose()
        {
            var undisposedChildrenScopes = _childrenScopes.Where(scope => !scope.IsDisposed);

            foreach (var scope in undisposedChildrenScopes)
            {
                scope.Dispose();
            }
        }

        public IInjectionScope BeginNewInjectionScope()
        {
            var newScope = new InjectionScope();

            _childrenScopes.Add(newScope);

            return newScope;
        }

        public void UseMap(IInjectionMap injectionMap)
        {
            _injectionMap = injectionMap;
        }

        public IInjectionMap GetMap() => _injectionMap;

        public T Resolve<T>() => Resolve<T>(new IInjectionParameter[0]);

        public T Resolve<T>(params IInjectionParameter[] parameters) => ResolveAll<T>(parameters).First();

        public IEnumerable<T> ResolveAll<T>() => ResolveAll<T>(new IInjectionParameter[0]);

        public IEnumerable<T> ResolveAll<T>(params IInjectionParameter[] parameters)
        {
            var interfaceType = typeof(T);

            return DependencyResolver.ResolveAll(_injectionMap, interfaceType, parameters).Cast<T>();
        }

        public bool TryResolve<T>(out T resolvedValue) => TryResolve(out resolvedValue, new IInjectionParameter[0]);

        public bool TryResolve<T>(out T resolvedValue, params IInjectionParameter[] parameters)
        {
            try
            {
                resolvedValue = Resolve<T>(parameters);

                return true;
            }
            catch (Exception)
            {
                resolvedValue = default(T);

                return false;
            }
        }

        public bool IsDisposed { get; }
    }
}