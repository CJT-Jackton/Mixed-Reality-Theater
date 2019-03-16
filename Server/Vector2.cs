namespace MRTheater_Server
{
    class Vector2
    {
        public float x;
        public float y;

        public Vector2()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 other)
        {
            x = other.x;
            y = other.y;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ")";
        }
    }
}
