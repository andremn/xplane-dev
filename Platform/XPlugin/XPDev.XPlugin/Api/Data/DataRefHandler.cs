using System;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// The handler of a X-Plane data ref which can be used to read or write values from or to data refs.
    /// </summary>
    public class DataRefHandler
    {
        private readonly string _name;
        private readonly IntPtr _dataRefPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRefHandler"/> class with the specified name and pointer.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref in umanaged memory.</param>
        internal DataRefHandler(string name, IntPtr dataRefPtr)
        {
            _name = name;
            _dataRefPtr = dataRefPtr;
        }

        /// <summary>
        /// Reads the current data ref value as an integer number.
        /// </summary>
        /// <returns>The value of the current data ref as an integer number.</returns>
        public IDataRef<int> AsInt()
        {
            return new IntDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as a boolean.
        /// </summary>
        /// <returns>The value of the current data ref as an boolean.</returns>
        public IDataRef<bool> AsBool()
        {
            return new BoolDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as a single-precision floating-point number.
        /// </summary>
        /// <returns>The value of the current data ref as a single-precision float number.</returns>
        public IDataRef<float> AsFloat()
        {
            return new FloatDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as a double-precision floating-point number.
        /// </summary>
        /// <returns>The value of the current data ref as a double-precision floating-point number.</returns>
        public IDataRef<double> AsDouble()
        {
            return new DoubleDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as an array of integer numbers.
        /// </summary>
        /// <returns>The value of the current data ref as an array of integer numbers.</returns>
        public IDataRef<int[]> AsIntArray()
        {
            return new IntArrayDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as an array of single-precision floating-point numbers.
        /// </summary>
        /// <returns>The value of the current data ref as an array of single-precision floating-point numbers.</returns>
        public IDataRef<float[]> AsFloatArray()
        {
            return new FloatArrayDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as an array of bytes.
        /// </summary>
        /// <returns>The value of the current data ref as an array of bytes.</returns>
        public IDataRef<byte[]> AsData()
        {
            return new ByteArrayDataRef(_name, _dataRefPtr);
        }

        /// <summary>
        /// Reads the current data ref value as a string.
        /// </summary>
        /// <returns>The value of the current data ref as a string.</returns>
        public IDataRef<string> AsString()
        {
            return new StringDataRef(_name, _dataRefPtr);
        }
    }
}
