using System.IO;
using XPDev.FlightSoundsManagement;
using XPDev.Foundation;
using XPDev.Foundation.Logging;

namespace XPDev.ExtensionPack
{
    internal class PluginService
    {
        private const string DefaultSoundsFolderName = "Sounds";
        private const string CabinSecureAnnouncementFileName = "cabin_secure.wav";
        private const string WelcomeAnnouncementFileName = "welcome.wav";
        private const string SafetyAnnouncementFileName = "safety.wav";
        private const string ClimbAnnouncementFileName = "climb.wav";
        private const string DescentAnnouncementFileName = "descent.wav";
        private const string LandingDayAnnouncementFileName = "landing_day.wav";
        private const string LandingNightAnnouncementFileName = "landing_night.wav";
        private const string UnboardingAnnouncementFileName = "unboarding.wav";
        private const string UnboardingDoneAnnouncementFileName = "unboarding_done.wav";
        private const string SeatBeltsOnAnnouncementFileName = "seat_belts_on.wav";
        private const string SeatBeltsOffAnnouncementFileName = "seat_belts_off.wav";

        private readonly IAnnouncementService _announcementService;
        private readonly ILogger _logger;

        public PluginService(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
            _logger = LoggerManager.GetLoggerFor<PluginService>();
        }

        public void Initialize(PluginContext pluginContext)
        {
            pluginContext.NotNull();

            var announcementsConfiguration = pluginContext.PluginConfiguration?.AnnouncementsConfigurationSection;
            var announcementsFolder = announcementsConfiguration?.SoundsFolderPath ?? Path.Combine(pluginContext.PluginFolder, DefaultSoundsFolderName);

            if (announcementsConfiguration?.LiveriesPaths != null)
            {
                if (announcementsConfiguration.LiveriesPaths.TryGetValue(pluginContext.CurrentAircraftLiveryFolder, out var folderPath))
                {
                    announcementsFolder = folderPath;
                }
            }

            _logger.Info($"Announcements folder set to {announcementsFolder}");

            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, CabinSecureAnnouncementFileName), FlightAction.BoardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, WelcomeAnnouncementFileName), FlightAction.BoardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, SafetyAnnouncementFileName), FlightAction.ReadyToPushback);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, ClimbAnnouncementFileName), FlightAction.ClimbingSeatBeltsOn);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, DescentAnnouncementFileName), FlightAction.DescentStarted);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, LandingDayAnnouncementFileName), FlightAction.ClearedToLandDay);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, LandingNightAnnouncementFileName), FlightAction.ClearedToLandNight);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, UnboardingAnnouncementFileName), FlightAction.UnboardingStarted);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, UnboardingDoneAnnouncementFileName), FlightAction.UnboardingFinished);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, SeatBeltsOnAnnouncementFileName), FlightAction.SeatBeltsSignTurnedOn);
            _announcementService.SetAnnouncementSoundFileForFlightAction(Path.Combine(announcementsFolder, SeatBeltsOffAnnouncementFileName), FlightAction.SeatBeltsSignTurnedOff);
        }
    }
}
