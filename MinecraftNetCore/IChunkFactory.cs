namespace MinecraftNetCore
{
    public interface IChunkFactory<ChunkT> where ChunkT : IChunk
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        ChunkT Deminify(string raw);
    }
}
