using System;

namespace MinecraftNetCore
{
    public struct ChunkLocation
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public ChunkLocation(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object location)
        {
            return location is ChunkLocation loc && (this.X == loc.X && this.Y == loc.Y && this.Z == loc.Z);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(ChunkLocation a, ChunkLocation b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(ChunkLocation a, ChunkLocation b)
        {
            return !a.Equals(b);
        }

        public BlockLocation ToBlockLocation(int width, int height, int depth)
        {
            return new BlockLocation(X * width, Y * height, Z * depth);
        }
    }
}
