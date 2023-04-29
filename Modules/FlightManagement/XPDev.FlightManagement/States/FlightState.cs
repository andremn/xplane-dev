namespace XPDev.FlightManagement
{
    /// <summary>
    /// Represents the possible states of a flight.
    /// </summary>
    public enum FlightState
    {
        /// <summary>
        /// The aircraft is parked at the gate.
        /// </summary>
        Parked,
        /// <summary>
        /// The aircraft is boarding passengers.
        /// </summary>
        Boarding,
        /// <summary>
        /// The aircraft finished boarding passengers and is waiting pushback clearance.
        /// </summary>
        BoardingDone,
        /// <summary>
        /// The aircraft is ready to be pushed back.
        /// </summary>
        Pushback,
        /// <summary>
        /// The aircraft is taxing out to takeoff.
        /// </summary>
        TaxiOut,
        /// <summary>
        /// The aircraft is taking off.
        /// </summary>
        Takeoff,
        /// <summary>
        /// The aircraft is climbing to cruise altitude.
        /// </summary>
        Climb,
        /// <summary>
        /// The aircraft is at cruise level.
        /// </summary>
        Cruise,
        /// <summary>
        /// The aircraft is descending from cruise level.
        /// </summary>
        Descent,
        /// <summary>
        /// The aircraft is approaching to land.
        /// </summary>
        Approach,
        /// <summary>
        /// The aircraft is about to land in day period.
        /// </summary>
        LandingDay,
        /// <summary>
        /// The aircraft is about to land in night period.
        /// </summary>
        LandingNight,
        /// <summary>
        /// The aircraft is on the runway after landing and before taxing in.
        /// </summary>
        Rollout,
        /// <summary>
        /// The aircraft is taxing in after landing or a rejected takeoff.
        /// </summary>
        TaxiIn,
        /// <summary>
        /// The aircraft is unboarding passengers.
        /// </summary>
        Unboarding,
        /// <summary>
        /// The aircraft is finished unboarding passengers.
        /// </summary>
        UnboardingDone
    }
}
