using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;

namespace MinecraftNet
{
    public class BlockType
    {
        public BlockIdentifier Identifier { get; internal set; }
        public BlockModel Model { get; set; }
        public Texture2D Texture { get; }
        public IBlockDataFactory DataFactory { get; }

        public BlockType(BlockIdentifier id,
            BlockModel model = null, Texture2D texture = null,
            IBlockDataFactory dataFactory = null)
        {
            Identifier = id;
            Model = model ?? new SolidBlockModel();
            Texture = texture;
            DataFactory = dataFactory ?? new EmptyBlockDataFactory();
        }
        public BlockType(string id,
            BlockModel model = null, Texture2D texture = null,
            IBlockDataFactory dataFactory = null)
        {
            Identifier = new BlockIdentifier(id);
            Model = model ?? new SolidBlockModel();
            Texture = texture;
            DataFactory = dataFactory ?? new EmptyBlockDataFactory();
        }
        public BlockType(string id, string displayName,
            BlockModel model = null, Texture2D texture = null,
            IBlockDataFactory dataFactory = null)
        {
            Identifier = new BlockIdentifier(id, displayName);
            Model = model ?? new SolidBlockModel();
            Texture = texture;
            DataFactory = dataFactory ?? new EmptyBlockDataFactory();
        }

        public override bool Equals(object obj)
        {
            return obj is BlockType type &&
                   EqualityComparer<BlockIdentifier>.Default.Equals(Identifier, type.Identifier);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Identifier);
        }

        public static bool operator ==(BlockType a, BlockType b)
        {
            if (a is null) {
                return b is null;
            }
            else {
                return a.Equals(b);
            }
        }
        public static bool operator !=(BlockType a, BlockType b) => !a.Equals(b);
    }
}
