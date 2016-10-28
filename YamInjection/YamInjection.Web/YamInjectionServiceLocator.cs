using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DependencyResolver = YamInjection.Internals.DependencyResolver;

namespace YamInjection.Web
{
    public sealed class YamInjectionServiceLocator : IDependencyResolver
    {
        private const string YamInjectionScopeCache = "YamInjectionScopeCache";

        private readonly IInjectionMap _injectionMap;

        public YamInjectionServiceLocator(IInjectionMap injectionMap)
        {
            _injectionMap = injectionMap;
        }

        private static object CurrentRequestScopeAsObject
        {
            get { return HttpContext.Current.Cache[YamInjectionScopeCache]; }
            set { HttpContext.Current.Cache[YamInjectionScopeCache] = value; }
        }

        public object GetService(Type serviceType)
        {
            var sessionScope = GetSessionScope();

            return GetIsController(serviceType)
                ? DependencyResolver.CreateInstanceForType(serviceType, sessionScope)
                : sessionScope.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var sessionScope = GetSessionScope();

            return sessionScope.ResolveAll(serviceType);
        }

        private static bool GetIsController(Type serviceType)
            => serviceType.BaseType == typeof(Controller) || serviceType == typeof(ApiController);

        private IInjectionScope GetSessionScope()
        {
            var currentCachedScope = CurrentRequestScopeAsObject as IInjectionScope;

            if (currentCachedScope != null)
            {
                return currentCachedScope;
            }

            var injectionScope = InjectionScopeFactory.BeginNewInjectionScope();

            injectionScope.UseMap(_injectionMap);

            CurrentRequestScopeAsObject = injectionScope;

            return injectionScope;
        }
    }
}