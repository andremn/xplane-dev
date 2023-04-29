using System;
using System.Collections.Generic;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Used to specify a method which can be invoked in the flight loop.
    /// </summary>
    /// <param name="elapsedTimeSinceLastCall">The time elapsed since the last time the callback was called.</param>
    /// <param name="elapsedTimeSinceLastFlightLoop">The time elapsed since the last time a flight lopp was run.</param>
    /// <param name="flightLoopNumber">The number of the current flight loop.</param>
    /// <returns>The time interval at which the callback will be called again. Use <see cref="TimeSpan.Zero"/> to not be called or null to be called at the next loop.</returns>
    public delegate TimeSpan? FlightLoopCallback(TimeSpan elapsedTimeSinceLastCall, TimeSpan elapsedTimeSinceLastFlightLoop, int flightLoopNumber);

    /// <summary>
    /// Provides access to the X-Plane Processing API.
    /// </summary>
    internal class XPLMProcessing : IXPlaneProcessing
    {
        private  readonly IDictionary<FlightLoopCallback, FlightLoopCallbackHandler> _registeredCallbacks;

        /// <summary>
        /// Initializes a new instance of the <see cref="XPLMProcessing"/> class.
        /// </summary>
        public XPLMProcessing()
        {
            _registeredCallbacks = new Dictionary<FlightLoopCallback, FlightLoopCallbackHandler>();
        }

        /// <summary>
        /// Rregisters the specified flight loop callback.
        /// </summary>
        /// <param name="flightLoopCallback">The flight loop callback to be registered.</param>
        /// <param name="interval">The interval at which the callback will be called. Use <see cref="TimeSpan.Zero"/> to not be called or null to be called at the next flight loop.</param>
        /// <returns>A flight callback handler to be used to unregister the callback from X-Plane later or to access the registered callback.</returns>
        public FlightLoopCallbackHandler RegisterFlightLoopCallback(FlightLoopCallback flightLoopCallback, TimeSpan? interval)
        {            
            var handler = new FlightLoopCallbackHandler(flightLoopCallback, interval);

            _registeredCallbacks[flightLoopCallback] = handler;

            return handler;
        }

        /// <summary>
        /// Unregisters the specified flight loop callback.
        /// </summary>
        /// <param name="flightLoopCallback">The flight loop callback to be unregistered.</param>
        public void UnregisterFlightLoopCallback(FlightLoopCallback flightLoopCallback)
        {
            if (_registeredCallbacks.TryGetValue(flightLoopCallback, out var handler))
            {
                XPLMProcessingNativeMethods.XPLMUnregisterFlightLoopCallback(handler.XPlaneFlightLoopCallback, IntPtr.Zero);
                _registeredCallbacks.Remove(flightLoopCallback);
            }
        }
    }
}
