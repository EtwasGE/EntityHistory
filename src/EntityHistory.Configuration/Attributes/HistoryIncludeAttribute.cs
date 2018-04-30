using System;

namespace EntityHistory.Configuration.Attributes
{
    /// <summary>
    /// Used with OptIn AnnotationMode to include the entity on the Audit logs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HistoryIncludeAttribute : Attribute
    {
    }
}
