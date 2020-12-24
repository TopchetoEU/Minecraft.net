namespace MinecraftNet
{
    public class EmptyBlockDataFactory: IBlockDataFactory
    {
        public IBlockData Default => new EmptyBlockData();

        public IBlockData Parse(string raw)
        {
            return new EmptyBlockData();
        }
    }
}
