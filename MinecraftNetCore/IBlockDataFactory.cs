namespace MinecraftNetCore
{
    public interface IBlockDataFactory
    {
        IBlockData Parse(string raw);
        IBlockData Default { get; }
    }
}
