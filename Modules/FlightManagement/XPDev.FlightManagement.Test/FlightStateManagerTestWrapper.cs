namespace XPDev.FlightManagement.Test
{
    internal class FlightStateManagerTestWrapper : FlightStateManager
    {
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        public new void SetState(FlightState flightState)
        {
            base.SetState(flightState);
        }

        public new void TryChangeToNextState(FlightState nextFlightState)
        {
            base.TryChangeToNextState(nextFlightState);
        }
    }
}
