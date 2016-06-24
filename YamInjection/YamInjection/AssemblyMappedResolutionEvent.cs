using System.Collections.Generic;

namespace YamInjection
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
            Map.RegisterAllMappings(_concreteAndInterfacePairs, resolutionEventEnum);
        }
    }
}