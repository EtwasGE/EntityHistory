using System;

namespace EntityHistory.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HistoryIncludeAttribute : Attribute
    {
    }
}
