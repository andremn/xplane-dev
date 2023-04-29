namespace XPDev.FlightSoundsManagement.Configuration
{
    /// <summary>
    /// Provides instances of <see cref="IFlightSoundConfiguration"/>.
    /// </summary>
    public interface IFlightSoundConfigurationProvider
    {
        /// <summary>
        /// Gets an instance of <see cref="IFlightSoundConfiguration"/>.
        /// </summary>
        /// <returns>An instance of <see cref="IFlightSoundConfiguration"/>.</returns>
        IFlightSoundConfiguration GetFlightSoundConfiguration();
    }
}
