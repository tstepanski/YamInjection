using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YamInjection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Delegate)]
    public sealed class DependencyInjectAttribute : Attribute
    {
    }

    internal static class DependencyInjectAttributeFinder
    {
        internal static IEnumerable<PropertyInfo> WhereHasDependencyInjectAttribute(
            this IEnumerable<PropertyInfo> propertiesInfo) => propertiesInfo.Where(HasDependencyInjectAttribute);

        internal static bool HasDependencyInjectAttribute(PropertyInfo propertyInfo)
            => propertyInfo.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(DependencyInjectAttribute));
    }
}