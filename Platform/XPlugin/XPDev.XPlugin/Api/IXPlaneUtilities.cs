namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Utilities API.
    /// </summary>
    public interface IXPlaneUtilities
    {
        /// <summary>
        /// Gets the directory of the current running X-Plane plugin.
        /// </summary>
        /// <returns>The absolute path of the current running X-Plane plugin.</returns>
        string GetPluginDirectory();

        /// <summary>
        /// Gets the ID of the current running X-Plane plugin.
        /// </summary>
        /// <returns>An integer number representing the current running X-Plane plugin.</returns>
        int GetPluginId();

        /// <summary>
        /// Writes the specified message to the X-Plane log file.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        void WriteDebugLog(string message);
    }
}