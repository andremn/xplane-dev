using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using XPDev.Audio;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;

namespace XPDev.FlightSoundsManagement.Test
{
    [TestClass]
    public class AircraftSoundsServiceTest
    {
        private const int TimeoutToWaitForPlaySoundCalled = 1000;

        private Mock<IFlightStateService> FlightServiceMock { get; set; }

        private Mock<IFlightSnapshotProvider> FlightSnapshotProviderMock { get; set; }

        private Mock<ISoundManager> SoundManagerMock { get; set; }

        private Mock<IFlightSoundConfigurationProvider> SoundConfigurationProviderMock { get; set; }

        private Mock<IFlightSoundConfiguration> SoundConfigurationMock { get; set; }

        private AircraftSoundsService Target { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            FlightServiceMock = new Mock<IFlightStateService>();
            FlightSnapshotProviderMock = new Mock<IFlightSnapshotProvider>();
            SoundManagerMock = new Mock<ISoundManager>();
            SoundConfigurationProviderMock = new Mock<IFlightSoundConfigurationProvider>();
            SoundConfigurationMock = new Mock<IFlightSoundConfiguration>();

            SoundConfigurationProviderMock.Setup(p => p.GetFlightSoundConfiguration()).Returns(SoundConfigurationMock.Object);

            Target = new AircraftSoundsService(FlightSnapshotProviderMock.Object, SoundManagerMock.Object, SoundConfigurationProviderMock.Object);
        }
    }
}
