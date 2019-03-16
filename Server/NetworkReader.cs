using System;
using System.Collections.Generic;
using System.Text;

namespace MRTheater_Server
{
    class NetworkReader
    {
        private byte[] buffer;
        private int position;
        private readonly int length;

        public NetworkReader(byte[] data)
        {
            buffer = data;
            position = 0;
            length = data.Length;
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int Position
        {
            get
            {
                return position;
            }
        }

        public override string ToString()
        {
            return buffer.ToString();
        }

        #region Read
        public byte ReadByte()
        {
            byte b = buffer[position];
            position += sizeof(byte);
            return b;
        }
        
        public byte[] ReadBytes(int count)
        {
            byte[] buf = new byte[count];
            Buffer.BlockCopy(buffer, position, buf, 0, count);
            position += count;
            return buf;
        }

        public char ReadChar()
        {
            char c = BitConverter.ToChar(buffer, position);
            position += sizeof(char);
            return c;
        }

        public double ReadDouble()
        {
            double d = BitConverter.ToDouble(buffer, position);
            position += sizeof(double);
            return d;
        }

        public short ReadInt16()
        {
            short s = BitConverter.ToInt16(buffer, position);
            position += sizeof(short);
            return s;
        }

        public int ReadInt32()
        {
            int i = BitConverter.ToInt32(buffer, position);
            position += sizeof(int);
            return i;
        }

        public long ReadInt64()
        {
            long l = BitConverter.ToInt64(buffer, position);
            position += sizeof(long);
            return l;
        }

        public sbyte ReadSByte()
        {
            sbyte sb = (sbyte)buffer[position];
            position += sizeof(sbyte);
            return sb;
        }

        public float ReadSingle()
        {
            float f = BitConverter.ToSingle(buffer, position);
            position += sizeof(float);
            return f;
        }

        public string ReadString()
        {
            short len = ReadInt16();
            string s = Encoding.ASCII.GetString(buffer, position, len);
            position += len;
            return s;
        }

        public ushort ReadUInt16()
        {
            ushort us = BitConverter.ToUInt16(buffer, position);
            position += sizeof(ushort);
            return us;
        }

        public uint ReadUInt32()
        {
            uint ui = BitConverter.ToUInt32(buffer, position);
            position += sizeof(uint);
            return ui;
        }

        public ulong ReadUInt64()
        {
            ulong ul = BitConverter.ToUInt64(buffer, position);
            position += sizeof(ulong);
            return ul;
        }

        public Vector2 ReadVector2()
        {
            Vector2 vec2 = new Vector2();
            vec2.x = ReadSingle();
            vec2.y = ReadSingle();
            return vec2;
        }

        public Vector3 ReadVector3()
        {
            Vector3 vec3 = new Vector3();
            vec3.x = ReadSingle();
            vec3.y = ReadSingle();
            vec3.z = ReadSingle();
            return vec3;
        }

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
