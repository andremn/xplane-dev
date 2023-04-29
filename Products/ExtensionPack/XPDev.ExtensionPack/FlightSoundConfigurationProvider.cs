using System;
using System.IO;
using System.Numerics;
using XPDev.FlightSoundsManagement.Configuration;

namespace XPDev.ExtensionPack
{
    /// <summary>
    /// Provides instances of XPDev.FlightSoundsManagement.Configuration.ISoundConfiguration.
    /// </summary>
    internal class FlightSoundConfigurationProvider : IFlightSoundConfigurationProvider
    {
        private readonly string _xPlaneRootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightSoundConfigurationProvider"/> with the specified root path of X-Plane and the airplane cockpit door.
        /// </summary>
        /// <param name="xPlaneRootPath">The absolute path of the X-Plane folder.</param>
        /// <param name="cockpitDoorPosition">The position of the cockpit door.</param>
        public FlightSoundConfigurationProvider(string xPlaneRootPath, Vector3 cockpitDoorPosition, TimeSpan delayBeforePlayingAnnouncement)
        {
            if (string.IsNullOrEmpty(xPlaneRootPath))
            {
                throw new ArgumentException("The parameter cannot be null or empty", nameof(xPlaneRootPath));
            }

            _xPlaneRootPath = xPlaneRootPath;

            CockpitDoorPosition = cockpitDoorPosition;
            DelayBeforePlayingAnnouncement = delayBeforePlayingAnnouncement;
        }

        public Vector3 CockpitDoorPosition { get; }

        public TimeSpan DelayBeforePlayingAnnouncement { get; }

        /// <summary>
        /// Gets an instance of XPDev.FlightSoundsManagement.Configuration.ISoundConfiguration.
        /// </summary>
        /// <returns>An instance of <see cref="ISoundConfiguration"/>.</returns>
        public IFlightSoundConfiguration GetFlightSoundConfiguration()
        {
            return new FmodSoundConfiguration(Path.Combine(_xPlaneRootPath, @"Resources/dlls/64/fmod64.dll"))
            {
                CockpitDoorPosition = CockpitDoorPosition,
                DelayBeforePlayingAnnouncement = DelayBeforePlayingAnnouncement
            };
        }
    }
}