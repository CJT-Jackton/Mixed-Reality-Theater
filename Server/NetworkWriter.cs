using System;
using System.Collections.Generic;
using System.Text;

namespace MRTheater_Server
{
    class NetworkWriter
    {
        private List<byte> buffer;

        private short type;
        private short size;

        /// <summary>
        /// Creates a new NetworkWriter.
        /// </summary>
        public NetworkWriter()
        {
            buffer = new List<byte>();
            size = 0;
        }

        public void StartMessage(short msgType)
        {
            type = msgType;
        }

        public void FinishMessage()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(size));
            buffer.InsertRange(2, BitConverter.GetBytes(type));
        }

        /// <summary>
        /// Returns a copy of internal array of bytes the writer is using, it copies only the bytes used.
        /// </summary>
        /// <returns>The array of bytes.</returns>
        public byte[] ToArray()
        {
            return buffer.ToArray();
        }

        #region Write
        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(char value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(char);
        }

        public void Write(byte value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(byte);
        }

        public void Write(sbyte value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(sbyte);
        }

        public void Write(short value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(short);
        }

        public void Write(ushort value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(ushort);
        }

        public void Write(int value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(int);
        }

        public void Write(uint value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(uint);
        }

        public void Write(long value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(long);
        }

        public void Write(ulong value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(ulong);
        }

        public void Write(float value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(float);
        }

        public void Write(double value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(double);
        }

        public void Write(string value)
        {
            // the length of the string
            buffer.AddRange(BitConverter.GetBytes((short)value.Length));
            // the actual data
            buffer.AddRange(Encoding.ASCII.GetBytes(value));

            size += sizeof(short);
            size += (short)value.Length;
        }

        public void Write(bool value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
            size += sizeof(bool);
        }

        public void Write(byte[] buf)
        {
            buffer.AddRange(buf);
            size += (short)buf.Length;
        }

        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }
        #endregion
    }
}
