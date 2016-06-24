namespace YamInjection
{
    public interface IMapTo
    {
        IResolutionEvent To<TInterface>() where TInterface : class;
        IResolutionEvent AsSelf();
    }
}