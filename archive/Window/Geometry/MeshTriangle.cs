using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.Geometry
{
    public class MeshTriangle: IMeshIndicie
    {
        public IndicieType IndicieType => IndicieType.Triangles;

        public uint A { get; }
        public uint B { get; }
        public uint C { get; }

        public uint[] ToUintArray()
        {
            return new [] { A, B, C };
        }

        public MeshTriangle(uint a, uint b, uint c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
