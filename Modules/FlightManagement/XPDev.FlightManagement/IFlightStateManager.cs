namespace XPDev.FlightManagement
{
    /// <summary>
    /// Manages the state of a flight.
    /// </summary>
    public interface IFlightStateManager
    {
        /// <summary>
        /// Gets the current flight state.
        /// </summary>
        FlightState CurrentFlightState { get; }

        /// <summary>
        /// Gets or sets if the aircraft has changed to takeoff state.
        /// </summary>
        public bool HasChangedToTakeoff { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft has reached the cruise level.
        /// </summary>
        public bool HasReachedCruiseLevel { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft is on ground.
        /// </summary>
        public bool IsAircraftOnGround { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft front left door is open.
        /// </summary>
        public bool IsAircraftFronLeftDoorOpen { get; set; }

        /// <summary>
        /// Updates the current flight state based on the specified <see cref="FlightSnapshot"/>.
        /// </summary>
        /// <param name="flightSnapshot">The flight snapshot to be used to update the flight state.</param>
        void UpdateState(FlightSnapshot flightSnapshot);
    }
}