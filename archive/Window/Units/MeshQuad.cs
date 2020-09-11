using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class MeshQuad: IMeshIndicie
    {
        public IndicieType IndicieType => IndicieType.Quads;

        public uint A { get; }
        public uint B { get; }
        public uint C { get; }
        public uint D { get; }

        public uint[] ToUintArray()
        {
            return new[] { A, B, C };
        }

        public MeshQuad(uint a, uint b, uint c, uint d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
