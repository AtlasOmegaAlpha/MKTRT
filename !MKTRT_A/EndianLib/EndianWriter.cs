using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    public sealed class EndianWriter : IDisposable
    {
        private byte[] Buffer { get; set; }
        private Stream m { get; set; }
        private Endianness _Endian { get; set; }
        private long _Position { get; set; }
        public bool Disposed { get; private set; }

        public EndianWriter(Stream Stream, Endianness Endian)
        {
            if (Stream == null)
            {
                throw new NullReferenceException();
            }
            Disposed = false;
            _Position = 0;
            m = Stream;
            _Endian = Endian;
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
        private void CreateBuffer(int Size)
        {
            Buffer = new byte[Size];
        }
        private void WriteBuffer(int Count, int Stride)
        {
            _Position += Count;
            if (_Endian == Endianness.LittleEndian)
            {
                for (int i = 0; i < Count; i += Stride)
                {
                    Array.Reverse(Buffer, i, Stride);
                }
            }
            m.Write(Buffer, 0, Count);
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
        public void WriteString(string Data)
        {
            CreateBuffer(Data.Length * GetEncodingSize(Encoding.Default));
            Buffer = Encoding.Default.GetBytes(Data.ToCharArray());
            WriteBuffer(Data.Length, GetEncodingSize(Encoding.Default));
        }
    }
}
