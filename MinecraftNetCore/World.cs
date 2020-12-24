using System;
using System.Collections.Generic;

namespace MinecraftNet
{
    public class World: IWorld<Chunk>
    {
        private Dictionary<ChunkLocation, Chunk> chunks = new Dictionary<ChunkLocation, Chunk>();

        public IChunkFactory<Chunk> ChunkFactory { get; } = new ChunkFactory();

        public Block this[int x, int y, int z] {
            get {
                var chunkX = (int)Math.Floor(x / (float)ChunkFactory.Width);
                var chunkY = (int)Math.Floor(y / (float)ChunkFactory.Height);

                var blockX = x - chunkX * ChunkFactory.Width;
                var blockY = y - chunkY * ChunkFactory.Height;

                var currChunk = chunks[new ChunkLocation(chunkX, chunkY, 0)];

                if (currChunk == null)
                    return new Block(BlockTypes.Get("air"));

                return currChunk[blockX, blockY, z];
            }
            set {
                var chunkX = (int)Math.Floor(x / (float)ChunkFactory.Width);
                var chunkY = (int)Math.Floor(y / (float)ChunkFactory.Height);

                var blockX = x - chunkX * ChunkFactory.Width;
                var blockY = y - chunkY * ChunkFactory.Height;

                chunks[new ChunkLocation(chunkX, chunkY, 0)][blockX, blockY, z] = value;
            }
        }

        public void LoadChunk(Chunk chunk, ChunkLocation location)
        {
            chunks[location] = chunk;
        }
        public Chunk UnloadChunk(ChunkLocation location)
        {
            if (chunks.Remove(location, out var res)) {
                return res;
            }
            else
                return null;
        }

        public Chunk GetChunk(ChunkLocation location)
        {
            return chunks.TryGetValue(location, out var res) ? res : null;
        }
    }
}
