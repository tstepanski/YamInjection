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

        [TestMethod]
        public void Resolve_GivenResolutionSetToInstancePerScope_ResolvesOnlyOnceForScope()
        {
            var injectionMap = new GreatInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var dbContextA = scope.Resolve<SomeDbContext>();

                var dbContextB = scope.Resolve<SomeDbContext>();

                Assert.AreSame(dbContextA, dbContextB);
            }
        }

        [TestMethod]
        public void Resolve_GivenResolutionSetToInstancePerScope_ResolvesNewInstanceForEachScope()
        {
            var injectionMap = new GreatInjectionMap();

            SomeDbContext dbContextA;
            SomeDbContext dbContextB;

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                dbContextA = scope.Resolve<SomeDbContext>();
            }

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                dbContextB = scope.Resolve<SomeDbContext>();
            }

            Assert.AreNotSame(dbContextA, dbContextB);
        }

        [TestMethod]
        public void Resolve_GivenResolutionSetToInstancePerRequest_ResolvesNewInstanceForEachRequest()
        {
            var injectionMap = new GreatInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var greatServiceA = scope.Resolve<ISomeGreatService>();

                var greatServiceB = scope.Resolve<ISomeGreatService>();

                Assert.AreNotSame(greatServiceA, greatServiceB);
            }
        }

        [TestMethod]
        public void Resolve_GivenObjectWithPropertiesMarkedForPropertyInjection_ResolvesPropertyToInstance()
        {
            var injectionMap = new GreatInjectionMap();

            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                scope.UseMap(injectionMap);

                var service = (SomeGreatService) scope.Resolve<ISomeGreatService>();

                Assert.IsNotNull(service.RandomDoubleGenerator);
            }
        }
    }
}