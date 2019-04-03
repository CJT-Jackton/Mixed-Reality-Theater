using System;
using System.Collections.Generic;
using System.Text;

namespace MRTheater_Server
{
    /// <summary>
    /// General purpose serializer. A class with same interfaces with UNET's NetworkReader, see https://docs.unity3d.com/ScriptReference/Networking.NetworkReader.html.
    /// </summary>
    class NetworkReader
    {
        /// <summary>
        /// The buffer.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// The current position within the buffer.
        /// </summary>
        private int position;

        /// <summary>
        /// The current length of the buffer.
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Creates a new NetworkReader object.
        /// </summary>
        /// <param name="data">The buffer to construct the reader with.</param>
        public NetworkReader(byte[] data)
        {
            buffer = data;
            position = 0;
            length = data.Length;
        }

        /// <summary>
        /// The current position within the buffer.
        /// </summary>
        public int Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// The current length of the buffer.
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Returns a string representation of the reader's buffer.
        /// </summary>
        /// <returns>The string representation of the buffer</returns>
        public override string ToString()
        {
            return buffer.ToString();
        }

        #region Read
        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public byte ReadByte()
        {
            byte b = buffer[position];
            position += sizeof(byte);
            return b;
        }

        /// <summary>
        /// Reads a number of bytes from the stream.
        /// </summary>
        /// <param name="count">Number of bytes to read.</param>
        /// <returns>Bytes read. (this is a copy).</returns>
        public byte[] ReadBytes(int count)
        {
            byte[] buf = new byte[count];
            Buffer.BlockCopy(buffer, position, buf, 0, count);
            position += count;
            return buf;
        }

        /// <summary>
        /// Reads a char from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public char ReadChar()
        {
            char c = BitConverter.ToChar(buffer, position);
            position += sizeof(char);
            return c;
        }

        /// <summary>
        /// Reads a double from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public double ReadDouble()
        {
            double d = BitConverter.ToDouble(buffer, position);
            position += sizeof(double);
            return d;
        }

        /// <summary>
        /// Reads a signed 16 bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public short ReadInt16()
        {
            short s = BitConverter.ToInt16(buffer, position);
            position += sizeof(short);
            return s;
        }

        /// <summary>
        /// Reads a signed 32bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public int ReadInt32()
        {
            int i = BitConverter.ToInt32(buffer, position);
            position += sizeof(int);
            return i;
        }

        /// <summary>
        /// Reads a signed 64 bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public long ReadInt64()
        {
            long l = BitConverter.ToInt64(buffer, position);
            position += sizeof(long);
            return l;
        }

        /// <summary>
        /// Reads a signed byte from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public sbyte ReadSByte()
        {
            sbyte sb = (sbyte)buffer[position];
            position += sizeof(sbyte);
            return sb;
        }

        /// <summary>
        /// Reads a float from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public float ReadSingle()
        {
            float f = BitConverter.ToSingle(buffer, position);
            position += sizeof(float);
            return f;
        }

        /// <summary>
        /// Reads a string from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public string ReadString()
        {
            short len = ReadInt16();
            string s = Encoding.ASCII.GetString(buffer, position, len);
            position += len;
            return s;
        }

        /// <summary>
        /// Reads an unsigned 16 bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public ushort ReadUInt16()
        {
            ushort us = BitConverter.ToUInt16(buffer, position);
            position += sizeof(ushort);
            return us;
        }

        /// <summary>
        /// Reads an unsigned 32 bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public uint ReadUInt32()
        {
            uint ui = BitConverter.ToUInt32(buffer, position);
            position += sizeof(uint);
            return ui;
        }

        /// <summary>
        /// Reads an unsigned 64 bit integer from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public ulong ReadUInt64()
        {
            ulong ul = BitConverter.ToUInt64(buffer, position);
            position += sizeof(ulong);
            return ul;
        }

        /// <summary>
        /// Reads a Vector2 object.
        /// </summary>
        /// <returns>The vector read from the stream.</returns>
        public Vector2 ReadVector2()
        {
            Vector2 vec2 = new Vector2();
            vec2.x = ReadSingle();
            vec2.y = ReadSingle();
            return vec2;
        }

        /// <summary>
        /// Reads a Vector3 object.
        /// </summary>
        /// <returns>The vector read from the stream.</returns>
        public Vector3 ReadVector3()
        {
            Vector3 vec3 = new Vector3();
            vec3.x = ReadSingle();
            vec3.y = ReadSingle();
            vec3.z = ReadSingle();
            return vec3;
        }

        /// <summary>
        /// Reads a Vector4 object.
        /// </summary>
        /// <returns>The vector read from the stream.</returns>
        public Vector4 ReadVector4()
        {
            Vector4 vec4 = new Vector4();
            vec4.x = ReadSingle();
            vec4.y = ReadSingle();
            vec4.z = ReadSingle();
            vec4.w = ReadSingle();
            return vec4;
        }
        #endregion
    }
}
