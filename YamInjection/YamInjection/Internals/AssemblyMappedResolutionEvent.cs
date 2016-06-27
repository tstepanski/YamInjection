using System.Collections.Generic;

namespace YamInjection.Internals
{
    internal sealed class AssemblyMappedResolutionEvent : ResolutionEventBase
    {
        private readonly IEnumerable<ConcreteAndInterfacePair> _concreteAndInterfacePairs;

        internal AssemblyMappedResolutionEvent(InjectionMap map,
            IEnumerable<ConcreteAndInterfacePair> concreteAndInterfacePairs) : base(map)
        {
            _concreteAndInterfacePairs = concreteAndInterfacePairs;
        }

        protected override void CompleteMapping(ResolutionEventEnum resolutionEventEnum)
        {
            MapRegistrar.RegisterAllMappings(Map, _concreteAndInterfacePairs, resolutionEventEnum);
        }
    }
}