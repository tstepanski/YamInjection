using System;

namespace YamInjection
{
    [Serializable]
    public sealed class NoPublicConstructorException : Exception
    {
        internal NoPublicConstructorException(Type type)
            : base($"No public constructor defined for the given type {type.Name}")
        {
            Type = type;
        }

        public Type Type { get; }
    }
}