using System.Numerics;

namespace XPDev.Audio
{
    /// <summary>
    /// Describes a sound.
    /// </summary>
    public interface ISoundDescriptor
    {
        /// <summary>
        /// Gets the path of the sound file.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets or sets the volume of the sound.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets or sets the occlusion of the sound.
        /// </summary>
        float Occlusion { get; set; }

        /// <summary>
        /// Gets or sets the position of the sound.
        /// </summary>
        Vector3 Position { get; set; }
    }
}
