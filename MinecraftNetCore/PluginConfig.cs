namespace MinecraftNet
{
    public class PluginConfig
    {
        public string[] Dependencies { get; set; } = new string[0];
        public string Name { get; set; } = "sample-text";
        public string Version { get; set; } = "0.0.1";
        public string[] Authors { get; set; } = new[] { "Steve Jobs" };
        public string Description { get; set; } = "A fine work of sample textary";
        public string MainType { get; set; }
    }
}
