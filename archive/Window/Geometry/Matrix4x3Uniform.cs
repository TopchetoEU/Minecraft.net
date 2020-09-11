using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix4x3Uniform: IUniform
    {
        public Matrix4x3Uniform(string name, Matrix4x3 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix4x3 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
