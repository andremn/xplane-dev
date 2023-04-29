using System;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Processing API.
    /// </summary>
    public interface IXPlaneProcessing
    {
        /// <summary>
        /// Rregisters the specified flight loop callback.
        /// </summary>
        /// <param name="flightLoopCallback">The flight loop callback to be registered.</param>
        /// <param name="interval">The interval at which the callback will be called. Use <see cref="TimeSpan.Zero"/> to not be called or null to be called at the next flight loop.</param>
        /// <returns>A flight callback handler to be used to unregister the callback from X-Plane later or to access the registered callback.</returns>
        FlightLoopCallbackHandler RegisterFlightLoopCallback(FlightLoopCallback flightLoopCallback, TimeSpan? interval);

        /// <summary>
        /// Unregisters the specified flight loop callback.
        /// </summary>
        /// <param name="flightLoopCallback">The flight loop callback to be unregistered.</param>
        void UnregisterFlightLoopCallback(FlightLoopCallback flightLoopCallback);
    }
}
