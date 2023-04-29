using System;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Represents a handler for a scheduled flight loop callback.
    /// </summary>
    public class FlightLoopCallbackHandler
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FlightLoopCallbackHandler"/> class.
        /// </summary>
        /// <param name="flightLoopCallback">The user flight loop callback handler.</param>
        /// <param name="interval">The interval which X-Plane will call the callback.</param>
        internal FlightLoopCallbackHandler(FlightLoopCallback flightLoopCallback, TimeSpan? interval)
        {
            FlightLoopCallback = flightLoopCallback;
            XPlaneFlightLoopCallback = new XPLMFlightLoop(OnFlightLoop);
            XPLMProcessingNativeMethods.XPLMRegisterFlightLoopCallback(XPlaneFlightLoopCallback, (float?)interval?.TotalSeconds ?? -1f, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the registered flight loop callback.
        /// </summary>
        public FlightLoopCallback FlightLoopCallback { get; }

        /// <summary>
        /// Gets the X-Plane flight loop callback.
        /// </summary>
        internal XPLMFlightLoop XPlaneFlightLoopCallback { get; }

        private float OnFlightLoop(float inElapsedSinceLastCall, float inElapsedTimeSinceLastFlightLoop, int inCounter, IntPtr inRefcon)
        {
            if (FlightLoopCallback == null)
            {
                // Returns 0 to make the callback inactive
                return 0;
            }

            var nextFlightLoop = FlightLoopCallback(TimeSpan.FromSeconds(inElapsedSinceLastCall), TimeSpan.FromSeconds(inElapsedTimeSinceLastFlightLoop), inCounter);

            if (!nextFlightLoop.HasValue)
            {
                // Return -1 to be called on the next flight loop
                return -1f;
            }

            return (float)nextFlightLoop.Value.TotalSeconds;
        }
    }
}
