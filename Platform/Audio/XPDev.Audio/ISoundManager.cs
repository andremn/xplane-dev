using System;
using System.Numerics;
using XPDev.Foundation;

namespace XPDev.Audio
{
    /// <summary>
    /// Represents a manager for sounds.
    /// </summary>
    public interface ISoundManager : IDisposable, IRunnable
    {
        /// <summary>
        /// Gets or sets the position of the listener.
        /// </summary>
        Vector3 ListenerPosition { get; set; }

        /// <summary>
        /// Gets or creates a sound for the given file path and returns a descriptor for it.
        /// </summary>
        /// <param name="path">The path of the sound file to be created.</param>
        /// <returns>An instance of <see cref="ISoundDescriptor"/>.</returns>
        ISoundDescriptor GetOrCreateSound(string path);

        /// <summary>
        /// Plays the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to play the related sound.</param>
        void PlaySound(ISoundDescriptor soundDescriptor);

        /// <summary>
        /// Plays the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to play the related sound.</param>
        /// <param name="repeatTimes">The repeat of the sound.</param>
        void PlaySound(ISoundDescriptor soundDescriptor, SoundRepeatTimes repeatTimes);

        /// <summary>
        /// Resumes playing the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to resume the related sound.</param>
        void ResumeSound(ISoundDescriptor soundDescriptor);

        /// <summary>
        /// Pauses the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to pause the related sound.</param>
        void PauseSound(ISoundDescriptor soundDescriptor);

        /// <summary>
        /// Stops the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to stops the related sound.</param>
        void StopSound(ISoundDescriptor soundDescriptor);

        /// <summary>
        /// Gets whether the specified sound is playing.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to verify if the associated sound is playing.</param>
        /// <returns>True if the sound is playing; false otherwise.</returns>
        bool IsPlaying(ISoundDescriptor soundDescriptor);
    }
}
