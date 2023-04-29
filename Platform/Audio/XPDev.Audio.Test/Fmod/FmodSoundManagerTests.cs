using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using XPDev.Audio.Fmod;

namespace XPDev.Audio.Test
{
    [TestClass]
    public class FmodSoundManagerTests
    {
        private Mock<IFmodSystemFacade> FmodSystemMock { get; set; }

        private FmodSoundManager Target { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            FmodSystemMock = new Mock<IFmodSystemFacade>();
            Target = new FmodSoundManager(FmodSystemMock.Object);
        }

        [TestMethod]
        public void PlaySound_CalledOnce_CreateSoundAndPlaySoundIsCalledOnce()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            // Act
            Target.PlaySound(soundDescriptor);

            // Assert
            FmodSystemMock.Verify(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>()), Times.Once());
            FmodSystemMock.Verify(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>()), Times.Once());
        }

        [TestMethod]
        public void PlaySound_CalledTwice_CreateSoundIsCalledOceAndPlaySoundIsCalledTwice()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.PlaySound(soundDescriptor);

            // Assert
            FmodSystemMock.Verify(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>()), Times.Exactly(2));
            FmodSystemMock.Verify(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>()), Times.Once());
        }

        [TestMethod]
        public void ResumeSound_SoundIsCreatedAndPlaying_PausedIsSetToFalse()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.ResumeSound(soundDescriptor);

            // Assert
            fmodChannelFacade.VerifySet(x => x.Paused = false);
        }

        [TestMethod]
        public void ResumeSound_SoundIsCreatedAndPaused_PausedIsSetToFalse()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);
            Target.PauseSound(soundDescriptor);

            // Act
            Target.ResumeSound(soundDescriptor);

            // Assert
            fmodChannelFacade.VerifySet(x => x.Paused = false);
        }

        [TestMethod]
        public void ResumeSound_SoundIsNotCreated_PausedIsNotSet()
        {
            // Arrange
            var soundDescriptor = new SoundDescriptor("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            // Act
            Target.ResumeSound(soundDescriptor);

            // Assert
            fmodChannelFacade.VerifySet(x => x.Paused = It.IsAny<bool>(), Times.Never());
        }

        [TestMethod]
        public void PauseSound_SoundIsCreatedAndPlaying_PausedIsSetToTrue()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.PauseSound(soundDescriptor);

            // Assert
            fmodChannelFacade.VerifySet(x => x.Paused = true);
        }

        [TestMethod]
        public void PauseSound_SoundIsNotCreated_PausedIsNotSet()
        {
            // Arrange
            var soundDescriptor = new SoundDescriptor("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            // Act
            Target.PauseSound(soundDescriptor);

            // Assert
            fmodChannelFacade.VerifySet(x => x.Paused = It.IsAny<bool>(), Times.Never());
        }

        [TestMethod]
        public void StopSound_SoundIsCreatedAndPlaying_StopSoundIsCalled()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.StopSound(soundDescriptor);

            // Assert
            fmodChannelFacade.Verify(x => x.Stop(), Times.Once());
        }

        [TestMethod]
        public void StopSound_SoundIsNotCreated_StopSoundIsNotCalled()
        {
            // Arrange
            var soundDescriptor = new SoundDescriptor("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            // Act
            Target.StopSound(soundDescriptor);

            // Assert
            fmodChannelFacade.Verify(x => x.Stop(), Times.Never());
        }

        [TestMethod]
        public void Run_SoundIsInitializedAndNotPlaying_ReleaseIsCalled()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            fmodChannelFacade.SetupGet(x => x.IsPlaying).Returns(false);

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.Run();

            // Assert
            fmodSoundFacade.Verify(x => x.Release(), Times.Once());
        }

        [TestMethod]
        public void Run_SoundIsInitializedAndPlaying_ReleaseIsNotCalled()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            fmodChannelFacade.SetupGet(x => x.IsPlaying).Returns(true);

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            Target.PlaySound(soundDescriptor);

            // Act
            Target.Run();

            // Assert
            fmodSoundFacade.Verify(x => x.Release(), Times.Never());
        }

        [TestMethod]
        public void Run_SoundIsNotInitializedAndNotPlaying_ReleaseIsNotCalled()
        {
            // Arrange
            var soundDescriptor = Target.GetOrCreateSound("path/to/file");
            var fmodSoundFacade = new Mock<IFmodSoundFacade>();
            var fmodChannelFacade = new Mock<IFmodChannelFacade>();

            fmodChannelFacade.SetupGet(x => x.IsPlaying).Returns(false);

            FmodSystemMock.Setup(x => x.CreateSound(soundDescriptor.Path, It.IsAny<FMOD.MODE>())).Returns(fmodSoundFacade.Object);
            FmodSystemMock.Setup(x => x.PlaySound(fmodSoundFacade.Object, It.IsAny<bool>())).Returns(fmodChannelFacade.Object);

            // Act
            Target.Run();

            // Assert
            fmodSoundFacade.Verify(x => x.Release(), Times.Never());
        }
    }
}
