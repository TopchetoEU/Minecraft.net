using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class ColoredVertex2D: IVertex
    {
        public ShaderArgumentMap[] ShaderArguments { get; } =
        {
            new ShaderArgumentMap("inPosition", 2),
            new ShaderArgumentMap("inColor", 4),
        };

        public float X { get; }
        public float Y { get; }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }

        public float[] ToFloatAray()
        {
            return new[] { X, Y, R, G, B, A };
        }

        public ColoredVertex2D(float x, float y, float r, float g, float b, float a)
        {
            X = x;
            Y = y;
            R = r; G = g;
            B = b; A = a;
        }
    }
}
