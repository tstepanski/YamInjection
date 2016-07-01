using System;
using System.Collections.Generic;
using System.Reflection;

namespace YamInjection
{
    public interface IInjectionMap
    {
        IInjectionMap Map(IInjectionMap injectionMap);
        IMapTo<TConcrete> Map<TConcrete>() where TConcrete : class;
        IMapTo MapAssembly(Assembly assemblyToScan);
        IMapTo Map(Type concreteType);
        IMapTo Map(IEnumerable<Type> concreteTypes);
        void Register();
    }
}