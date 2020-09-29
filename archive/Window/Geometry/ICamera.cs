using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public interface ICamera
    {
        Transformation Transformation { get; set; }

        CameraUniformLayouts UniformLayouts { get; }

        Matrix4 GetCameraMatrix();
        Matrix4 GetViewMatrix();
    }
}