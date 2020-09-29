using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix2x3Uniform: IUniform
    {
        public Matrix2x3Uniform(string name, Matrix2x3 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix2x3 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
