using XPDev.Modularization;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Manages aircraft sounds.
    /// </summary>
    public interface IAircraftSoundsService : IModuleService
    {
        /// <summary>
        /// Adds the specified aircraft sound to be played when the condition is met.
        /// </summary>
        /// <param name="flightSound">The aircraft sound to be added.</param>
        void AddAircraftSound(FlightSound flightSound);
    }
}