using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class Vertex2D: IVertex
    {
        public ShaderArgumentMap[] ShaderArguments { get; } =
        {
            new ShaderArgumentMap("inPosition", 2),
        };

        public float X { get; }
        public float Y { get; }

        public float[] ToFloatAray()
        {
            return new[] { X, Y };
        }

        public Vertex2D(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
