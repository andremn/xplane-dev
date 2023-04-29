using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using XPDev.Audio;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;

namespace XPDev.FlightSoundsManagement.Test
{
    [TestClass]
    public class AnnouncementManagerTest
    {
        private Mock<IFlightStateService> FlightServiceMock { get; set; }

        private Mock<ISoundManager> SoundManagerMock { get; set; }

        private Mock<IFlightSoundConfigurationProvider> SoundConfigurationProviderMock { get; set; }

        private Mock<IFlightSoundConfiguration> SoundConfigurationMock { get; set; }

        private AnnouncementManager Target { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            FlightServiceMock = new Mock<IFlightStateService>();
            SoundManagerMock = new Mock<ISoundManager>();
            SoundConfigurationProviderMock = new Mock<IFlightSoundConfigurationProvider>();
            SoundConfigurationMock = new Mock<IFlightSoundConfiguration>();

            SoundConfigurationProviderMock.Setup(p => p.GetFlightSoundConfiguration()).Returns(SoundConfigurationMock.Object);

            Target = new AnnouncementManager(FlightServiceMock.Object, SoundManagerMock.Object, SoundConfigurationProviderMock.Object);
        }

        [TestMethod]
        public void FlightStateChanged_GetOrCreateSoundIsCalledWithPathToSoundFile()
        {
            // Arrange
            var soundDescriptorMock = new Mock<ISoundDescriptor>();

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file", FlightAction.BoardingFinished);

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file")).Returns(soundDescriptorMock.Object);
            SoundManagerMock.Setup(x => x.IsPlaying(soundDescriptorMock.Object)).Returns(true);

            // Act
            FlightServiceMock.Raise(x => x.FlightStateChanged += null, new FlightStateChangedEventArgs(FlightState.Boarding, FlightState.BoardingDone));

            // Assert
            SoundManagerMock.Verify(x => x.GetOrCreateSound("path/to/file"), Times.Once());
        }

        [TestMethod]
        public void FlightStateChanged_PlaySoundIsCalledWhenFlightStateChanges()
        {
            // Arrange
            using var resetEvent = new AutoResetEvent(false);
            var soundDescriptorMock = new Mock<ISoundDescriptor>();

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file", FlightAction.ClearedToLandDay);

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file")).Returns(soundDescriptorMock.Object);
            SoundManagerMock.Setup(x => x.IsPlaying(soundDescriptorMock.Object)).Returns(true);

            // Act
            FlightServiceMock.Raise(x => x.FlightStateChanged += null, new FlightStateChangedEventArgs(FlightState.Approach, FlightState.LandingDay));

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, SoundRepeatTimes.Once), Times.Once());
        }

        [TestMethod]
        public async Task UpdateAsync_FlightStateChangedToClimbAndAltitudeIsGreaterThan23000Feet_PlaySoundIsCalledWhenProcessAnnouncementsIsCalled()
        {
            // Arrange
            using var resetEvent = new AutoResetEvent(false);
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var cancellationToken = new CancellationTokenSource();

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file")).Returns(soundDescriptorMock.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file", FlightAction.ClimbingSeatBeltsOn);

            FlightServiceMock.Setup(x => x.CurrentFlightState).Returns(FlightState.Climb);

            StartGameLoopTask(new FlightSnapshot { AircraftIndicatedAltitute = 23000 }, cancellationToken.Token);

            // Act
            await Target.ProcessAnnouncementsAsync();

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, SoundRepeatTimes.Once), Times.Once());

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_FlightStateChangedToClimbAndAltitudeIsGreaterThan23000Feet_PlaySoundIsNotCalledWhenRunIsCalledSecondTime()
        {
            // Arrange
            using var resetEvent = new AutoResetEvent(false);
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var cancellationToken = new CancellationTokenSource();

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file")).Returns(soundDescriptorMock.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file", FlightAction.ClimbingSeatBeltsOn);

            FlightServiceMock.Setup(x => x.CurrentFlightState).Returns(FlightState.Climb);

            StartGameLoopTask(new FlightSnapshot { AircraftIndicatedAltitute = 23000 }, cancellationToken.Token);

            // Act
            await Target.ProcessAnnouncementsAsync();
            Thread.Sleep(100);
            await Target.ProcessAnnouncementsAsync();

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, SoundRepeatTimes.Once), Times.Once());

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_SeatBeltsSignChangedToOn_PlaySoundIsCalledOnce()
        {
            // Arrange
            using var resetEvent = new AutoResetEvent(false);
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var cancellationToken = new CancellationTokenSource();
            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 20000,
                AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.Off
            };

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file1")).Returns(soundDescriptorMock.Object);
            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file2")).Returns(soundDescriptorMock.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file1", FlightAction.SeatBeltsSignTurnedOn);
            Target.SetAnnouncementSoundFileForFlightAction("path/to/file2", FlightAction.SeatBeltsSignTurnedOff);

            StartGameLoopTask(snapshot, cancellationToken.Token);

            // Act
            snapshot.AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On;
            await Task.Delay(150);
            await Target.ProcessAnnouncementsAsync();

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, SoundRepeatTimes.Forever), Times.Once());

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_SeatBeltsSignWasOnAndChangedToOff_PlaySoundIsCalledTwice()
        {
            // Arrange
            using var resetEvent = new AutoResetEvent(false);
            var soundDescriptorMock1 = new Mock<ISoundDescriptor>();
            var soundDescriptorMock2 = new Mock<ISoundDescriptor>();
            var cancellationToken = new CancellationTokenSource();
            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 20000,
                AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On
            };

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file1")).Returns(soundDescriptorMock1.Object);
            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file2")).Returns(soundDescriptorMock2.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file1", FlightAction.SeatBeltsSignTurnedOn);
            Target.SetAnnouncementSoundFileForFlightAction("path/to/file2", FlightAction.SeatBeltsSignTurnedOff);

            StartGameLoopTask(snapshot, cancellationToken.Token);

            // Act
            snapshot.AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.Off;
            await Task.Delay(150);
            await Target.ProcessAnnouncementsAsync();
            snapshot.AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On;
            await Task.Delay(150);
            await Target.ProcessAnnouncementsAsync();

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock1.Object, SoundRepeatTimes.Forever), Times.Once());
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock2.Object, SoundRepeatTimes.Forever), Times.Once());

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_ExternalViewIsTrue_VolumeIsZero()
        {
            // Arrange
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var isExternalView = true;
            var cancellationToken = new CancellationTokenSource();
            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 2000f,
                CameraLocation = new CameraLocation(new Vector3(0, 0, 0), isExternalView),
                AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On
            };

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file1")).Returns(soundDescriptorMock.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file1", FlightAction.SeatBeltsSignTurnedOn);

            StartGameLoopTask(snapshot, cancellationToken.Token);

            // Act
            await Target.ProcessAnnouncementsAsync();

            // Assert
            soundDescriptorMock.VerifySet(x => x.Volume = 0f);
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, It.IsAny<SoundRepeatTimes>()));

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_ExternalViewIsFalse_VolumeIsNotZero()
        {
            // Arrange
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var isExternalView = false;
            var cancellationToken = new CancellationTokenSource();
            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 2000f,
                CameraLocation = new CameraLocation(new Vector3(0, 0, 0), isExternalView),
                AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On
            };

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file1")).Returns(soundDescriptorMock.Object);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file1", FlightAction.SeatBeltsSignTurnedOn);

            StartGameLoopTask(snapshot, cancellationToken.Token);

            // Act
            await Target.ProcessAnnouncementsAsync();

            // Assert
            soundDescriptorMock.VerifySet(x => x.Volume = 100f);
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, It.IsAny<SoundRepeatTimes>()));

            cancellationToken.Cancel();
        }

        [TestMethod]
        public async Task UpdateAsync_PausedChangedToTrueThenFalse_SoundIsPausedAndResumed()
        {
            // Arrange
            var soundDescriptorMock = new Mock<ISoundDescriptor>();
            var cancellationToken = new CancellationTokenSource();
            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 2000f,
                AircraftSeatBeltsSignSwitchPosition = SeatBeltsSignSwtichPosition.On
            };

            SoundManagerMock.Setup(x => x.GetOrCreateSound("path/to/file1")).Returns(soundDescriptorMock.Object);
            SoundManagerMock.Setup(x => x.IsPlaying(soundDescriptorMock.Object)).Returns(true);

            Target.SetAnnouncementSoundFileForFlightAction("path/to/file1", FlightAction.SeatBeltsSignTurnedOn);

            StartGameLoopTask(snapshot, cancellationToken.Token);

            // Do not await here since it will not return until the sound finishes playing after resuming it
            _ = Target.ProcessAnnouncementsAsync();

            // Act
            snapshot.IsPaused = true;
            await Task.Delay(150);
            snapshot.IsPaused = false;
            await Task.Delay(150);

            // Assert
            SoundManagerMock.Verify(x => x.PlaySound(soundDescriptorMock.Object, It.IsAny<SoundRepeatTimes>()), Times.Once());
            SoundManagerMock.Verify(x => x.PauseSound(soundDescriptorMock.Object), Times.Once());
            SoundManagerMock.Verify(x => x.ResumeSound(soundDescriptorMock.Object), Times.Once());

            cancellationToken.Cancel();
        }

        private void StartGameLoopTask(FlightSnapshot flightSnapshot, CancellationToken cancellationToken)
        {
            Target.ProcessFlightSnapshot(flightSnapshot);

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(50);
                    Target.ProcessFlightSnapshot(flightSnapshot);
                }
            }, cancellationToken);
        }
    }
}
