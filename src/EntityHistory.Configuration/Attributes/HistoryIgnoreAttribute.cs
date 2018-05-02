using System;

namespace EntityHistory.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HistoryIgnoreAttribute : Attribute
    {
    }
}
