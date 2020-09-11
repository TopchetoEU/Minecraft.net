using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Vector3Uniform: IUniform
    {
        public Vector3Uniform(string name, Vector3 value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public Vector3 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniform(Name, Value.X, Value.Y, Value.Z);
    }
}
