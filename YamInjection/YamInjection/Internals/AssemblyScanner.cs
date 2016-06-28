using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YamInjection.Internals
{
    internal sealed class AssemblyScanner
    {
        internal static IEnumerable<ConcreteAndInterfacePair> GetAllTypes(Assembly assemblyToScan)
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
            var allInterfaces = GetAllInheritedTypes(type);

            return allInterfaces.Select(interfaceType => new ConcreteAndInterfacePair(type, interfaceType));
        }

        private static IEnumerable<Type> GetAllInheritedTypes(Type type)
        {
            var thisTypesInterfaces = type.GetInterfaces();

            var baseClassAndItsInterfaces = GetBaseClassAndItsInheritedTypes(type);

            var thisTypesInterfacesInterfaces = thisTypesInterfaces.SelectMany(GetAllInheritedTypes);

            return thisTypesInterfaces
                .Union(thisTypesInterfacesInterfaces)
                .Union(baseClassAndItsInterfaces)
                .Distinct();
        }

        private static IEnumerable<Type> GetBaseClassAndItsInheritedTypes(Type type)
        {
            var baseClass = type.BaseType;
            
            if (baseClass == null)
            {
                return new Type[0];
            }
            
            var baseClassInheritance = GetAllInheritedTypes(baseClass);

            var baseClassInArray = new []
            {
                baseClass
            };

            var baseClassAndItsInheritance = baseClassInArray.Union(baseClassInheritance);

            return baseClassAndItsInheritance;
        }
    }
}