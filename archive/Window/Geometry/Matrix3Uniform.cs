using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix3Uniform: IUniform
    {
        public Matrix3Uniform(string name, Matrix3 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix3 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
