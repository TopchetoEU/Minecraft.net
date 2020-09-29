using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Matrix2Uniform: IUniform
    {
        public Matrix2Uniform(string name, Matrix2 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Matrix2 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniformMatrix(Name, Value);
    }
}
