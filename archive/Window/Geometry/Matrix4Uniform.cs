using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix4Uniform: IUniform
    {
        public Matrix4Uniform(string name, Matrix4 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix4 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
