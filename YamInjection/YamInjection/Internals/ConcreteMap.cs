using System;
using System.Collections.Generic;

namespace YamInjection.Internals
{
    internal class ConcreteMap : IMapTo
    {
        protected readonly IEnumerable<Type> ConcreteTypes;
        protected readonly InjectionMap InjectionMap;

        internal ConcreteMap(InjectionMap injectionMap, IEnumerable<Type> concreteTypes)
        {
            InjectionMap = injectionMap;
            ConcreteTypes = concreteTypes;
        }

        public IResolutionEvent To<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            return CreateConcreteResolutionEventForInterface(new[] {interfaceType});
        }

        public IResolutionEvent AsSelf() => CreateConcreteResolutionEventForInterface(ConcreteTypes);

        public IResolutionEvent AsAllItsInterfaces()
        {
            var typeAndInterfaceTypePairsForTypes = AssemblyScanner.GetTypeAndInterfaceTypePairsForTypes(ConcreteTypes);

            return new AssemblyMappedResolutionEvent(InjectionMap, typeAndInterfaceTypePairsForTypes);
        }

        private IResolutionEvent CreateConcreteResolutionEventForInterface(IEnumerable<Type> interfaceTypes)
            => new MultiInterfaceConcretePairResolutionEvent(InjectionMap, ConcreteTypes, interfaceTypes);
    }

    internal sealed class ConcreteMap<TConcrete> : ConcreteMap, IMapTo<TConcrete>
    {
        internal ConcreteMap(InjectionMap injectionMap) : base(injectionMap, new[] {typeof(TConcrete)})
        {
        }

        public IResolutionEvent Using(Func<TConcrete> factory)
        {
            Func<IInjectionScope, TConcrete> factoryWrapped = scope => factory();

            return Using(factoryWrapped);
        }

        public IResolutionEvent Using(Func<IInjectionScope, TConcrete> factory)
            => new FactorizedResolutionEvent<TConcrete>(InjectionMap, factory);
    }
}