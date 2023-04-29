using System.IO;
using System.Text;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Planes API.
    /// </summary>
    public class XPlanePlanes : IXPlanePlanes
    {
        /// <summary>
        /// Gets the number of the current aircraft, that is, the aircraft selected by the user.
        /// </summary>
        public const int UserAircraftIndex = 0;

        /// <summary>
        /// Gets whether the specified aircraft index represents the user aircraft.
        /// </summary>
        /// <param name="aircraftIndex">The aircraft index to be checked.</param>
        /// <returns>True if the aircraft index represents the user aircraft; false otherwise.</returns>
        public bool IsUserAircraft(int aircraftIndex)
        {
            return aircraftIndex == UserAircraftIndex;
        }

        /// <summary>
        /// Gets the name of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the name of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        public string GetCurrentAircraftFileName()
        {
            GetAircraftModel(UserAircraftIndex, out var aircraftFileName, out var _);

            return aircraftFileName;
        }

        /// <summary>
        /// Gets the path of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the path of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        public string GetCurrentAircraftFilePath()
        {
            GetAircraftModel(UserAircraftIndex, out var _, out var aircraftPath);

            return aircraftPath;
        }

        /// <summary>
        /// Gets the folder path of the current aircraft ACF file.
        /// </summary>
        /// <returns>A string representing the folder of the current aircraft ACF file or null if no aircraft is loaded.</returns>
        public string GetCurrentAircraftFolderPath()
        {
            GetAircraftModel(UserAircraftIndex, out var _, out var aircraftPath);

            return Path.GetDirectoryName(aircraftPath);
        }

        private void GetAircraftModel(int aircraftId, out string aircraftFileName, out string aircraftFilePath)
        {
            var fileName = new StringBuilder(256);
            var path = new StringBuilder(512);

            XPlanePlanesNativeMethods.XPLMGetNthAircraftModel(aircraftId, fileName, path);

            aircraftFileName = fileName.ToString();
            aircraftFilePath = path.ToString();
        }
    }
}
