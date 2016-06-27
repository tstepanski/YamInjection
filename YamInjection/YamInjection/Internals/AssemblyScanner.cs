using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YamInjection.Internals
{
    internal sealed class AssemblyScanner
    {
        public static IEnumerable<ConcreteAndInterfacePair> GetAllTypes(Assembly assemblyToScan)
        {
            var allTypePairs = assemblyToScan
                .GetTypes()
                .Where(IsTypeInstantiatable)
                .SelectMany(GetTypeAndInterfaceTypePairsForType);

            return allTypePairs;
        }

        private static bool IsTypeInstantiatable(Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        private static IEnumerable<ConcreteAndInterfacePair> GetTypeAndInterfaceTypePairsForType(Type type)
        {
            var allInterfaces = GetAllInterfacesAndBaseClasses(type);

            return allInterfaces.Select(interfaceType => new ConcreteAndInterfacePair(type, interfaceType));
        }

        private static IEnumerable<Type> GetAllInterfacesAndBaseClasses(Type type)
        {
            var thisTypesInterfaces = type.GetInterfaces();

            var baseClass = type.BaseType;

            IEnumerable<Type> baseClassAndItsInterfaces = new List<Type>();

            if (baseClass != null)
            {
                ((List<Type>) baseClassAndItsInterfaces).Add(baseClass);

                var baseClassInterfaces = GetAllInterfacesAndBaseClasses(baseClass);

                baseClassAndItsInterfaces = baseClassAndItsInterfaces.Union(baseClassInterfaces);
            }

            var thisTypesInterfacesInterfaces = thisTypesInterfaces.SelectMany(GetAllInterfacesAndBaseClasses);

            return thisTypesInterfaces
                .Union(thisTypesInterfacesInterfaces)
                .Union(baseClassAndItsInterfaces)
                .Distinct();
        }
    }
}