using System;

namespace YamInjection.Internals
{
    internal sealed class ConcreteMap<TConcrete> : IMapTo<TConcrete>
    {
        private readonly Type _concreteType;
        private readonly InjectionMap _injectionMap;

        internal ConcreteMap(InjectionMap injectionMap)
        {
            _injectionMap = injectionMap;
            _concreteType = typeof(TConcrete);
        }

        public IResolutionEvent To<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            return new InterfaceConcretePairResolutionEvent(_injectionMap, _concreteType, interfaceType);
        }

        public IResolutionEvent AsSelf()
            => new InterfaceConcretePairResolutionEvent(_injectionMap, _concreteType, _concreteType);

        public IResolutionEvent Using(Func<TConcrete> factory)
        {
            Func<IInjectionScope, TConcrete> factoryWrapped = scope => factory();

            return Using(factoryWrapped);
        }

        public IResolutionEvent Using(Func<IInjectionScope, TConcrete> factory)
            => new FactorizedResolutionEvent<TConcrete>(_injectionMap, factory);
    }
}