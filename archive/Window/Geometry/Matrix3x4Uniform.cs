using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix3x4Uniform: IUniform
    {
        public Matrix3x4Uniform(string name, Matrix3x4 value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public Matrix3x4 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
