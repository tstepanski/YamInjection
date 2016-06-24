using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YamInjection
{
    public static class DependencyResolver
    {
        public static IEnumerable<object> ResolveAll(IInjectionMap injectionMap, Type interfaceType,
            params IInjectionParameter[] parameters)
        {
            var typesToResolve = injectionMap.Mappings[interfaceType];

            foreach (var concreteTypeMapping in typesToResolve)
            {
                var instance = ResolveConcreteType(injectionMap, parameters, concreteTypeMapping);

                yield return instance;
            }
        }

        private static object ResolveConcreteType(IInjectionMap injectionMap, IInjectionParameter[] parameters,
            ConcreteTypeMapping concreteTypeMapping)
        {
            var concreteType = concreteTypeMapping.MappedConcreteType;

            var constructor = GetFirstNonPrivateConstructorForType(concreteType);

            var constructorParameters = constructor.GetParameters();

            var parameterResolutions = GetResolvedParametersInstances(injectionMap, parameters,
                constructorParameters, concreteType);

            var instance = constructor.Invoke(parameterResolutions.ToArray());

            return instance;
        }

        private static ConstructorInfo GetFirstNonPrivateConstructorForType(Type concreteType)
        {
            var constructor = concreteType
                .GetConstructors()
                .FirstOrDefault(info => !info.IsPrivate);

            if (constructor == null)
            {
                throw new NoPublicConstructorException(concreteType);
            }

            return constructor;
        }

        private static IEnumerable<object> GetResolvedParametersInstances(IInjectionMap injectionMap,
            IInjectionParameter[] parameters, IEnumerable<ParameterInfo> constructorParameters, Type concreteType)
        {
            foreach (var parameter in constructorParameters)
            {
                var parameterType = parameter.ParameterType;

                var userDefinedResolution = parameters
                    .FirstOrDefault(injectionParameter => injectionParameter.MatchesParameterDefinition(parameter));

                if (userDefinedResolution != null)
                {
                    yield return userDefinedResolution.Value;

                    continue;
                }

                var instanceToUseForParameter = GetMappedInstanceToUseForDependencyParameter(injectionMap, parameters,
                    concreteType, parameterType);

                yield return instanceToUseForParameter;
            }
        }

        private static object GetMappedInstanceToUseForDependencyParameter(IInjectionMap injectionMap,
            IInjectionParameter[] parameters, Type concreteType, Type parameterType)
        {
            if (!injectionMap.Mappings.ContainsKey(parameterType))
            {
                throw new ParameterTypeNotDefinedException(parameterType, concreteType);
            }

            var instanceToUseForParameter = ResolveAll(injectionMap, parameterType, parameters).First();

            return instanceToUseForParameter;
        }
    }
}