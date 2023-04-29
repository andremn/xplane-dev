namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Data (DataRefs) API.
    /// </summary>
    public interface IXPlaneDataAccess
    {
        /// <summary>
        /// Finds a data ref by the specified name and returns a handle to it if found.
        /// </summary>
        /// <param name="name">The name of the data ref to be looked up.</param>
        /// <returns>An instance of <see cref="DataRefHandler"/> if the data ref with the specified name was found; null otherwise.</returns>
        DataRefHandler FindDataRef(string name);
    }
}
