namespace MinecraftNet
{
    public class Block
    {
        public BlockType Type { get; set; }
        public IBlockData Data { get; }

        public Block(BlockType type)
        {
            Type = type;
            Data = type.DataFactory.Default;
        }
        public Block(BlockType type, string data)
        {
            Type = type;
            Data = type.DataFactory.Parse(data);
        }
        public Block(BlockType type, IBlockData data)
        {
            Type = type;
            Data = data;
        }

        public Block(ShortBlock shortBlock)
        {
            Type = BlockTypes.Get(shortBlock.Id);
            Data = Type.DataFactory.Parse(shortBlock.Data);
        }
    }
}
