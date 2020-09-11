using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix2x4Uniform: IUniform
    {
        public Matrix2x4Uniform(string name, Matrix2x4 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix2x4 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
