using System;
using System.Collections.Generic;
using System.Linq;
using YamInjection.Internals;

namespace YamInjection
{
    internal sealed class InjectionScope : IInjectionScope
    {
        private readonly ICollection<IInjectionScope> _childrenScopes;
        private readonly Guid _scopeGuid;
        private IInjectionMap _injectionMap;

        internal InjectionScope()
        {
            IsDisposed = false;

            _childrenScopes = new HashSet<IInjectionScope>();

            _injectionMap = new InjectionMapSeed();

            _scopeGuid = Guid.NewGuid();
        }

        public void Dispose()
        {
            var undisposedChildrenScopes = GetAllUndisposedChildrenScopes();

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
            => ResolveAll(typeof(T), parameters).Cast<T>();

        public object Resolve(Type type) => Resolve(type, new IInjectionParameter[0]);

        public object Resolve(Type type, params IInjectionParameter[] parameters)
            => ResolveAll(type, parameters).First();

        public IEnumerable<object> ResolveAll(Type type) => ResolveAll(type, new IInjectionParameter[0]);

        public IEnumerable<object> ResolveAll(Type type, params IInjectionParameter[] parameters)
            => DependencyResolver.ResolveAll(this, type, parameters);

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

        public bool TryResolve(Type type, out object resolvedValue)
            => TryResolve(type, out resolvedValue, new IInjectionParameter[0]);

        public bool TryResolve(Type type, out object resolvedValue, params IInjectionParameter[] parameters)
        {
            try
            {
                resolvedValue = Resolve(type, parameters);

                return true;
            }
            catch (Exception)
            {
                resolvedValue = type.IsValueType ? Activator.CreateInstance(type) : null;

                return false;
            }
        }

        public bool IsDisposed { get; }

        public bool Equals(IInjectionScope other)
        {
            var otherAsInjectionScope = other as InjectionScope;

            return otherAsInjectionScope != null && _scopeGuid.Equals(otherAsInjectionScope._scopeGuid);
        }

        private IEnumerable<IInjectionScope> GetAllUndisposedChildrenScopes()
            => _childrenScopes.Where(scope => !scope.IsDisposed);
    }
}