using System;

namespace SomeGreatLibrary
{
    public sealed class SomeEvenLessGreatImplementationOfAnAbstractClass : SomeNotSoGreatAbstractClass
    {
        public override int GetRandomInt()
        {
            return new Random(DateTime.Now.Millisecond).Next();
        }
    }
}