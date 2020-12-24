using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftNet
{
    public class ByteWriteStream
    {
        private List<byte> bytes = new List<byte>();

        public ByteWriteStream Write(byte value)
        {
            bytes.Add(value);
            return this;
        }
        public ByteWriteStream Write(IEnumerable<byte> values)
        {
            bytes.AddRange(values);
            return this;
        }
        public ByteWriteStream Write(params byte[] values)
        {
            bytes.AddRange(values);
            return this;
        }

        public ByteWriteStream Write(long value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
            return this;
        }
        public ByteWriteStream Write(int value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
            return this;
        }
        public ByteWriteStream Write(short value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriteStream Write(bool value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriteStream Write(char value)
        {
            bytes.Add((byte)value);
            return this;
        }
        public ByteWriteStream Write(IEnumerable<char> values)
        {
            bytes.AddRange(values.Select(v => (byte)v));
            return this;
        }
        public ByteWriteStream Write(params char[] values)
        {
            bytes.AddRange(values.Select(v => (byte)v));
            return this;
        }

        public ByteWriteStream Write(string value)
        {
            var tempValue = value;
            if (value == null)
                tempValue = "";
            Write(tempValue.ToCharArray());
            return this;
        }

        public byte[] Flush()
        {
            var a = bytes.ToArray();
            bytes.Clear();

            return a;
        }
        public string FlushString()
        {
            return new string(Flush().Select(v => (char)v).ToArray());
        }
    }
}
