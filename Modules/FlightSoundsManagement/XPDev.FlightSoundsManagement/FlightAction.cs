namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// The possible actions of an airplane during a flight.
    /// </summary>
    public enum FlightAction
    {
        /// <summary>
        /// Aircraft oarding has started.
        /// </summary>
        BoardingStarted,
        /// <summary>
        /// Aircraft boarding has finished.
        /// </summary>
        BoardingFinished,
        /// <summary>
        /// Aircraft is ready for pushback.
        /// </summary>
        ReadyToPushback,
        /// <summary>
        /// Aircraft is cleared for taxi.
        /// </summary>
        ClearedToTaxi,
        /// <summary>
        /// Aircraft is cleared for takeoff.
        /// </summary>
        ClearedToTakeoff,
        /// <summary>
        /// Aircraft is climbing with seat belts sign on.
        /// </summary>
        ClimbingSeatBeltsOn,
        /// <summary>
        /// Aircraft is at cruise altitude.
        /// </summary>
        LevelOff,
        /// <summary>
        /// Aircraft has started the descent.
        /// </summary>
        DescentStarted,
        /// <summary>
        /// Aircraft is approaching to land.
        /// </summary>
        Approaching,
        /// <summary>
        /// Aircraft is cleared to land at day.
        /// </summary>
        ClearedToLandDay,
        /// <summary>
        /// Aircraft is cleared to land at night.
        /// </summary>
        ClearedToLandNight,
        /// <summary>
        /// Aircraft has landed and is taxing to gate.
        /// </summary>
        TaxingToGate,
        /// <summary>
        /// Aircraft has arrived at the gate.
        /// </summary>
        ArrivedAtGate,
        /// <summary>
        /// Aircraft unboarding has started.
        /// </summary>
        UnboardingStarted,
        /// <summary>
        /// Aircraft unboarding has finished.
        /// </summary>
        UnboardingFinished,
        /// <summary>
        /// Aircraft seat belts sign has been turned on.
        /// </summary>
        SeatBeltsSignTurnedOn,
        /// <summary>
        /// Aircraft seat belts sign has been turned off.
        /// </summary>
        SeatBeltsSignTurnedOff
    }
}
