using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public interface IVertex
    {
        ShaderArgumentMap[] ShaderArguments { get; }
        float[] ToFloatAray();
    }
}
