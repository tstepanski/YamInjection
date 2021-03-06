﻿using System;
using System.Collections.Generic;

namespace YamInjection
{
    public interface IInjectionScope : IDisposable, IEquatable<IInjectionScope>
    {
        bool IsDisposed { get; }
        IInjectionScope BeginNewInjectionScope();

        void UseMap(IInjectionMap injectionMap);
        IInjectionMap GetMap();

        T Resolve<T>();
        T Resolve<T>(params IInjectionParameter[] parameters);
        IEnumerable<T> ResolveAll<T>();
        IEnumerable<T> ResolveAll<T>(params IInjectionParameter[] parameters);

        object Resolve(Type type);
        object Resolve(Type type, params IInjectionParameter[] parameters);
        IEnumerable<object> ResolveAll(Type type);
        IEnumerable<object> ResolveAll(Type type, params IInjectionParameter[] parameters);

        bool TryResolve<T>(out T resolvedValue);
        bool TryResolve<T>(out T resolvedValue, params IInjectionParameter[] parameters);

        bool TryResolve(Type type, out object resolvedValue);
        bool TryResolve(Type type, out object resolvedValue, params IInjectionParameter[] parameters);
    }
}