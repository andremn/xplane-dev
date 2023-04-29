namespace XPDev.FlightManagement
{
    /// <summary>
    /// The possible states of a door.
    /// </summary>
    public enum DoorState
    {
        /// <summary>
        /// The door is completely closed.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// The door is opening.
        /// </summary>
        Opening,
        /// <summary>
        /// The door is completely open.
        /// </summary>
        Open,
        /// <summary>
        /// The door is closing.
        /// </summary>
        Closing
    }
}