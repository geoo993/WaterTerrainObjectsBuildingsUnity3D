namespace TerrainGenerator
{
    public class Vector2i
    {
        public int X { get; set; }

        public int Z { get; set; }

        public Vector2i()
        {
            X = 0;
            Z = 0;
        }

        public Vector2i(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector2i;
            if (other == null)
                return false;

            return this.X == other.X && this.Z == other.Z;
        }

        public override string ToString()
        {
            return "[" + X + "," + Z + "]";
        }
    }
}