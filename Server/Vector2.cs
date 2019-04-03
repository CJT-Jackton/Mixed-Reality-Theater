namespace MRTheater_Server
{
    /// <summary>
    /// Representation of 2D vectors and points.
    /// </summary>
    public class Vector2
    {
        private float[] _var = new float[2];

        /// <summary>
        /// Creates a new vector with default x, y components.
        /// </summary>
        public Vector2()
        {
            x = 0.0f;
            y = 0.0f;
        }

        /// <summary>
        /// Creates a new vector with given x, y components.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates a new vector with another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        public Vector2(Vector2 other)
        {
            x = other.x;
            y = other.y;
        }

        /// <summary>
        /// Access the x or y component using [0] or [1] respectively.
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
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ")";
        }
    }
}
