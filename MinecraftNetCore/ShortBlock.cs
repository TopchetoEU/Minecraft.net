namespace MinecraftNetCore
{
    public struct ShortBlock
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public ShortBlock(Block block)
        {
            Data = block.Data.Stringify();
            Id = block.Type.Identifier.Id;
        }
        public string Stringify()
        {
            var a = new ByteWriteStream();

            a.Write(Id);
            a.Write(Data ?? "");

            return a.FlushString();
        }
        public static ShortBlock Parse(string raw)
        {
            var stream = new ByteReadStream(raw);

            var id = stream.ReadInt();
            var data = stream.ReadString();

            if (data.Length == 0)
                data = null;

            return new ShortBlock() {
                Data = data,
                Id = id,
            };
        }
    }
}
