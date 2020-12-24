namespace MinecraftNet
{
    public interface IBlockDataFactory
    {
        IBlockData Parse(string raw);
        IBlockData Default { get; }
    }
}
