using System;

namespace YamInjection
{
    public interface IMapTo
    {
        IResolutionEvent To<TInterface>() where TInterface : class;
        IResolutionEvent AsSelf();
    }

    public interface IMapTo<in TConcrete> : IMapTo
    {
        IResolutionEvent Using(Func<TConcrete> factory);
        IResolutionEvent Using(Func<IInjectionScope, TConcrete> factory);
    }
}