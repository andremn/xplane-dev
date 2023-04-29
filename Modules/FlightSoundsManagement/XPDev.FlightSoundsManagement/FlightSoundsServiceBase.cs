using System;
using System.Numerics;
using System.Threading.Tasks;
using XPDev.Audio;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.Foundation;
using XPDev.Modularization;

namespace XPDev.FlightSoundsManagement
{
    internal abstract class FlightSoundsServiceBase : Disposable, IModuleService
    {
        protected readonly IFlightSnapshotProvider _snapshotProvider;
        protected readonly ISoundManager _soundManager;
        protected readonly IFlightSoundConfiguration _soundConfiguration;

        protected FlightSnapshot _lastSnapshot;
        protected DateTime _lastSnapshotDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightSoundsServiceBase"/> class with the specified 
        /// <see cref="IFlightSnapshotProvider"/>, <see cref="ISoundManager"/> and <see cref="IFlightSoundConfiguration"/>.
        /// </summary>
        /// <param name="snapshotProvider">The <see cref="IFlightSnapshotProvider"/> to get flight info from.</param>
        /// <param name="soundManager">The <see cref="ISoundManager"/> to manage sounds.</param>
        /// <param name="soundConfiguration">The <see cref="IFlightSoundConfiguration"/> to be used.</param>
        protected FlightSoundsServiceBase(IFlightSnapshotProvider snapshotProvider, ISoundManager soundManager, IFlightSoundConfiguration soundConfiguration)
        {
            snapshotProvider.NotNull();
            soundManager.NotNull();
            soundConfiguration.NotNull();

            _snapshotProvider = snapshotProvider;
            _soundManager = soundManager;
            _soundConfiguration = soundConfiguration;
        }

        /// <summary>
        /// Updates the service asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateAsync()
        {
            if (_lastSnapshot == null || _lastSnapshotDateTime != _snapshotProvider.LastUpdate)
            {
                _lastSnapshot = _snapshotProvider.TakeSnapshot();
                _lastSnapshotDateTime = _snapshotProvider.LastUpdate;
                _soundManager.ListenerPosition = _lastSnapshot?.CameraLocation?.Position ?? Vector3.Zero;
            }

            await OnUpdateAsync();            
            _soundManager.Run();
        }

        /// <summary>
        /// Called after the last snapshot was taken and before the sound manager is updated.
        /// </summary>
        /// <returns></returns>
        protected abstract Task OnUpdateAsync();
    }
}
