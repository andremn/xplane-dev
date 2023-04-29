using System;
using System.Runtime.InteropServices;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Used to specify a method which can be invoked in the X-Plane flight loop.
    /// </summary>
    /// <param name="inElapsedSinceLastCall">The time elapsed since the last time the callback was called.</param>
    /// <param name="inElapsedTimeSinceLastFlightLoop">The time elapsed since the last time a flight lopp was run.</param>
    /// <param name="inCounter">The number of the current flight loop.</param>
    /// <param name="inRefcon">The object to be passed to the callback function.</param>
    /// <returns>The time interval at which the callback will be called again. Use 0 to not be called or -1 to be called at the next loop.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate float XPLMFlightLoop(float inElapsedSinceLastCall, float inElapsedTimeSinceLastFlightLoop, int inCounter, IntPtr inRefcon);

    /// <summary>
    /// Represents the parameters to be used when creating a flight loop.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XPLMCreateFlightLoop
    {
        /// <summary>
        /// The size of the struct.
        /// </summary>
        public int StructSize;

        /// <summary>
        /// The phase the flight loop should be called.
        /// </summary>
        public FlightLoopPhase Phase;

        /// <summary>
        /// The callback to be invoked.
        /// </summary>
        public XPLMFlightLoop CallbackFunc;

        /// <summary>
        /// A object to be passed to the callback function.
        /// </summary>
        public IntPtr Refcon;
    }

    /// <summary>
    /// Provides access to the X-Plane Processing API native methods.
    /// </summary>
    internal static class XPLMProcessingNativeMethods
    {
        /// <summary>
        /// Rregisters the specified flight loop callback.
        /// </summary>
        /// <param name="inFlightLoop">The flight loop callback to be registered.</param>
        /// <param name="inInterval">The interval at which the callback will be called. Use 0 to not be called or -1 to be called at the next flight loop.</param>
        /// <param name="inRefcon">The object to be passed to the callback function.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl)]
        public extern static void XPLMRegisterFlightLoopCallback(XPLMFlightLoop inFlightLoop, float inInterval, IntPtr inRefcon);

        /// <summary>
        /// Unregisters the specified flight loop callback.
        /// </summary>
        /// <param name="inFlightLoop">The flight loop callback to be unregistered.</param>
        /// <param name="inRefcon">The object to be passed to the callback function.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl)]
        public extern static void XPLMUnregisterFlightLoopCallback(XPLMFlightLoop inFlightLoop, IntPtr inRefcon);
    }
}
