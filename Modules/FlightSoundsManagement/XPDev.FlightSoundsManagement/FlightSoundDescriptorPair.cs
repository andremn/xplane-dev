using XPDev.Audio;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Represents a pair of <see cref="ISoundDescriptor"/> and <see cref="FlightSoundsManagement.FlightSound"/>.
    /// </summary>
    internal struct FlightSoundDescriptorPair
    {
        public static readonly FlightSoundDescriptorPair Empty = new FlightSoundDescriptorPair(null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightSoundDescriptorPair"/> struct 
        /// with the specified <see cref="ISoundDescriptor"/> and <see cref="FlightSoundsManagement.FlightSound"/>.
        /// </summary>
        /// <param name="soundDescriptor">The sound descriptor.</param>
        /// <param name="flightSound">The flight sound.</param>
        public FlightSoundDescriptorPair(ISoundDescriptor soundDescriptor, FlightSound flightSound)
        {
            SoundDescriptor = soundDescriptor;
            FlightSound = flightSound;
        }

        /// <summary>
        /// Gets the <see cref="Audio.SoundDescriptor"/> associated with this pair.
        /// </summary>
        public ISoundDescriptor SoundDescriptor { get; }

        /// <summary>
        /// Gets the <see cref="FlightSoundsManagement.FlightSound"/> associated with this pair.
        /// </summary>
        public FlightSound FlightSound { get; }

        /// <summary>
        /// Gets whether this pair is empty, that is, both <see cref="SoundDescriptor"/> and <see cref="FlightSound"/> properties are null.
        /// </summary>
        public bool IsEmpty => SoundDescriptor == null && FlightSound == null;
    }
}
