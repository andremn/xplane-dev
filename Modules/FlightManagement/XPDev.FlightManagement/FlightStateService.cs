using System;
using System.Threading.Tasks;
using XPDev.Foundation;
using XPDev.Foundation.Logging;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// Provides methods related to flight state operations.
    /// </summary>
    internal class FlightStateService : Disposable, IFlightStateService
    {
        private readonly IFlightSnapshotProvider _flightSnapshotProvider;
        private readonly IFlightStateManager _flightStateManager;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightStateService"/> class.
        /// </summary>
        /// <param name="flightSnapshotProvider">The <see cref="IFlightSnapshotProvider"/> to be used by this <see cref="FlightStateService"/>.</param>
        /// <param name="flightStateManager">The <see cref="IFlightStateManager"/> to be used by this <see cref="FlightStateService"/>.</param>
        public FlightStateService(IFlightSnapshotProvider flightSnapshotProvider, IFlightStateManager flightStateManager)
        {
            _flightSnapshotProvider = flightSnapshotProvider;
            _flightStateManager = flightStateManager;
            _logger = LoggerManager.GetLoggerFor<FlightStateService>();

            CurrentFlightState = FlightState.Parked;
        }

        /// <summary>
        /// Occurs when the current flight state changes to another state.
        /// </summary>
        public event EventHandler<FlightStateChangedEventArgs> FlightStateChanged;

        /// <summary>
        /// Gets the current flight state.
        /// </summary>
        public FlightState CurrentFlightState { get; internal set; }

        /// <summary>
        /// Updates the current state of the flight.
        /// </summary>
        public Task UpdateAsync()
        {
            return Task.Run(() =>
            {
                var flightSnapshot = _flightSnapshotProvider.TakeSnapshot();

                _flightStateManager.UpdateState(flightSnapshot);

                SetState(_flightStateManager.CurrentFlightState);
            });
        }

        private void SetState(FlightState flightState)
        {
            if (CurrentFlightState != flightState)
            {
                _logger.Info($"Current flight changed from '{CurrentFlightState}' to '{flightState}'");

                CurrentFlightState = flightState;                

                var handler = FlightStateChanged;

                if (handler != null)
                {
                    var currentFlightState = CurrentFlightState;

                    foreach (EventHandler<FlightStateChangedEventArgs> eventHandler in handler.GetInvocationList())
                    {
                        try
                        {
                            eventHandler?.Invoke(this, new FlightStateChangedEventArgs(currentFlightState, flightState));
                        }
                        catch (Exception ex)
                        {
                            _logger.Warning(ex, $"Unable to invoke event handler '{eventHandler?.Method?.Name}'");
                        }
                    }
                }
            }
        }
    }
}
