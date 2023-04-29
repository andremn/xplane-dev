using System;
using System.Runtime.InteropServices;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the native methods of the X-Plane Data API.
    /// </summary>
    internal unsafe static class XPLMDataAccessNativeMethods
    {
        /// <summary>
        /// Finds a data ref and returns a handle to it if found.
        /// </summary>
        /// <param name="inDataRefName">The name of the data ref.</param>
        /// <returns>A handle to the data ref if found; <see cref="IntPtr.Zero"/> otherwise.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static IntPtr XPLMFindDataRef([MarshalAs(UnmanagedType.LPStr)] string inDataRefName);

        /// <summary>
        /// Gets the value of the given data ref as an integer number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <returns>The value of the data ref as an integer number.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int XPLMGetDatai(IntPtr inDataRef);

        /// <summary>
        /// Sets the value of the given data ref as an integer number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValue">The value to set the data ref value to.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDatai(IntPtr inDataRef, int inValue);

        /// <summary>
        /// Gets the value of the given data ref as a single-precision floating number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <returns>The value of the data ref as a single-precision floating number.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static float XPLMGetDataf(IntPtr inDataRef);

        /// <summary>
        /// Sets the value of the given data ref as a  single-precision floating number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValue">The value to set the data ref value to.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDataf(IntPtr inDataRef, float inValue);

        /// <summary>
        /// Gets the value of the given data ref as a double-precision floating number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <returns>The value of the data ref as a double-precision floating number.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static double XPLMGetDatad(IntPtr inDataRef);

        /// <summary>
        /// Sets the value of the given data ref as a  double-precision floating number.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValue">The value to set the data ref value to.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDatad(IntPtr inDataRef, double inValue);

        /// <summary>
        /// Gets the value of the given data ref as an array of integer numbers.
        /// If <paramref name="outValues"/> is null, then the number of values in the data ref buffer is returned.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="outValues">The buffer where the values will be copied to.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the data ref.</param>
        /// <param name="inMax">The maximum number of values to be copied from the data ref.</param>
        /// <returns>The number of values copied.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int XPLMGetDatavi(IntPtr inDataRef, int[] outValues, int inOffset, int inMax);

        /// <summary>
        /// Sets the value of the given data ref as an array of integer numbers.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValues">The buffer where the values will be copeied from.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the buffer.</param>
        /// <param name="inCount">The maximum number of values to be copied to the data ref.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDatavi(IntPtr inDataRef, int[] inValues, int inOffset, int inCount);

        /// <summary>
        /// Gets the value of the given data ref as an array of single-precision floating numbers. 
        /// If <paramref name="outValues"/> is null, then the number of values in the data ref buffer is returned.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="outValues">The buffer where the values will be written to.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the data ref.</param>
        /// <param name="inMax">The maximum number of values to be copied from the data ref.</param>
        /// <returns>The number of values copied.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int XPLMGetDatavf(IntPtr inDataRef, float[] outValues, int inOffset, int inMax);

        /// <summary>
        /// Sets the value of the given data ref as an array of single-precision floating numbers.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValues">The buffer where the values will be copeied from.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the buffer.</param>
        /// <param name="inCount">The maximum number of values to be copied to the data ref.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDatavf(IntPtr inDataRef, float[] inValues, int inOffset, int inCount);

        /// <summary>
        /// Gets the value of the given data ref as an array of bytes.
        /// If <paramref name="outValues"/> is null, then the number of values in the data ref buffer is returned.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="outValues">The buffer where the values will be written to.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the data ref.</param>
        /// <param name="inMax">The maximum number of values to be copied from the data ref.</param>
        /// <returns>The number of values copied.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static int XPLMGetDatab(IntPtr inDataRef, byte[] outValues, int inOffset, int inMax);

        /// <summary>
        /// Sets the value of the given data ref as an array of bytes.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <param name="inValues">The buffer where the values will be copeied from.</param>
        /// <param name="inOffset">The offset at which the copy will be started in the buffer.</param>
        /// <param name="inCount">The maximum number of values to be copied to the data ref.</param>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static void XPLMSetDatab(IntPtr inDataRef, byte[] inValues, int inOffset, int inCount);

        /// <summary>
        /// Gets the types of the specified data ref.
        /// </summary>
        /// <param name="inDataRef">The handle of the data ref.</param>
        /// <returns>The types of the data ref as <see cref="DataRefType"/>.</returns>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal extern static DataRefType XPLMGetDataRefTypes(IntPtr inDataRef);
    }
}
