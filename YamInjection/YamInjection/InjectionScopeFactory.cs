namespace YamInjection
{
    public static class InjectionScopeFactory
    {
        public static IInjectionScope BeginNewInjectionScope() => new InjectionScope();
    }
}