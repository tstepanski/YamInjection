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

        public void SomeGreatMethod()
        {
        }
    }
}