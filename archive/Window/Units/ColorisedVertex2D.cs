using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class ColorisedVertex2D: IVertex
    {

        public ShaderArgumentMap[] ShaderArguments { get; } =
        {
                new ShaderArgumentMap("inPosition", 2),
                new ShaderArgumentMap("inTexcoord", 2),
                new ShaderArgumentMap("inColor", 4),
        };

        public float X { get; }
        public float Y { get; }

        public float T { get; }
        public float S { get; }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }

        public float[] ToFloatAray()
        {
            return new[] { X, Y, S, T, R, G, B, A };
        }

        public ColorisedVertex2D(float x, float y, float s, float t, float r, float g, float b, float a)
        {
            X = x;
            Y = y;

            S = s;
            T = t;
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
