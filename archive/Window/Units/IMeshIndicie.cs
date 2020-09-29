using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public interface IMeshIndicie
    {
        uint[] ToUintArray();
        IndicieType IndicieType { get; }
    }
}
