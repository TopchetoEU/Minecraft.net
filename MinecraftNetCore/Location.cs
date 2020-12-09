using System;

namespace MinecraftNetCore
{
    public struct Location
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Location(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static explicit operator BlockLocation(Location location)
        {
            return new BlockLocation(
                (int)Math.Floor(location.X),
                (int)Math.Floor(location.Y),
                (int)Math.Floor(location.Z)
            );
        }
    }
}
