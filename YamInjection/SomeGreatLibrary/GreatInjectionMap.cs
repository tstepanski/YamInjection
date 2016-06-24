using System.Reflection;
using YamInjection;

namespace SomeGreatLibrary
{
    public static class GreatInjectionMap
    {
        public static IInjectionMap GetInjectionMap()
        {
            var injectionMap = InjectionMapFactory.NewMap();

            var thisAssembly = Assembly.GetAssembly(typeof(GreatInjectionMap));

            injectionMap.MapAssembly(thisAssembly).ResolveEveryRequest();

            return injectionMap;
        }
    }
}