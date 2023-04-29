using System;
using System.Diagnostics.CodeAnalysis;

namespace XPDev.Audio
{
    /// <summary>
    /// Represents the way a sound repeats.
    /// </summary>
    public struct SoundRepeatTimes : IEquatable<SoundRepeatTimes>, IComparable<SoundRepeatTimes>
    {
        /// <summary>
        /// Gets a <see cref="SoundRepeatTimes"/> which can be used for a sound that repeats once.
        /// </summary>
        public static readonly SoundRepeatTimes Once = new SoundRepeatTimes(1);

        /// <summary>
        /// Gets a <see cref="SoundRepeatTimes"/> which can be used for a sound that repeats twice.
        /// </summary>
        public static readonly SoundRepeatTimes Twice = new SoundRepeatTimes(2);

        /// <summary>
        /// Gets a <see cref="SoundRepeatTimes"/> which can be used for a sound that repeats forever.
        /// </summary>
        public static readonly SoundRepeatTimes Forever = new SoundRepeatTimes(-1);

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundRepeatTimes"/> structure.
        /// </summary>
        /// <param name="numberOfRepeats"></param>
        public SoundRepeatTimes(int numberOfRepeats)
        {
            NumberOfRepeats = numberOfRepeats;
        }

        /// <summary>
        /// Gets the number of times this <see cref="SoundRepeatTimes"/> repeats.
        /// </summary>
        public int NumberOfRepeats { get; }

        /// <summary>
        /// Compares the current object with the specified <see cref="SoundRepeatTimes"/>.
        /// </summary>
        /// <param name="other">The <see cref="SoundRepeatTimes"/> to compare.</param>
        /// <returns>A positive integer if the <see cref="NumberOfRepeats"/> of the current object is greater than 
        /// the <see cref="NumberOfRepeats"/> of other object.</returns>
        public int CompareTo([AllowNull] SoundRepeatTimes other)
        {
            if (this == other)
            {
                return 0;
            }

            return NumberOfRepeats > other.NumberOfRepeats ? 1 : -1;
        }

        /// <summary>
        /// Compares if the current object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is SoundRepeatTimes times && Equals(times);
        }

        /// <summary>
        /// Compares if the current object is equal to another object.
        /// </summary>
        /// <param name="other">The other object to compare.</param>
        /// <returns>True if the objects are equal; false otherwise.</returns>
        public bool Equals(SoundRepeatTimes other)
        {
            return NumberOfRepeats == other.NumberOfRepeats;
        }

        /// <summary>
        /// Gets the hash code for this <see cref="SoundRepeatTimes"/>.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(NumberOfRepeats);
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is equal to the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the objects are equal; false otherwise.</returns>
        public static bool operator ==(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is not equal to the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the objects are not equal; false otherwise.</returns>
        public static bool operator !=(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is greater than the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the object on the left side is greater than the one in the right side; false otherwise.</returns>
        public static bool operator >(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return left.NumberOfRepeats > right.NumberOfRepeats;
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is less than the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the object on the left side is less than the one in the right side; false otherwise.</returns>
        public static bool operator <(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return left.NumberOfRepeats < right.NumberOfRepeats;
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is greater than or equal to the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the object on the left side is greater than or equal to the one in the right side; false otherwise.</returns>
        public static bool operator >=(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return left == right || left > right;
        }

        /// <summary>
        /// Compares if the <see cref="SoundRepeatTimes"/> on the left side of the operation is less than or equal to the <see cref="SoundRepeatTimes"/> on the right side.
        /// </summary>
        /// <param name="left">The <see cref="SoundRepeatTimes"/> on the left side of the operation.</param>
        /// <param name="right">The <see cref="SoundRepeatTimes"/> on the right side of the operation.</param>
        /// <returns>True if the object on the left side is less than or equal to the one in the right side; false otherwise.</returns>
        public static bool operator <=(SoundRepeatTimes left, SoundRepeatTimes right)
        {
            return left == right || left < right;
        }
    }
}
