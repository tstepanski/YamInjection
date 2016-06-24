using System;

namespace SomeGreatLibrary
{
    public interface IUnitOfWork
    {
    }

    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        [ThreadStatic] public static IUnitOfWork Instance;

        public UnitOfWork()
        {
            _previous = Instance;

            Instance = this;
        }

        public delegate IUnitOfWork Factory();

        private readonly IUnitOfWork _previous;

        public void Dispose()
        {
            Instance = _previous;
        }
    }
}