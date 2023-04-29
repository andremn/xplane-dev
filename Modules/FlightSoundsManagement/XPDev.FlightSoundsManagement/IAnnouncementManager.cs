using System;
using System.Threading.Tasks;
using XPDev.FlightManagement;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Manages announcements logic.
    /// </summary>
    public interface IAnnouncementManager
    {
        /// <summary>
        /// Gets or sets the time to wait before playing an announcement.
        /// </summary>
        TimeSpan DelayToPlayAnnouncement { get; set; }

        /// <summary>
        /// Sets the path of a sound file that will be played in the specified <see cref="FlightState"/>.
        /// Different paths can be used for the same flight action. 
        /// In this case, the announcements will play one after the other in the other they were added.
        /// </summary>
        /// <param name="soundFilePath">The path of the sound file.</param>
        /// <param name="flightAction">The sound file to be played when the specified action happens.</param>
        void SetAnnouncementSoundFileForFlightAction(string soundFilePath, FlightAction flightAction);

        /// <summary>
        /// Process the queued and playing announcements.
        /// </summary>
        /// <param name="flightSnapshot">The flight snapshot to be processed.</param>
        void ProcessFlightSnapshot(FlightSnapshot flightSnapshot);

        /// <summary>
        /// Process the announcements to be played.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ProcessAnnouncementsAsync();
    }
}