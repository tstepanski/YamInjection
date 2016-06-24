using System.Collections.Generic;

namespace SomeGreatLibrary
{
    public interface ISomeGreatRepository
    {
        KeyValuePair<int, string> GetSomeKeyValuePair(int key);
    }

    public class SomeGreatRepository : ISomeGreatRepository
    {
        public KeyValuePair<int, string> GetSomeKeyValuePair(int key)
        {
            return new KeyValuePair<int, string>(key, key.ToString());
        }
    }
}