using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class MeshLine: IMeshIndicie
    {
        public IndicieType IndicieType => IndicieType.Lines;

        public uint A { get; }
        public uint B { get; }

        public uint[] ToUintArray()
        {
            return new[] { A, B };
        }

        public MeshLine(uint a, uint b)
        {
            A = a;
            B = b;
        }
    }
}
