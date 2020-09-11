namespace MinecraftNetWindow.Geometry
{
    public class FloatUniform: IUniform
    {
        public FloatUniform(string name, float value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public float Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniform(Name, Value);
    }
}
