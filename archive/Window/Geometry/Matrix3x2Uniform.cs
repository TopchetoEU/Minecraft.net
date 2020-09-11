using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix3x2Uniform: IUniform
    {
        public Matrix3x2Uniform(string name, Matrix3x2 value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public Matrix3x2 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
