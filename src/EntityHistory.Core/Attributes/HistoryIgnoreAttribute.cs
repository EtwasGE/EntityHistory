using System;

namespace EntityHistory.Core.Attributes
{
    /// <summary>
    /// Used in OptOut mode to ignore the entity on the Audit logs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HistoryIgnoreAttribute : Attribute
    {
    }
}
