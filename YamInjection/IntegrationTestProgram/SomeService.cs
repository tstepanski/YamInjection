using System;

namespace IntegrationTestProgram
{
    public interface ISomeService
    {
        void DoSomething();
        string GetSomeString();
    }

    public class SomeService : ISomeService
    {
        public void DoSomething()
        {
            var someString = GetSomeString();

            Console.WriteLine(someString);

            Console.ReadLine();
        }

        public string GetSomeString()
        {
            return "It Worked";
        }
    }
}