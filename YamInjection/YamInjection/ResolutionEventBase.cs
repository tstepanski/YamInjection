namespace YamInjection
{
    public interface IResolutionEvent
    {
        IInjectionMap ResolveEveryRequest();
        IInjectionMap ResolveOncePerScope();
        IInjectionMap ResolveOnlyOnce();
    }

    internal abstract class ResolutionEventBase : IResolutionEvent
    {
        protected InjectionMap Map;

        internal ResolutionEventBase(InjectionMap map)
        {
            Map = map;
        }

        public IInjectionMap ResolveEveryRequest() => Resolve(ResolutionEventEnum.EveryRequest);
        public IInjectionMap ResolveOncePerScope() => Resolve(ResolutionEventEnum.OncePerScope);
        public IInjectionMap ResolveOnlyOnce() => Resolve(ResolutionEventEnum.OneInstance);

        protected abstract void CompleteMapping(ResolutionEventEnum resolutionEventEnum);

        private IInjectionMap Resolve(ResolutionEventEnum resolutionEventEnum)
        {
            CompleteMapping(resolutionEventEnum);

            return Map;
        }
    }
}