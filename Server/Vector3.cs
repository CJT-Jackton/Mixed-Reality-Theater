namespace MRTheater_Server
{
    class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
        }
    }
}
