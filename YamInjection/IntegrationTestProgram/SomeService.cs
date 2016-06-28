using System;

namespace IntegrationTestProgram
{
    public interface ISomeService
    {
        void DoSomething();
    }

    public class SomeService : ISomeService
    {
        public void DoSomething()
        {
            Console.WriteLine("It Worked");
            Console.ReadLine();
        }
    }
}