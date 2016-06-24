using System;

namespace SomeGreatLibrary
{
    public static class SomeReallyBadStaticClass
    {
        public static string GetMeAGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}