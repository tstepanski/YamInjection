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
            var injectionMap = new GreatInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var service = scope.Resolve<ISomeGreatService>();

                Assert.IsNotNull(service);
            }
        }

        [TestMethod]
        public void Resolve_GivenMappedTypeWithFactory_ResolvesToConcreteImplementation()
        {
            var injectionMap = new GreatInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var dbContext = scope.Resolve<SomeDbContext>();

                Assert.IsNotNull(dbContext);
            }
        }
    }
}