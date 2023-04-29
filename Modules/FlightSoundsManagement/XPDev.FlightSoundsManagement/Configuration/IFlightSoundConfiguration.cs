using System;
using System.Numerics;
using XPDev.Audio;

namespace XPDev.FlightSoundsManagement.Configuration
{
    /// <summary>
    /// Represents the sound configuration to be used when playing flight sounds.
    /// </summary>
    public interface IFlightSoundConfiguration
    {
        /// <summary>
        /// Gets the <see cref="SoundManagerEngine"/> to be used.
        /// </summary>
        SoundManagerEngine Engine { get; }

        /// <summary>
        /// Gets the position of the cockpit door, which is used to determine whether the listener is inside the cockpit.
        /// </summary>
        Vector3 CockpitDoorPosition { get; set; }

        /// <summary>
        /// Gets the delay applied before start playing an announcement.
        /// </summary>
        TimeSpan DelayBeforePlayingAnnouncement { get; set; }
    }
}
