namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// The possible phases of the flight loop registered callbacks will be run.
    /// </summary>
    public enum FlightLoopPhase : int
    {
        /// <summary>
        /// Specifies the callback should be run before the flight model is integrated into X-Plane.
        /// </summary>
        BeforeFlightModel = 0,
        /// <summary>
        /// Specifies the callback should be run after the flight model is integrated into X-Plane.
        /// </summary>
        AfterFlightModel
    }
}
