using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using XPDev.Foundation.Logging;

namespace XPDev.FlightManagement.Test
{
    [TestClass]
    public class FlightStateServiceTests
    {
        private Mock<IFlightSnapshotProvider> _flightSnapshotProviderMock;
        private Mock<IFlightStateManager> _flightStateManagerMock;

        private FlightStateService Target { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            LoggerManager.Initialize(new LoggerConfig("log.txt", LogLevel.Debug));

            _flightSnapshotProviderMock = new Mock<IFlightSnapshotProvider>();
            _flightStateManagerMock = new Mock<IFlightStateManager>();

            Target = new FlightStateService(_flightSnapshotProviderMock.Object, _flightStateManagerMock.Object);
        }

        [TestMethod]
        public async Task StateUpdatedEventIsRaieedWhenRunIsExecutedAndStateIsDifferent()
        {
            // Arrange
            var eventFired = false;

            _flightStateManagerMock.Setup(x => x.CurrentFlightState).Returns(FlightState.BoardingDone);
            Target.CurrentFlightState = FlightState.Boarding;
            Target.FlightStateChanged += delegate
            {
                eventFired = true;
            };

            // Act
            await Target.UpdateAsync();

            // Assert
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public async Task StateUpdatedEventIsNotRaieedWhenRunIsExecutedAndStateIsNotDifferent()
        {
            // Arrange
            var eventFired = false;

            _flightStateManagerMock.Setup(x => x.CurrentFlightState).Returns(FlightState.Approach);
            Target.CurrentFlightState = FlightState.Approach;
            Target.FlightStateChanged += delegate
            {
                eventFired = true;
            };

            // Act
            await Target.UpdateAsync();

            // Assert
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public async Task TakeSnapshotAndUpdateStateAreCalledWhenRunIsExecuted()
        {
            // Arrange
            var snapshot = new FlightSnapshot();

            _flightSnapshotProviderMock.Setup(x => x.TakeSnapshot()).Returns(snapshot);

            // Act
            await Target.UpdateAsync();

            // Assert
            _flightSnapshotProviderMock.Verify(x => x.TakeSnapshot(), Times.Once);
            _flightStateManagerMock.Verify(x => x.UpdateState(snapshot), Times.Once);
        }
    }
}
