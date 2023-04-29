using System;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// The possible types of a X-Plane dataref.
    /// </summary>
    [Flags]
    public enum DataRefType : int
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Integer type (<see cref="int"/>).
        /// </summary>
        Int = 1,
        /// <summary>
        /// Single float type (<see cref="float"/>).
        /// </summary>
        Float = 2,
        /// <summary>
        /// Double float type (<see cref="double"/>).
        /// </summary>
        Double = 4,
        /// <summary>
        /// Array of single float type (<see cref="float[]"/>).
        /// </summary>
        FloatArray = 8,
        /// <summary>
        /// Array of integer type (<see cref="int[]"/>).
        /// </summary>
        IntArray = 16,
        /// <summary>
        /// Variable block of data (<see cref="byte[]"/>).
        /// </summary>
        Data = 32
    }
}
