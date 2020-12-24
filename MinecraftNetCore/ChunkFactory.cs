namespace MinecraftNet
{
    public class ChunkFactory: IChunkFactory<Chunk>
    {
        public int Width => 16;
        public int Height => 255;
        public int Depth => 16;

        public Chunk Create()
        {
            return new Chunk();
        }

        public Chunk Deminify(string data)
        {
            var stream = new ByteReadStream(data);
            var chunk = new Chunk();

            for (var y = 0; y < 255; y++) {
                for (var z = 0; z < 16; z++) {
                    for (var x = 0; x < 16; x++) {
                        var length = stream.ReadInt();
                        var id = stream.ReadInt();
                        var blockData = stream.ReadString(length - 4);

                        chunk.SetShortBlock(x, y, z, new ShortBlock() {
                            Id = id,
                            Data = blockData,
                        });
                    }
                }
            }

            return chunk;
        }
    }
}
