using System;
using System.Text;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Represents a data ref with a value of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the value of the data ref.</typeparam>
    internal abstract class DataRef<T> : IDataRef<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRef{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        protected DataRef(string name, IntPtr dataRefPtr)
        {
            if (dataRefPtr == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid data ref pointer", nameof(dataRefPtr));
            }

            Name = name;
            DataRefPtr = dataRefPtr;
        }

        /// <summary>
        /// Gets the name of the data ref.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the pointer to the data ref.
        /// </summary>
        public IntPtr DataRefPtr { get; }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public abstract T Value { get; set; }
    }

    /// <summary>
    /// Represents a data ref with integer number value.
    /// </summary>
    internal class IntDataRef : DataRef<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public IntDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override int Value
        {
            get { return XPLMDataAccessNativeMethods.XPLMGetDatai(DataRefPtr); }
            set { XPLMDataAccessNativeMethods.XPLMSetDatai(DataRefPtr, value); }
        }
    }

    /// <summary>
    /// Represents a data ref with an integer number as the value.
    /// </summary>
    internal class BoolDataRef : DataRef<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public BoolDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override bool Value
        {
            get { return XPLMDataAccessNativeMethods.XPLMGetDatai(DataRefPtr) == 1; }
            set { XPLMDataAccessNativeMethods.XPLMSetDatai(DataRefPtr, value ? 1 : 0); }
        }
    }

    /// <summary>
    /// Represents a data ref with a single-precision floating number as the value.
    /// </summary>
    internal class FloatDataRef : DataRef<float>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public FloatDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override float Value
        {
            get { return XPLMDataAccessNativeMethods.XPLMGetDataf(DataRefPtr); }
            set { XPLMDataAccessNativeMethods.XPLMSetDataf(DataRefPtr, value); }
        }
    }

    /// <summary>
    /// Represents a data ref with a double-precision floating number as the value.
    /// </summary>
    internal class DoubleDataRef : DataRef<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public DoubleDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override double Value
        {
            get { return XPLMDataAccessNativeMethods.XPLMGetDatad(DataRefPtr); }
            set { XPLMDataAccessNativeMethods.XPLMSetDatad(DataRefPtr, value); }
        }
    }

    /// <summary>
    /// Represents a data ref with  an array of integer numbers as the value.
    /// </summary>
    internal class IntArrayDataRef : DataRef<int[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntArrayDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public IntArrayDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override int[] Value
        {
            get { return GetDataRefValue(); }
            set { SetDataRefValue(value); }
        }

        private int[] GetDataRefValue()
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                var buffer = new int[arrayLength];

                XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, buffer, 0, arrayLength);

                return buffer;
            }

            return new int[0];
        }

        private void SetDataRefValue(int[] value)
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, value, 0, arrayLength);
            }
        }
    }

    /// <summary>
    /// Represents a data ref with an array of single-precision floating numbers as the value.
    /// </summary>
    internal class FloatArrayDataRef : DataRef<float[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatArrayDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public FloatArrayDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override float[] Value
        {
            get { return GetDataRefValue(); }
            set { SetDataRefValue(value); }
        }

        private float[] GetDataRefValue()
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavf(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                var buffer = new float[arrayLength];

                XPLMDataAccessNativeMethods.XPLMGetDatavf(DataRefPtr, buffer, 0, arrayLength);

                return buffer;
            }

            return new float[0];
        }

        private void SetDataRefValue(float[] value)
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                XPLMDataAccessNativeMethods.XPLMGetDatavf(DataRefPtr, value, 0, arrayLength);
            }
        }
    }

    /// <summary>
    /// Represents a data ref with an array of bytes as the value.
    /// </summary>
    internal class ByteArrayDataRef : DataRef<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public ByteArrayDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override byte[] Value
        {
            get { return GetDataRefValue(); }
            set { SetDataRefValue(value); }
        }

        private byte[] GetDataRefValue()
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                var buffer = new byte[arrayLength];

                XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, buffer, 0, arrayLength);

                return buffer;
            }

            return new byte[0];
        }

        private void SetDataRefValue(byte[] value)
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, value, 0, arrayLength);
            }
        }
    }

    /// <summary>
    /// Represents a data ref with a string as the value.
    /// </summary>
    internal class StringDataRef : DataRef<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringDataRef"/> class.
        /// </summary>
        /// <param name="name">The name of the data ref.</param>
        /// <param name="dataRefPtr">The pointer to the data ref.</param>
        public StringDataRef(string name, IntPtr dataRefPtr) : base(name, dataRefPtr)
        {
        }

        /// <summary>
        /// Gets or sets the value of the data ref.
        /// </summary>
        public override string Value
        {
            get { return GetDataRefValue(); }
            set { SetDataRefValue(value); }
        }

        private string GetDataRefValue()
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                var buffer = new byte[arrayLength];

                XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, buffer, 0, arrayLength);

                return Encoding.ASCII.GetString(buffer, 0, arrayLength).TrimEnd((char)0);
            }

            return string.Empty;
        }

        private void SetDataRefValue(string value)
        {
            var arrayLength = XPLMDataAccessNativeMethods.XPLMGetDatavi(DataRefPtr, null, 0, 0);

            if (arrayLength > 0)
            {
                var stringBytes = Encoding.ASCII.GetBytes(value, 0, arrayLength);

                XPLMDataAccessNativeMethods.XPLMGetDatab(DataRefPtr, stringBytes, 0, arrayLength);
            }
        }
    }
}
