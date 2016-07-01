using YamInjection;

namespace SomeGreatLibrary
{
    public interface ISomeGreatService
    {
        void SomeGreatMethod();
    }

    public class SomeGreatService : ISomeGreatService
    {
        private readonly ISomeGreatRepository _repository;

        public SomeGreatService(ISomeGreatRepository repository)
        {
            _repository = repository;
        }

        [DependencyInject]
        public IRandomDoubleGenerator RandomDoubleGenerator { get; set; }

        public void SomeGreatMethod()
        {
        }
    }
}