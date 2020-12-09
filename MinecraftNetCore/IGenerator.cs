namespace MinecraftNetCore
{
    public interface IGenerator
    {
        public int Seed { get; }
        public Chunk GenerateChunkAt(ChunkLocation location);
    }
}
