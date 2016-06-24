using System;

namespace YamInjection
{
    [Serializable]
    public sealed class ParameterTypeNotDefinedException : Exception
    {
        internal ParameterTypeNotDefinedException(Type parameterType, Type concreteType)
            : base($"Constructor paramter type ({parameterType.Name}) not mapped for constructor ({concreteType.Name})")
        {
            ParameterType = parameterType;
            ConcreteType = concreteType;
        }

        public Type ParameterType { get; }
        public Type ConcreteType { get; }
    }
}