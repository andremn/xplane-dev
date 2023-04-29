using System;
using System.Numerics;
using XPDev.FlightSoundsManagement.Configuration;

namespace XPDev.FlightSoundsSimulator
{
    /// <summary>
    /// Provides instances of XPDev.FlightSoundsManagement.Configuration.ISoundConfiguration.
    /// </summary>
    internal class FlightSoundConfigurationProvider : IFlightSoundConfigurationProvider
    {
        private static readonly Vector3 _cockpitDoorPosition = new Vector3(0, 0, 10);

        private readonly string _fmodDllPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightSoundConfigurationProvider"/> with the specified root path of X-Plane.
        /// </summary>
        /// <param name="fmodDllPath"></param>
        public FlightSoundConfigurationProvider(string fmodDllPath)
        {
            if (string.IsNullOrEmpty(fmodDllPath))
            {
                throw new ArgumentException("The parameter cannot be null or empty", nameof(fmodDllPath));
            }

            _fmodDllPath = fmodDllPath;
        }

        /// <summary>
        /// Gets an instance of XPDev.FlightSoundsManagement.Configuration.ISoundConfiguration.
        /// </summary>
        /// <returns>An instance of <see cref="ISoundConfiguration"/>.</returns>
        public IFlightSoundConfiguration GetFlightSoundConfiguration()
        {
            return new FmodSoundConfiguration(_fmodDllPath)
            {
                CockpitDoorPosition = _cockpitDoorPosition,
                DelayBeforePlayingAnnouncement = TimeSpan.FromSeconds(3)
            };
        }
    }
}