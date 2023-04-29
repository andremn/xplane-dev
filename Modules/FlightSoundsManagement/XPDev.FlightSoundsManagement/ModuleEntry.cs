using System;
using System.Runtime.InteropServices;
using XPDev.Audio;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.Foundation.Logging;
using XPDev.Modularization;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// The entry point of the module.
    /// </summary>
    public class ModuleEntry : ModuleEntryBase
    {
        private readonly ILogger _logger;

        private IntPtr _fmodDllHandle;

        /// <summary>
        /// Initialize a new instance of the <see cref="ModuleEntry"/> class with the specified name.
        /// </summary>
        public ModuleEntry() : base("FlightSoundsManagement")
        {
            _logger = LoggerManager.GetLoggerFor<ModuleEntry>();
        }

        private IAircraftSoundsService AircraftSoundsService => GetService<IAircraftSoundsService>();

        private IAnnouncementService AnnouncementService => GetService<IAnnouncementService>();

        private IFlightSoundConfigurationProvider SoundConfigurationProvider => GetService<IFlightSoundConfigurationProvider>();

        /// <summary>
        /// Updates this module.
        /// </summary>
        public override void Run()
        {
            AircraftSoundsService.UpdateAsync();
            AnnouncementService.UpdateAsync();
        }

        /// <summary>
        /// Registers the services of this module.
        /// </summary>
        protected override void OnRegisterServices()
        {
            RegisterService(new SoundManagerFactory().CreateSoundManager(SoundManagerEngine.Fmod));
            RegisterService<IAnnouncementManager, AnnouncementManager>();
            RegisterService<IAircraftSoundsService, AircraftSoundsService>();
            RegisterService<IAnnouncementService, AnnouncementService>();
        }

        /// <summary>
        /// Called when the module starts.
        /// </summary>
        protected override void OnStarted()
        {
            var configuration = SoundConfigurationProvider.GetFlightSoundConfiguration();

            if (configuration is FmodSoundConfiguration fmodConfiguration)
            {
                LoadFmodDll(fmodConfiguration);
            }
        }

        /// <summary>
        /// Called when the module stops.
        /// </summary>
        protected override void OnStopped()
        {
            if (_fmodDllHandle != IntPtr.Zero)
            {
                NativeLibrary.Free(_fmodDllHandle);
            }
        }

        private void LoadFmodDll(FmodSoundConfiguration configuration)
        {
            _logger.Debug("Loading Fmod dll...");

            try
            {
                _fmodDllHandle = NativeLibrary.Load(configuration.FmodDllPath);
                _logger.Info("Fmod loaded");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, $"Unable to load Fmod dll");
            }
        }
    }
}
