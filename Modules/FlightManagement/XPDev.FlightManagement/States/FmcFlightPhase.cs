namespace XPDev.FlightManagement
{
    /// <summary>
    /// The possible phases of a flight in a FMC.
    /// </summary>
    public enum FmcFlightPhase
    {
        /// <summary>
        /// No defined phase.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The phase if takeoff.
        /// </summary>
        Takeoff,
        /// <summary>
        /// The phase is climb.
        /// </summary>
        Climb,
        /// <summary>
        /// The phase is cruise.
        /// </summary>
        Cruise,
        /// <summary>
        /// The phase is descent.
        /// </summary>
        Descent,
        /// <summary>
        /// The phase is approach.
        /// </summary>
        Approach,
        /// <summary>
        /// The phase is go around.
        /// </summary>
        GoAround
    }
}
