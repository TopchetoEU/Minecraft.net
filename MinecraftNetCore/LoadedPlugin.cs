namespace MinecraftNetCore
{
    public class LoadedPlugin
    {
        public PluginConfig Config { get; }

        public IPlugin Plugin { get; }

        public LoadedPlugin(IPlugin plugin, PluginConfig config)
        {
            Plugin = plugin;
            Config = config;
        }
    }
}
