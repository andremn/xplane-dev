using System;
using XPDev.Modularization;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// Provides methods related to flight state operations.
    /// </summary>
    public interface IFlightStateService : IModuleService
    {
        /// <summary>
        /// Occurs when the current flight state changes to another state.
        /// </summary>
        event EventHandler<FlightStateChangedEventArgs> FlightStateChanged;

        /// <summary>
        /// Gets the current flight state.
        /// </summary>
        FlightState CurrentFlightState { get; }
    }
}