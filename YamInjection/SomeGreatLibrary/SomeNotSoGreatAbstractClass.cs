using System;

namespace SomeGreatLibrary
{
    public interface IRandomDoubleGenerator
    {
        double GetRandomDecimal();
    }

    public abstract class SomeNotSoGreatAbstractClass : IRandomDoubleGenerator
    {
        public double GetRandomDecimal()
        {
            return new Random(DateTime.Now.Millisecond).NextDouble();
        }

        public abstract int GetRandomInt();
    }
}