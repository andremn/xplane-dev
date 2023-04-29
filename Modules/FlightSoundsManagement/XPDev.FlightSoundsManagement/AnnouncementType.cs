namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// The possible types of an anouncement.
    /// </summary>
    public enum AnnouncementType
    {
        /// <summary>
        /// The announcement is from cockpit crew to the cabin crew.
        /// </summary>
        CockpitToCrew,
        /// <summary>
        /// The announcement is from the cabin crew to cockpit crew.
        /// </summary>
        CrewToCockpit,
        /// <summary>
        /// The announcement is from cabin crew to passenger cabin.
        /// </summary>
        CrewToCabin
    }
}
