using System;
using System.Numerics;
using XPDev.Audio;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Represents a flight sound.
    /// </summary>
    public class FlightSound
    {
        /// <summary>
        /// Gets the max volume of a sound.
        /// </summary>
        public const float MaxVolume = 100f;

        /// <summary>
        /// Gets the max occlusion of a sound.
        /// </summary>
        public const float MaxOcclusion = 1f;

        /// <summary>
        /// Gets the min volume of a sound.
        /// </summary>
        public const float MinVolume = 0f;

        /// <summary>
        /// Gets the min occlusion of a sound.
        /// </summary>
        public const float MinOcclusion = 0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightSound"/> class with the specified file path, position and number of repeats.
        /// </summary>
        /// <param name="filePath">The path of the sound file.</param>
        /// <param name="repeatTimes">The repeat behaviour of this sound.</param>
        public FlightSound(string filePath, SoundRepeatTimes repeatTimes)
        {
            FilePath = filePath;
            RepeatTimes = repeatTimes;
        }

        /// <summary>
        /// Gets the path of the sound file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets how this sound repeats.
        /// </summary>
        public SoundRepeatTimes RepeatTimes { get; }

        /// <summary>
        /// Gets or sets the location of the sound.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the volume of the sound.
        /// </summary>
        public float Volume { get; set; } = MaxVolume;

        /// <summary>
        /// Gets or sets the occlusion of the sound.
        /// </summary>
        public float Occlusion { get; set; } = MinOcclusion;

        /// <summary>
        /// Gets or sets the condition to play the sound.
        /// </summary>
        public Func<bool> ConditionToPlay { get; set; }

        /// <summary>
        /// Gets or sets the type of the location where this sound should play.
        /// </summary>
        public SoundLocationType LocationType { get; set; }
    }
}
