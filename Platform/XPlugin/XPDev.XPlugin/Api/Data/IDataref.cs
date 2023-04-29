namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Represents an X-Plane data ref.
    /// </summary>
    public interface IDataRef
    {
        /// <summary>
        /// Gets the name of the data ref.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Represents an X-Plane data ref with a value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the data ref value.</typeparam>
    public interface IDataRef<T> : IDataRef
    {
        /// <summary>
        /// Gets or sets the data ref value.
        /// </summary>
        T Value { get; set; }
    }
}
