using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomeGreatLibrary;

namespace YamInjection.Tests
{
    [TestClass]
    public class ScopeTests
    {
        [TestMethod]
        public void Resolve_GivenMappedInterfaceWithDependencies_ResolvesToConcreteImplementation()
        {
            var injectionMap = GreatInjectionMap.GetInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var service = scope.Resolve<ISomeGreatService>();

                Assert.IsNotNull(service);
            }
        }
    }
}