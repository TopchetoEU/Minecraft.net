using OpenTK;

namespace MinecraftNetWindow
{
    public interface ITransformable
    {
        Transformation Transformation { get; set; }
        Matrix4 GetMatrix();
    }
}