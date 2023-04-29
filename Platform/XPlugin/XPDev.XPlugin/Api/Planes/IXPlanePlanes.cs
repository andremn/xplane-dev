namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Planes API.
    /// </summary>
    public interface IXPlanePlanes
    {
        /// <summary>
        /// Gets whether the specified aircraft index represents the user aircraft.
        /// </summary>
        /// <param name="aircraftIndex">The aircraft index to be checked.</param>
        /// <returns>True if the aircraft index represents the user aircraft; false otherwise.</returns>
        bool IsUserAircraft(int aircraftIndex);

        /// <summary>
        /// Gets the name of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the name of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        string GetCurrentAircraftFileName();

        /// <summary>
        /// Gets the path of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the path of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        string GetCurrentAircraftFilePath();

        /// <summary>
        /// Gets the folder path of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the folder of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        string GetCurrentAircraftFolderPath();
    }
}