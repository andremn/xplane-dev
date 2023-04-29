using System;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// Argument for flight state changed event.
    /// </summary>
    public class FlightStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FlightStateChangedEventArgs"/> class with the specified old and new states.
        /// </summary>
        /// <param name="oldFlightState">The flight state before changing.</param>
        /// <param name="newFlightState">The flight state after changing.</param>
        public FlightStateChangedEventArgs(FlightState oldFlightState, FlightState newFlightState)
        {
            OldFlightState = oldFlightState;
            NewFlightState = newFlightState;
        }

        /// <summary>
        /// Gets the flight state before changing.
        /// </summary>
        public FlightState OldFlightState { get; }


        /// <summary>
        /// Gets the flight state after changing.
        /// </summary>
        public FlightState NewFlightState { get; }
    }
}
