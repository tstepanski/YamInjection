using System;

namespace YamInjection.Exceptions
{
    [Serializable]
    public sealed class ParameterTypeNotDefinedException : Exception
    {
        internal ParameterTypeNotDefinedException(Type parameterType, Type concreteType)
            : base(GetMessageFromTypes(parameterType, concreteType))
        {
            ParameterType = parameterType;
            ConcreteType = concreteType;
        }

        public Type ParameterType { get; }
        public Type ConcreteType { get; }

        private static string GetMessageFromTypes(Type parameterType, Type concreteType)
            => $"Constructor paramter type ({parameterType.Name}) not mapped for constructor ({concreteType.Name})";
    }
}