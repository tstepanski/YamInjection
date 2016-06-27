using System.Reflection;

namespace YamInjection
{
    public interface IInjectionMap
    {
        IInjectionMap Map(IInjectionMap injectionMap);
        IMapTo<TConcrete> Map<TConcrete>() where TConcrete : class;
        IResolutionEvent MapAssembly(Assembly assemblyToScan);
    }
}