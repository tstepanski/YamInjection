using System;

namespace YamInjection.Exceptions
{
    [Serializable]
    public sealed class NoPublicConstructorException : Exception
    {
        internal NoPublicConstructorException(Type type)
            : base(GetMessageFromType(type))
        {
            Type = type;
        }

        public Type Type { get; }

        private static string GetMessageFromType(Type type)
            => $"No public constructor defined for the given type {type.Name}";
    }
}