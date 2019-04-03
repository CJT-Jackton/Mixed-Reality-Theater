using System;

namespace MRTheater_Server
{
    /// <summary>
    /// Representation of 3D vectors and points.
    /// </summary>
    public class Vector3
    {
        private float[] _var = new float[3];

        /// <summary>
        /// Creates a new vector with default x, y, z components.
        /// </summary>
        public Vector3()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }

        /// <summary>
        /// Creates a new vector with given x, y, z components.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        ///  Creates a new vector with another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        public Vector3(Vector3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        /// <summary>
        /// Access the x, y, z components using [0], [1], [2] respectively.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>the component value</returns>
        public float this[int i]
        {
            get => _var[i];
            set => _var[i] = value;
        }

        /// <summary>
        /// X component of the vector.
        /// </summary>
        public float x
        {
            get => _var[0];
            set => _var[0] = value;
        }

        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public float y
        {
            get => _var[1];
            set => _var[1] = value;
        }

        /// <summary>
        /// Z component of the vector.
        /// </summary>
        public float z
        {
            get => _var[2];
            set => _var[2] = value;
        }

        /// <summary>
        /// Returns a nicely formatted string for this vector.
        /// </summary>
        /// <returns>String representation of the vector.</returns>
        public override string ToString()
        {
            return ToString("");
        }

        /// <summary>
        /// Returns a nicely formatted string for this vector.
        /// </summary>
        /// <param name="format">The string format.</param>
        /// <returns>String representation of the vector.</returns>
        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
        }

        public static bool TryParse(string s, out Vector3 result)
        {
            result = new Vector3();

            bool parse = false;
            float fvalue;
            string[] str = s.Split(',');

            if (str.Length == 3)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (Single.TryParse(str[i], out fvalue))
                    {
                        result[i] = fvalue;

                    }
                    else
                    {
                        parse = false;
                        break;
                    }
                }

                parse = true;
            }

            return parse;
        }
    }
}
