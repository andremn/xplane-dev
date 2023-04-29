using XPDev.FlightSoundsSimulator.Model;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement;
using System.Threading.Tasks;
using System.Threading;

namespace XPDev.FlightSoundsSimulator
{
    public class FlightParameterRequestProcessor : IFlightParameterRequestProcessor
    {
        private readonly IFlightParametersService _flightParametersService;
        private readonly IFlightStateService _flightStateService;
        private readonly IAnnouncementService _announcementService;
        private readonly AutoResetEvent _autoResetEvent;

        public FlightParameterRequestProcessor(
            IFlightParametersService flightParametersService,
            IFlightStateService flightStateService,
            IAnnouncementService announcementService)
        {
            _flightParametersService = flightParametersService;
            _flightStateService = flightStateService;
            _announcementService = announcementService;
            _autoResetEvent = new AutoResetEvent(false);

            _flightStateService.FlightStateChanged += OnFlightStateChanged;

            InitializeSounds();
        }

        public Task<FlightState> ProcessRequestAsync(FlightParameters parameter)
        {
            return Task.Run(() =>
            {
                _flightParametersService.UpdateFlightParameters(parameter);

                // Wait up to 1 second, which is the default update interval of the modules
                _autoResetEvent.WaitOne(1000);

                return _flightStateService.CurrentFlightState;
            });
        }

        private void InitializeSounds()
        {
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/cabin_secure.wav", FlightAction.BoardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/welcome.wav", FlightAction.BoardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/safety.wav", FlightAction.ReadyToPushback);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/climb.wav", FlightAction.ClimbingSeatBeltsOn);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/descent.wav", FlightAction.DescentStarted);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/landing_day.wav", FlightAction.ClearedToLandDay);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/landing_night.wav", FlightAction.ClearedToLandNight);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/unboarding.wav", FlightAction.UnboardingStarted);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/unboarding_done.wav", FlightAction.UnboardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/seat_belts_on.wav", FlightAction.SeatBeltsSignTurnedOn);
            _announcementService.SetAnnouncementSoundFileForFlightAction("Sounds/seat_belts_off.wav", FlightAction.SeatBeltsSignTurnedOff);
        }

        private void OnFlightStateChanged(object sender, FlightStateChangedEventArgs e)
        {
            // To-do: remove this and use websockets to notify the state change.
            _autoResetEvent.Set();
        }
    }
}
