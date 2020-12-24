using NetGL;

namespace MinecraftNet
{
    public struct Vertice
    {
        public float LightLevel { get; set; }
        public Vector3 position { get; set; }
        public Vector2 TexCoord { get; set; }

        public Vertice(float x, float y, float z, float s = 0, float t = 0, float light = 1)
        {
            LightLevel = light;
            position = new Vector3(x, y, z);
            TexCoord = new Vector2(s, t);
        }
    }
}
