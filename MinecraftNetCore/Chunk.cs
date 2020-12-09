using System;

namespace MinecraftNetCore
{
    public class Chunk: IChunk
    {
        private ShortBlock[,,] blocks = new ShortBlock[16, 255, 16];

        public Block this[BlockLocation location] {
            get => this[location.X, location.Y, location.Z];
            set => this[location.X, location.Y, location.Z] = value;
        }
        public Block this[int x, int y, int z] {
            get {
                if (x < 0 || x >= 16)
                    throw new Exception("The x component must be between 0 and 15");
                if (y < 0 || y >= 16)
                    throw new Exception("The y component must be between 0 and 15");
                if (x < 0 || x >= 255)
                    throw new Exception("The z component must be between 0 and 255");

                return new Block(blocks[x, y, z]);
            }
            set {
                if (x < 0 || x >= 16)
                    throw new Exception("The x component must be between 0 and 15");
                if (y < 0 || y >= 16)
                    throw new Exception("The y component must be between 0 and 15");
                if (x < 0 || x >= 255)
                    throw new Exception("The z component must be between 0 and 255");

                blocks[x, y, z] = new ShortBlock(value);
            }
        }

        public ShortBlock GetShortBlock(int x, int y, int z) => blocks[x, y, z];
        public void SetShortBlock(int x, int y, int z, ShortBlock value) => blocks[x, y, z] = value;

        public string Minify()
        {
            var raw = new ByteWriteStream();

            for (var y = 0; y < 255; y++) {
                for (var z = 0; z < 16; z++) {
                    for (var x = 0; x < 16; x++) {
                        var data = blocks[x, y, z].Stringify();
                        raw.Write(data.Length);
                        raw.Write(data);
                    }
                }
            }

            return raw.FlushString();
        }

        internal Chunk() { }
    }
}
