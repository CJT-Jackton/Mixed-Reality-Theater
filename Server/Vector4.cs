namespace MRTheater_Server
{
    /// <summary>
    /// Representation of four-dimensional vectors.
    /// </summary>
    public class Vector4
    {
        private float[] _var = new float[4];

        /// <summary>
        /// Creates a new vector with default x, y, z, w components.
        /// </summary>
        public Vector4()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            w = 0.0f;
        }

        /// <summary>
        /// Creates a new vector with given x, y, z, w components.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Creates a new vector with another vector.
        /// </summary>
        /// <param name="other">The other vector</param>
        public Vector4(Vector4 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        /// <summary>
        /// Access the x, y, z, w components using [0], [1], [2], [3] respectively.
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
        /// W component of the vector.
        /// </summary>
        public float w
        {
            get => _var[3];
            set => _var[3] = value;
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
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ", " + w.ToString(format) + ")";
        }
    }
}
