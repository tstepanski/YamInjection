using System.Reflection;

namespace YamInjection
{
    public sealed class PositionedParameter : IInjectionParameter
    {
        public PositionedParameter(uint position, object value)
        {
            Position = position;
            Value = value;
        }

        public uint Position { get; }
        public object Value { get; }

        public bool MatchesParameterDefinition(ParameterInfo parameterInfo)
        {
            return Position == parameterInfo.Position;
        }
    }
}