namespace XPDev.FlightManagement
{
    /// <summary>
    /// Flight management related constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Min taxi speed, in knots.
        /// </summary>
        public const float MinTaxiSpeed = 10f;

        /// <summary>
        /// Min takeoff speed, in knots.
        /// </summary>
        public const float MinTakeoffSpeed = 40f;

        /// <summary>
        /// Min vertical speed to indicate climb, in knots.
        /// </summary>
        public const float MinClimbVerticalSpeed = 500;

        /// <summary>
        /// Min vertical speed to indicate descent, in knots.
        /// </summary>
        public const float MinDescentVerticalSpeed = -MinClimbVerticalSpeed;

        /// <summary>
        /// Tolerance to cruise altitude, in feet.
        /// </summary>
        public const float CruiseToleranceAltitude = 1000;

        /// <summary>
        /// Limitation altitude, in feet.
        /// </summary>
        public const float LimitationAltitude = 10000;
    }
}
