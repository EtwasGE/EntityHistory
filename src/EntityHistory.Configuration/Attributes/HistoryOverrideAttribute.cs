using System;

namespace EntityHistory.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HistoryOverrideAttribute : Attribute
    {
        public object Value { get; set; }

        public HistoryOverrideAttribute()
        {
        }

        public HistoryOverrideAttribute(object value)
        {
            Value = value;
        }
    }
}
