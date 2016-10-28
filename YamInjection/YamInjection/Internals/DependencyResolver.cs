using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamInjection.Exceptions;

namespace YamInjection.Internals
{
    internal static class DependencyResolver
    {
        public static IEnumerable<object> ResolveAll(IInjectionScope injectionScope, Type interfaceType,
            params IInjectionParameter[] parameters)
        {
            var injectionMapCasted = (InjectionMap) injectionScope.GetMap();

            var typesToResolve = injectionMapCasted.Mappings[interfaceType];

            return typesToResolve
                .Select(mapping => ResolveInstanceFromMappingFromCache(injectionScope, parameters, mapping));
        }

        private static object ResolveInstanceFromMappingFromCache(IInjectionScope injectionScope,
            IInjectionParameter[] parameters, MappingBase mapping)
        {
            if (mapping.Resolver.GetIsAlreadyResolved(injectionScope))
            {
                return mapping.Resolver.GetInstance(injectionScope);
            }

            var newInstance = ResolveNewInstanceFromMapping(injectionScope, parameters, mapping);

            mapping.Resolver.SetInstance(injectionScope, newInstance);

            return newInstance;
        }

        private static object ResolveNewInstanceFromMapping(IInjectionScope injectionScope,
            IInjectionParameter[] parameters, MappingBase mapping)
        {
            var concreteMapping = mapping as IConcreteTypeMapping;

            if (concreteMapping != null)
            {
                return ResolveConcreteType(injectionScope, parameters, concreteMapping);
            }

            var factorizedMapping = mapping as IFactorizedMapping;

            if (factorizedMapping != null)
            {
                return factorizedMapping.Factory(injectionScope);
            }

            throw new NotSupportedException("Unsupported MappingBase type");
        }

        private static object ResolveConcreteType(IInjectionScope injectionScope, IInjectionParameter[] parameters,
            IConcreteTypeMapping concreteTypeMapping)
        {
            var concreteType = concreteTypeMapping.MappedConcreteType;

            return CreateInstanceForType(concreteType, injectionScope, parameters);
        }

        public static object CreateInstanceForType(Type concreteType, IInjectionScope injectionScope,
            params IInjectionParameter[] parameters)
        {
            var constructor = GetFirstNonPrivateConstructorForType(concreteType);

            if (constructor == null)
            {
                throw new NoPublicConstructorException(concreteType);
            }

            var constructorParameters = constructor.GetParameters();

            var parameterResolutions = GetResolvedParametersInstances(injectionScope, parameters,
                constructorParameters, concreteType);

            var instance = constructor.Invoke(parameterResolutions.ToArray());

            var instancesToApplyToPropertyInjectedProperties = GetObjectsToPropertyInject(injectionScope, concreteType);

            instance = ApplyInstancesToPropertyInjectedPropertiesOnObject(instance,
                instancesToApplyToPropertyInjectedProperties);

            return instance;
        }

        private static object ApplyInstancesToPropertyInjectedPropertiesOnObject(object instance,
            IReadOnlyDictionary<PropertyInfo, object> instancesToApply)
        {
            foreach (var propertyInfoAndInstance in instancesToApply)
            {
                var propertyInfo = propertyInfoAndInstance.Key;
                var instanceToApply = propertyInfoAndInstance.Value;

                propertyInfo.SetValue(instance, instanceToApply);
            }

            return instance;
        }

        private static IReadOnlyDictionary<PropertyInfo, object> GetObjectsToPropertyInject(
            IInjectionScope injectionScope, Type concreteType)
        {
            var dictionary = new Dictionary<PropertyInfo, object>();

            var propertiesNeedingInjection = concreteType.GetProperties().WhereHasDependencyInjectAttribute();

            foreach (var propertyInfo in propertiesNeedingInjection)
            {
                var type = propertyInfo.PropertyType;

                var instance = ResolveAll(injectionScope, type).First();

                dictionary.Add(propertyInfo, instance);
            }

            return dictionary;
        }

        private static ConstructorInfo GetFirstNonPrivateConstructorForType(Type concreteType)
        {
            var constructor = concreteType
                .GetConstructors()
                .FirstOrDefault(info => !info.IsPrivate);

            return constructor;
        }

        private static IEnumerable<object> GetResolvedParametersInstances(IInjectionScope injectionScope,
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

                var instanceToUseForParameter = GetMappedInstanceToUseForDependencyParameter(injectionScope, parameters,
                    concreteType, parameterType);

                yield return instanceToUseForParameter;
            }
        }

        private static object GetMappedInstanceToUseForDependencyParameter(IInjectionScope injectionScope,
            IInjectionParameter[] parameters, Type concreteType, Type parameterType)
        {
            var injectionMapCasted = (InjectionMap) injectionScope.GetMap();

            if (!injectionMapCasted.Mappings.ContainsKey(parameterType))
            {
                throw new ParameterTypeNotDefinedException(parameterType, concreteType);
            }

            var instanceToUseForParameter = ResolveAll(injectionScope, parameterType, parameters).First();

            return instanceToUseForParameter;
        }
    }
}