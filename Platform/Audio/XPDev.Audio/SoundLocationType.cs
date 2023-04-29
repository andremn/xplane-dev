namespace XPDev.Audio
{
    /// <summary>
    /// The possible locations of a sound.
    /// </summary>
    public enum SoundLocationType
    {
        /// <summary>
        /// The sound is played only inside cockpit.
        /// </summary>
        CockpitOnly,
        /// <summary>
        /// The sound is played only inside passenger cabin.
        /// </summary>
        PassengerCabinOnly,
        /// <summary>
        /// The sound is played inside cockpit and passenger cabin.
        /// </summary>
        Both
    }
}
