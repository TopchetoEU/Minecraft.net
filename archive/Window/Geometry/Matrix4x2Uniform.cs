using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix4x2Uniform: IUniform
    {
        public Matrix4x2Uniform(string name, Matrix4x2 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix4x2 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
