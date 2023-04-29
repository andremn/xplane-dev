using System;

namespace XPDev.Audio
{
    /// <summary>
    /// Represents the range of a sound.
    /// </summary>
    public struct SoundRange : IEquatable<SoundRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundRange"/> struct.
        /// </summary>
        /// <param name="minDistance">The minumum distance from where the sound will start to attenuate.</param>
        /// <param name="maxDistance">The distance where the sound will stop completely.</param>
        public SoundRange(float minDistance, float maxDistance)
        {
            MinDistance = minDistance;
            MaxDistance = maxDistance;
        }

        /// <summary>
        /// Gets the minumum distance from where the sound will start to attenuate.
        /// </summary>
        public float MinDistance { get; }

        /// <summary>
        /// Gets the distance where the sound will stop completely.
        /// </summary>
        public float MaxDistance { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is SoundRange range && Equals(range);
        }

        /// <summary>
        /// Indicates whether the current sound range is equal to the specified one.
        /// </summary>
        /// <param name="other">The other sound range to compare.</param>
        /// <returns>True if the sound ranges are equal; false otherwise.</returns>
        public bool Equals(SoundRange other)
        {
            return MinDistance == other.MinDistance &&
                   MaxDistance == other.MaxDistance;
        }

        /// <summary>
        /// Gets a hashcode for the sound range.
        /// </summary>
        /// <returns>A <see cref="int"/> used to uniquely identify the sound range.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(MinDistance, MaxDistance);
        }

        /// <summary>
        /// Indicates whether two sound ranges are equal.
        /// </summary>
        /// <param name="left">The sound range on the left side of the comparison.</param>
        /// <param name="right">The sound range on the right side of the comparison.</param>
        /// <returns>True if the sound ranges are equal; false otherwise.</returns>
        public static bool operator ==(SoundRange left, SoundRange right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two sound ranges are different.
        /// </summary>
        /// <param name="left">The sound range on the left side of the comparison.</param>
        /// <param name="right">The sound range on the right side of the comparison.</param>
        /// <returns>True if the sound ranges are different; false otherwise.</returns>
        public static bool operator !=(SoundRange left, SoundRange right)
        {
            return !(left == right);
        }
    }
}
