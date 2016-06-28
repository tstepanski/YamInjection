using System;

namespace YamInjection.Internals
{
    internal sealed class InstanceNotYetResolvedException : Exception
    {
        internal InstanceNotYetResolvedException(Type type) : base(GetMessageFromType(type))
        {
            Type = type;
        }

        public Type Type { get; }

        private static string GetMessageFromType(Type type) => $"{type.Name} has not yet been resolved";
    }
}