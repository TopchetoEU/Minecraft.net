using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class MeshPoint: IMeshIndicie
    {
        public IndicieType IndicieType => IndicieType.Points;

        public uint A { get; }

        public uint[] ToUintArray()
        {
            return new[] { A };
        }

        public MeshPoint(uint a)
        {
            A = a;
        }
    }
}
