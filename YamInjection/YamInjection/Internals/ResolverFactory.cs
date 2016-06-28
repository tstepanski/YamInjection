using System;

namespace YamInjection.Internals
{
    internal static class ResolverFactory
    {
        public static Resolver GetResolver(ResolutionEventEnum resolutionEvent, Type typeToResolve)
        {
            switch (resolutionEvent)
            {
                case ResolutionEventEnum.OneInstance:
                    return CreateSingleInstanceResolver(typeToResolve);
                case ResolutionEventEnum.EveryRequest:
                    return CreatePerRequestInstanceResolver(typeToResolve);
                case ResolutionEventEnum.OncePerScope:
                    return CreateScopedInstanceResolver(typeToResolve);
                default:
                    throw new NotImplementedException("Given resolution type is not yet supported");
            }
        }

        private static Resolver CreateScopedInstanceResolver(Type typeToResolve)
            => new ScopedInstanceResolver(typeToResolve);

        private static Resolver CreatePerRequestInstanceResolver(Type typeToResolve)
            => new PerRequestInstanceResolver(typeToResolve);

        private static Resolver CreateSingleInstanceResolver(Type typeToResolve)
            => new SingleInstanceResolver(typeToResolve);
    }
}