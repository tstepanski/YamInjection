namespace YamInjection
{
    public interface IResolutionEvent
    {
        IInjectionMap ResolveEveryRequest();
        IInjectionMap ResolveOncePerScope();
        IInjectionMap ResolveOnlyOnce();
    }
}