namespace EntityHistory.Core.Interfaces
{
    public interface IEntityPropertyChange
    {
        /// <summary>
        /// NewValue.
        /// </summary>
        string NewValue { get; set; }

        /// <summary>
        /// OriginalValue.
        /// </summary>
        string OriginalValue { get; set; }

        /// <summary>
        /// PropertyName.
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="NewValue"/> and <see cref="OriginalValue"/>.
        /// It's the FullName of the type.
        /// </summary>
        string PropertyTypeFullName { get; set; }
    }
}
