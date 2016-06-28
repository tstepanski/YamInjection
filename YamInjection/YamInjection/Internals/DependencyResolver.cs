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

            return typesToResolve
                .Select(mapping => ResolveInstanceFromMappingFromCache(injectionScope, injectionMap, parameters, mapping));
        }

        private static object ResolveInstanceFromMappingFromCache(IInjectionScope injectionScope, IInjectionMap injectionMap,
            IInjectionParameter[] parameters, MappingBase mapping)
        {
            if (mapping.Resolver.GetIsAlreadyResolved(injectionScope))
            {
                return mapping.Resolver.GetInstance(injectionScope);
            }

            var newInstance = ResolveNewInstanceFromMapping(injectionScope, injectionMap, parameters, mapping);

            mapping.Resolver.SetInstance(injectionScope, newInstance);

            return newInstance;
        }

        private static object ResolveNewInstanceFromMapping(IInjectionScope injectionScope,
            IInjectionMap injectionMap, IInjectionParameter[] parameters, MappingBase mapping)
        {
            var concreteMapping = mapping as IConcreteTypeMapping;

            if (concreteMapping != null)
            {
                return ResolveConcreteType(injectionScope, injectionMap, parameters, concreteMapping);
            }

            var factorizedMapping = mapping as IFactorizedMapping;

            if (factorizedMapping != null)
            {
                return factorizedMapping.Factory(injectionScope);
            }

            throw new NotSupportedException("Unsupported MappingBase type");
        }

        private static object ResolveConcreteType(IInjectionScope injectionScope, IInjectionMap injectionMap,
            IInjectionParameter[] parameters, IConcreteTypeMapping concreteTypeMapping)
        {
            var concreteType = concreteTypeMapping.MappedConcreteType;

            var constructor = GetFirstNonPrivateConstructorForType(concreteType);

            if (constructor == null)
            {
                throw new NoPublicConstructorException(concreteType);
            }

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