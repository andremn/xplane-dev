using System;
using System.Threading.Tasks;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.Foundation;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Provides methods related to announcement sounds.
    /// </summary>
    internal class AnnouncementService : Disposable, IAnnouncementService
    {
        private readonly IAnnouncementManager _announcementManager;
        private readonly IFlightSnapshotProvider _snapshotProvider;

        private FlightSnapshot _lastSnapshot;
        private DateTime _lastSnapshotTime;
        private Task _currentProcessAnnouncementsTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementService"/> class with the /// specified <see cref="IAnnouncementService"/> and <see cref="IFlightSoundConfiguration"/>.
        /// </summary>
        /// <param name="announcementManager">An instance of the <see cref="IAnnouncementManager"/> interface.</param>
        /// <param name="snapshotProvider">An instance if the <see cref="IFlightSnapshotProvider"/> interface.</param>
        public AnnouncementService(IAnnouncementManager announcementManager, IFlightSnapshotProvider snapshotProvider)
        {
            _announcementManager = announcementManager;
            _snapshotProvider = snapshotProvider;

            _lastSnapshotTime = DateTime.MaxValue;
            _currentProcessAnnouncementsTask = Task.CompletedTask;
        }

        /// <summary>
        /// Gets or sets the time to wait before playing an announcement.
        /// </summary>
        public TimeSpan DelayToPlayAnnouncement
        {
            get => _announcementManager.DelayToPlayAnnouncement;
            set => _announcementManager.DelayToPlayAnnouncement = value;
        }

        /// <summary>
        /// Sets the path of a sound file that will be played in the specified <see cref="FlightState"/>.
        /// Different paths can be used for the same flight action. 
        /// In this case, the announcements will play one after the other in the other they were added.
        /// </summary>
        /// <param name="soundFilePath">The path of the sound file.</param>
        /// <param name="flightAction">The sound file to be played when the specified action happens.</param>
        public void SetAnnouncementSoundFileForFlightAction(string soundFilePath, FlightAction flightAction)
        {
            _announcementManager.SetAnnouncementSoundFileForFlightAction(soundFilePath, flightAction);
        }

        /// <summary>
        /// Plays scheduled announcements and check for changes in the flight state.
        /// </summary>
        public async Task UpdateAsync()
        {
            await Task.Run(() =>
            {
                if (_lastSnapshot == null || _snapshotProvider.LastUpdate > _lastSnapshotTime)
                {
                    _lastSnapshot = _snapshotProvider.TakeSnapshot();
                    _lastSnapshotTime = _snapshotProvider.LastUpdate;

                    _announcementManager.ProcessFlightSnapshot(_lastSnapshot);
                }

                if (_currentProcessAnnouncementsTask.IsCompleted)
                {
                    _currentProcessAnnouncementsTask = _announcementManager.ProcessAnnouncementsAsync();
                }
            });
        }
    }
}
