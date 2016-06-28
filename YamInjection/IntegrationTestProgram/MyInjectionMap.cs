using YamInjection.Internals;

namespace IntegrationTestProgram
{
    public class MyInjectionMap : InjectionMap
    {
        public override void Register()
        {
            Map<SomeService>()
                .To<ISomeService>()
                .ResolveOnlyOnce();
        }
    }
}