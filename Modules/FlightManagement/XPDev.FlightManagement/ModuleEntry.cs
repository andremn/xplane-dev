using XPDev.Modularization;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// The entry point of the module.
    /// </summary>
    public class ModuleEntry : ModuleEntryBase
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ModuleEntry"/> class with the specified name.
        /// </summary>
        public ModuleEntry() : base("FlightManagement")
        {
        }

        private IFlightStateService FlightStateService => GetService<IFlightStateService>();

        /// <summary>
        /// Updates this module.
        /// </summary>
        public override void Run()
        {
            FlightStateService.UpdateAsync();
        }

        /// <summary>
        /// Registers the services of this module.
        /// </summary>
        protected override void OnRegisterServices()
        {
            RegisterService<IFlightStateManager, FlightStateManager>();
            RegisterService<IFlightStateService, FlightStateService>();
        }

        /// <summary>
        /// Called when the module starts.
        /// </summary>
        protected override void OnStarted()
        {
        }

        /// <summary>
        /// Called when the module stops.
        /// </summary>
        protected override void OnStopped()
        {
        }
    }
}
