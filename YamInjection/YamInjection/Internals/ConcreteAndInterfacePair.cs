using System;

namespace YamInjection.Internals
{
    internal class ConcreteAndInterfacePair
    {
        internal ConcreteAndInterfacePair(Type concreteType, Type interfaceType)
        {
            ConcreteType = concreteType;
            InterfaceType = interfaceType;
        }

        public Type ConcreteType { get; }
        public Type InterfaceType { get; }
    }
}