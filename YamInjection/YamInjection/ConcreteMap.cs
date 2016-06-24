using System;

namespace YamInjection
{
    internal sealed class ConcreteMap : IMapTo
    {
        private readonly Type _concreteType;
        private readonly InjectionMap _injectionMap;

        internal ConcreteMap(InjectionMap injectionMap, Type concreteType)
        {
            _injectionMap = injectionMap;
            _concreteType = concreteType;
        }

        public IResolutionEvent To<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            return new ResolutionEvent(_injectionMap, _concreteType, interfaceType);
        }

        public IResolutionEvent AsSelf() => new ResolutionEvent(_injectionMap, _concreteType, _concreteType);
    }
}