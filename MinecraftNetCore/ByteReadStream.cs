using System;
using System.Linq;

namespace MinecraftNetCore
{
    public class ByteReadStream
    {
        private byte[] bytes;
        int index = 0;

        public ByteReadStream(string data)
        {
            bytes = data.ToCharArray().Select(v => (byte)v).ToArray();
        }
        public ByteReadStream(byte[] data)
        {
            bytes = (byte[])data.Clone();
        }

        public byte ReadByte()
        {
            return ReadByteArray(1)[0];
        }
        public byte[] ReadByteArray(int amount)
        {
            var array = new byte[amount];

            for (int i = 0; i < amount; i++) {
                if (index >= bytes.Length)
                    throw new Exception("Reached the end of the array");
                array[i] = bytes[index];
                index++;

            }

            return array;
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadByteArray(8));
        }
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadByteArray(4));
        }
        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadByteArray(2));
        }

        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadByteArray(1));
        }

        public char ReadChar()
        {
            return BitConverter.ToChar(ReadByteArray(1));
        }
        public char[] ReadCharArray(int? length = null)
        {
            if (!length.HasValue)
                length = bytes.Length - index;

            return ReadByteArray(length.Value).Select(v => (char)v).ToArray();
        }
        public string ReadString(int? length = null)
        {
            return new string(ReadCharArray(length));
        }
    }
}
