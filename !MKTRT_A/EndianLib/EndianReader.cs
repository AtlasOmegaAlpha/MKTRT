using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    public sealed class EndianReader : IDisposable
    {
        private byte[] Buffer { get; set; }
        private Stream m { get; set; }
        private Endianness _Endian { get; set; }
        private long _Position { get; set; }
        public bool Disposed { get; private set; }

        public EndianReader(Stream Stream, Endianness Endian)
        {
            if (Stream == null)
            {
                throw new NullReferenceException();
            }
            Disposed = false;
            _Position = 0;
            _Endian = Endian;
            m = Stream;
        }
        
        public void Close()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (!Disposed)
            {
                GC.SuppressFinalize(this);
                if (m != null)
                {
                    m.Close();
                    m = null;
                }
                _Position = 0;
                Disposed = true;
            }
        }
        private void Try(int Length)
        {
            if (m == null)
            {
                throw new NullReferenceException();
            }
            if (_Position + Length > m.Length)
            {
                throw new EndOfStreamException();
            }
        }
        private void FillBuffer(int Count, int Stride)
        {
            Buffer = new byte[Count];
            m.Read(Buffer, 0, Count);
            _Position += Count;
            if (Endian == Endianness.LittleEndian)
            {
                for (int i = 0; i < Count; i += Stride)
                {
                    Array.Reverse(Buffer, i, Stride);
                }
            }
        }
        private static int GetEncodingSize(Encoding En)
        {
            if (En == Encoding.Unicode || En == Encoding.BigEndianUnicode)
            {
                return 2;
            }
            else if (En == Encoding.UTF32)
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }

        public Endianness Endian
        {
            get
            {
                return _Endian;
            }
            set
            {
                _Endian = value;
            }
        }
        public long Position
        {
            get
            {
                if (m == null)
                {
                    throw new NullReferenceException();
                }
                return _Position;
            }
            set
            {
                if (m == null)
                {
                    throw new NullReferenceException();
                }
                if (value < 0)
                {
                    throw new ArgumentException("The position can't be negative");
                }
                m.Position = value;
                _Position = value;
            }
        }
        public Int32 ReadInt32()
        {
            Try(sizeof(Int32));
            FillBuffer(4, 4);
            Array.Reverse(Buffer);
            return BitConverter.ToInt32(Buffer, 0);
        }
        public string ReadString(int Count)
        {
            Try(Count);
            FillBuffer(Count, 1);
            return Encoding.Default.GetString(Buffer);
        }
    }
}
