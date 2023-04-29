using System;
using System.Numerics;
using XPDev.Audio;

namespace XPDev.FlightSoundsManagement.Configuration
{
    /// <summary>
    /// Represents the Fmod sound configuration to be used when playing flight sounds.
    /// </summary>
    public class FmodSoundConfiguration : IFlightSoundConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FmodSoundConfiguration"/> class with the specified path for the Fmod DLL.
        /// </summary>
        /// <param name="fmodDllPath">The path for the Fmod DLL.</param>
        public FmodSoundConfiguration(string fmodDllPath)
        {
            if (string.IsNullOrEmpty(fmodDllPath))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(fmodDllPath));
            }

            FmodDllPath = fmodDllPath;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SoundManagerEngine Engine => SoundManagerEngine.Fmod;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string FmodDllPath { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Vector3 CockpitDoorPosition { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TimeSpan DelayBeforePlayingAnnouncement { get; set; }
    }
}
