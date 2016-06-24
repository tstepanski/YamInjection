using System.Reflection;

namespace YamInjection
{
    public interface IInjectionParameter
    {
        object Value { get; }
        bool MatchesParameterDefinition(ParameterInfo parameterInfo);
    }
}