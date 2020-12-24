namespace MinecraftNet
{
    public interface IWorld<ChunkT> where ChunkT : IChunk
    {
        IChunkFactory<ChunkT> ChunkFactory { get; }
        ChunkT GetChunk(ChunkLocation Location);

        void LoadChunk(ChunkT chunk, ChunkLocation location);
        ChunkT UnloadChunk(ChunkLocation chunk);

        Block this[int x, int y, int z] { get; set; }
    }
}
