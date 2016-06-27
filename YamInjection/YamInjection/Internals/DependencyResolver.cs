using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamInjection.Exceptions;

namespace YamInjection.Internals
{
    internal static class DependencyResolver
    {
        public static IEnumerable<object> ResolveAll(IInjectionScope injectionScope,
            IInjectionMap injectionMap, Type interfaceType, params IInjectionParameter[] parameters)
        {
            var injectionMapCasted = (InjectionMap) injectionMap;

            var typesToResolve = injectionMapCasted.Mappings[interfaceType];

            foreach (var mapping in typesToResolve)
            {
                var concreteMapping = mapping as ConcreteTypeMapping;

                if (concreteMapping != null)
                {
                    yield return ResolveConcreteType(injectionScope, injectionMap, parameters, concreteMapping);
                }

                var factorizedMapping = mapping as FactorizedMapping;

                if (factorizedMapping != null)
                {
                    yield return factorizedMapping.Factory(injectionScope);
                }
            }
        }

        private static object ResolveConcreteType(IInjectionScope injectionScope, IInjectionMap injectionMap,
            IInjectionParameter[] parameters, ConcreteTypeMapping concreteTypeMapping)
        {
            var concreteType = concreteTypeMapping.MappedConcreteType;

            var constructor = GetFirstNonPrivateConstructorForType(concreteType);

            var constructorParameters = constructor.GetParameters();

            var parameterResolutions = GetResolvedParametersInstances(injectionScope, injectionMap, parameters,
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

        private static IEnumerable<object> GetResolvedParametersInstances(IInjectionScope injectionScope,
            IInjectionMap injectionMap, IInjectionParameter[] parameters,
            IEnumerable<ParameterInfo> constructorParameters, Type concreteType)
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

                var instanceToUseForParameter = GetMappedInstanceToUseForDependencyParameter(injectionScope,
                    injectionMap, parameters, concreteType, parameterType);

                yield return instanceToUseForParameter;
            }
        }

        private static object GetMappedInstanceToUseForDependencyParameter(IInjectionScope injectionScope,
            IInjectionMap injectionMap, IInjectionParameter[] parameters, Type concreteType, Type parameterType)
        {
            var injectionMapCasted = (InjectionMap) injectionMap;

            if (!injectionMapCasted.Mappings.ContainsKey(parameterType))
            {
                throw new ParameterTypeNotDefinedException(parameterType, concreteType);
            }

            var instanceToUseForParameter = ResolveAll(injectionScope, injectionMap, parameterType, parameters).First();

            return instanceToUseForParameter;
        }
    }
}