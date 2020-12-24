using System;

namespace MinecraftNet
{
    public struct BlockLocation
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public BlockLocation(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object location)
        {
            return location is BlockLocation loc && (this.X == loc.X && this.Y == loc.Y && this.Z == loc.Z);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(BlockLocation a, BlockLocation b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(BlockLocation a, BlockLocation b)
        {
            return !a.Equals(b);
        }

        public ChunkLocation ToChunkLocation(int width, int height, int depth)
        {
            return new ChunkLocation(
                (int)Math.Floor((float)X / width),
                (int)Math.Floor((float)Y / height),
                (int)Math.Floor((float)Z / depth)
            );
        }

        public static implicit operator Location(BlockLocation location)
        {
            return new Location(
                location.X,
                location.Y,
                location.Z
            );
        }
    }
}
