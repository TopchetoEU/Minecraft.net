namespace MinecraftNetCore
{
    public interface IChunk
    {
        Block this[int x, int y, int z] { get; set; }
        Block this[BlockLocation location] { get; set; }

        string Minify();
    }
}
