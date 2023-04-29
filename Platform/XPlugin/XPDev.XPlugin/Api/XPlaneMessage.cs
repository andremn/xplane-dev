namespace XPDev.XPlugin
{
    /// <summary>
    /// The messages sent by X-Plane to plugins.
    /// </summary>
    public enum XPlaneMessage : int
    {
        /// <summary>
        /// This message is sent whenever the user’s plane crashes. The parameter is ignored.
        /// </summary>
        PlaneCrashed = 101,
        /// <summary>
        /// This message is sent whenever a new plane is loaded. The parameter contains the index number of the plane being loaded; 0 indicates the user’s plane.
        /// </summary>
        PlaneLoaded = 102,
        /// <summary>
        /// This messages is sent whenever the user’s plane is positioned at a new airport. The parameter is ignored.
        /// </summary>
        AirportLoaded = 103,
        /// <summary>
        /// This message is sent whenever new scenery is loaded. Use datarefs to determine the new scenery files that were loaded. The parameter is ignored.
        /// </summary>
        SceneryLoaded = 104,
        /// <summary>
        /// This message is sent whenever the user adjusts the number of X-Plane aircraft models. The parameter is ignored.
        /// </summary>
        AirplaneCountChanged = 105,
        /// <summary>
        /// This message is sent whenever a plane is unloaded. The parameter contains the index number of the plane being unloaded; 0 indicates the user’s plane. The parameter is of type int, passed as the value of the pointer.
        /// </summary>
        PlaneUnloaded = 106,
        /// <summary>
        /// This message is sent right before X-Plane writes its preferences file. The parameter is ignored.
        /// </summary>
        BeforeWritePreferences = 107,
        /// <summary>
        /// This message is sent right after a livery is loaded for an airplane. The parameter contains the index number of the aircraft whose livery is changing.
        /// </summary>
        PlaneLiveryLoaded = 108,
        /// <summary>
        /// This message is sent right before X-Plane enters virtual reality mode (at which time any windows that are not positioned in VR mode will no longer be visible to the user). The parameter is ignored.
        /// </summary>
        EnteringVR = 109,
        /// <summary>
        /// This message is sent right before X-Plane leaves virtual reality mode (at which time you may want to clean up windows that are positioned in VR mode). The parameter is ignored.
        /// </summary>
        ExitingVR = 110,
        /// <summary>
        /// This message is sent if another plugin wants to take over AI planes. If you are a synthetic traffic provider, that probably means a plugin for an online network has connected and wants to supply aircraft flown by real humans and you should cease to provide synthetic traffic. If however you are providing online traffic from real humans, you probably don’t want to disconnect, in which case you just ignore this message. The sender is the plugin ID of the plugin asking for control of the planes now. You can use it to find out who is requesting and whether you should yield to them. Synthetic traffic providers should always yield to online networks. The parameter is ignored.
        /// </summary>
        ReleasePlanes = 111
    }
}
