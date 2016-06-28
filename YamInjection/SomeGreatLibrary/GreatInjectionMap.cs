using YamInjection.Internals;

namespace SomeGreatLibrary
{
    public class GreatInjectionMap : InjectionMap
    {
        public override void Register()
        {
            MapAssembly(MyAssembly)
                .ResolveEveryRequest();

            Map<SomeDbContext>()
                .Using(() => new SomeDbContext("someConnectionString"))
                .ResolveOncePerScope();
        }
    }
}