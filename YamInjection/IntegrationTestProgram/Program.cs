using YamInjection;

namespace IntegrationTestProgram
{
    public class Program
    {
        public static void Main()
        {
            using (var scope = InjectionScopeFactory.BeginNewInjectionScope())
            {
                var myInjectionMap = new MyInjectionMap();

                scope.UseMap(myInjectionMap);

                var service = scope.Resolve<ISomeService>();

                service.DoSomething();
            }
        }
    }
}