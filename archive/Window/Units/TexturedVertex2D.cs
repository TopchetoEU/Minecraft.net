using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class TexturedVertex2D: IVertex
    {
        public ShaderArgumentMap[] ShaderArguments { get; } =
        {
            new ShaderArgumentMap("inPosition", 2),
            new ShaderArgumentMap("inTexcoord", 2),
        };

        public float X { get; }
        public float Y { get; }

        public float T { get; }
        public float S { get; }

        public float[] ToFloatAray()
        {
            return new[] { X, Y, S, T };
        }

        public TexturedVertex2D(float x, float y, float s, float t)
        {
            X = x;
            Y = y;

            S = s;
            T = t;
        }
    }
}
