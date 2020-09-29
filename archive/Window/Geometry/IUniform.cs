namespace MinecraftNetWindow.Geometry
{
    public interface IUniform
    {
        string Name { get; set; }
        void ApplyToShader(ShaderProgram shader);
    }
}
