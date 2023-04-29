using System;
using XPDev.FlightManagement;
using XPDev.Modularization;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Provides methods related to announcement sounds.
    /// </summary>
    public interface IAnnouncementService : IModuleService
    {
        /// <summary>
        /// Gets or sets the time to wait before playing an announcement.
        /// </summary>
        TimeSpan DelayToPlayAnnouncement { get; set; }

        /// <summary>
        /// Sets the path of a sound file that will be played in the specified <see cref="FlightState"/>.
        /// </summary>
        /// <param name="soundFilePath">The path of the sound file.</param>
        /// <param name="flightAction">The sound file to be played when the specified action happens.</param>
        void SetAnnouncementSoundFileForFlightAction(string soundFilePath, FlightAction flightAction);
    }
}