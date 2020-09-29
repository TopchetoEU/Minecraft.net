using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Vector2Uniform: IUniform
    {
        public Vector2Uniform(string name, Vector2 value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public Vector2 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniform(Name, Value.X, Value.Y);
    }
}
