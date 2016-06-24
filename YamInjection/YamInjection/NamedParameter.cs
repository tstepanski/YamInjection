using System;
using System.Reflection;

namespace YamInjection
{
    public sealed class NamedParameter : IInjectionParameter
    {
        public NamedParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }

        public bool MatchesParameterDefinition(ParameterInfo parameterInfo)
        {
            return Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}