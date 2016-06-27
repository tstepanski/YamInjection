namespace SomeGreatLibrary
{
    public class SomeDbContext
    {
        private readonly string _connectionString;

        public SomeDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}