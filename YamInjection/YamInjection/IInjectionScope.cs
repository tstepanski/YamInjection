using System;
using System.Collections.Generic;

namespace YamInjection
{
    public interface IInjectionScope : IDisposable
    {
        IInjectionScope BeginNewInjectionScope();

        void UseMap(IInjectionMap injectionMap);
        IInjectionMap GetMap();

        T Resolve<T>();
        T Resolve<T>(params IInjectionParameter[] parameters);
        IEnumerable<T> ResolveAll<T>();
        IEnumerable<T> ResolveAll<T>(params IInjectionParameter[] parameters);

        bool TryResolve<T>(out T resolvedValue);
        bool TryResolve<T>(out T resolvedValue, params IInjectionParameter[] parameters);

        bool IsDisposed { get; }
    }
}