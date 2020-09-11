namespace MinecraftNetWindow.Geometry
{
    public class ShaderArgumentMap
    {
        public string Name { get; }
        public int Size { get; }

        public ArgumentMap Map(ShaderProgram shader)
        {
            return new ArgumentMap(shader.GetAttributeLocation(Name), Size);
        }

        public ShaderArgumentMap(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
