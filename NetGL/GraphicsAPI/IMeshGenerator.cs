namespace NetGL.GraphicsAPI
{
    public interface IMeshGenerator<T> where T : struct
    {
        VBO<T> GetBuffer();
        ShaderProgram GetProgram();
    }
}
