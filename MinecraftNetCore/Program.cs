using NetGL.WindowAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace MinecraftNet
{
    public interface IPlugin
    {
        void Load();
        void Unload();
    }
    public interface IAllLoadedHandler: IPlugin
    {
        void AllLoaded();

        public static void TryCall(IPlugin plugin)
        {
            if (plugin is IAllLoadedHandler handler) {
                handler.AllLoaded();
            }
        }
    }
    public interface IWindowHost: IPlugin
    {
        void Initialise();
    }
    public interface IRenderer
    {
        void Render();
    }
    public interface IAssetsPlugin: IPlugin
    {
        void LoadAssetManager(IPluginFileManager manager);

        public static void TryCall(IPlugin plugin, IPluginFileManager fileManager)
        {
            if (plugin is IAssetsPlugin pl)
                pl.LoadAssetManager(fileManager);
        }
    }

    public enum MouseButton
    {
        None,
        Left,
        Right,
        Middle,
    }

    public class MouseEventArgs
    {
        public int Delta { get; }
        public Point Location { get; }
        public MouseButton Button { get; }

        public MouseEventArgs(int delta, int x, int y, MouseButton button)
        {
            Delta = delta;
            Location = new Point(x, y);
            Button = button;
        }
    }
    public class KeyboardEventArgs
    {
        public Key Key { get; }

        public KeyboardEventArgs(Key key)
        {
            Key = key;
        }
    }

    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    public interface IMouseControl
    {
        Point Location { get; }

        bool IsPressed(MouseButton button);

        event EventHandler Moved;
        event EventHandler Pressed;
        event EventHandler Released;
        event EventHandler Scrolled;
    }
    public interface IKeyboardControl
    {
        bool IsPressed(Key key);

        event KeyboardEventHandler Pressed;
        event KeyboardEventHandler Released;
    }

    public struct Size
    {
        public int Width { get; }
        public int Height { get; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    public struct SizeF
    {
        public float X { get; }
        public float Y { get; }

        public SizeF(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public interface IPluginFileManager
    {
        StreamReader LoadAsset(string relativePath);
        StreamWriter SaveFile(string relativePath);
    }
    public class SimplePluginFileManager: IPluginFileManager
    {
        private string pluginPath;

        private string GetAssetPath(string relativePath)
        {
            var assetsPath = Path.GetFullPath(Path.Combine(pluginPath, "assets"));

            var newPath = Path.GetFullPath(Path.Combine(assetsPath, relativePath));

            if (!newPath.StartsWith(newPath))
                throw new Exception("Can't access files outside the assets folder!");

            if (!File.Exists(newPath))
                throw new Exception($"The selected asset {newPath} doesn't exist");

            return newPath;
        }
        private string GetFilePath(string relativePath)
        {
            var dataPath = Path.GetFullPath(Path.Combine(pluginPath, "pluginData"));

            Directory.CreateDirectory(dataPath);

            var newPath = Path.GetFullPath(Path.Combine(dataPath, relativePath));

            if (!newPath.StartsWith(newPath))
                throw new Exception("Can't access files outside the data folder!");

            return newPath;
        }

        public StreamWriter SaveFile(string relativePath)
        {
            return new StreamWriter(GetFilePath(relativePath));
        }
        public StreamReader LoadAsset(string relativePath)
        {
            return new StreamReader(GetAssetPath(relativePath));
        }

        public SimplePluginFileManager(string path)
        {
            pluginPath = Path.GetFullPath(path);
        }
    }

    public static class Program
    {
        #region Plugin Resolvation
        private class PluginShell
        {
            public PluginConfig Config { get; set; }
            public string DllPath { get; set; }

            public bool Loaded { get; private set; } = false;

            public IPlugin Plugin { get; private set; }

            public void Load()
            {
                if (Loaded)
                    throw new Exception("Can't load the plugin since it's already loaded");
                if (Plugin == null) {
                    var file = Assembly.LoadFile(Path.GetFullPath(DllPath));
                    var type = file.GetExportedTypes().First(v => v.FullName == Config.MainType);

                    if (Activator.CreateInstance(type) is IPlugin a) {
                        Plugin = a;
                    }
                    else {
                        Console.WriteLine("Can't load the plugin, since it doesn't implement IPlugin");
                    }
                }

                Console.WriteLine("Loading plugin {0} {1}...", Config.Name, Config.Version);
                Plugin.Load();

                Loaded = true;
            }
            public void Unload()
            {
                if (!Loaded)
                    throw new Exception("Can't unload the plugin since it's already unloaded");

                Console.WriteLine("Unloading plugin {0} ...", Config.Name);
                Plugin.Unload();

                Loaded = false;
            }

            public bool TryLoad()
            {
                if (Loaded)
                    return false;

                Load();
                return true;
            }
            public bool TryUnload()
            {
                if (!Loaded)
                    return false;

                Unload();
                return true;
            }
        }
        private class PluginNode
        {
            public PluginShell Node { get; set; }
            public List<PluginNode> Dependencies { get; } = new List<PluginNode>();
            public List<PluginNode> Dependents { get; } = new List<PluginNode>();

            public bool Loaded => Node.Loaded;
            public bool Constructed { get; set; } = false;
            public bool DependencyOk { get; set; } = false;

            public PluginNode(PluginShell plugin, bool depOk = false)
            {
                Node = plugin;

                DependencyOk = depOk;
            }
        }

        static PluginShell ParsePlugin(string path)
        {
            try {
                var a = Path.GetFullPath(Path.Combine(Config.PluginFolder, path, "config.json"))
                    .Substring(Directory.GetCurrentDirectory().Length + 1);
                var b = Path.GetFullPath(Path.Combine(Config.PluginFolder, path, "plugin.dll"))
                    .Substring(Directory.GetCurrentDirectory().Length + 1);
                return new PluginShell() {
                    Config = JsonSerializer.Deserialize<PluginConfig>(File.ReadAllText(a)),
                    DllPath = b
                };
            }
            catch {
                return null;
            }
        }
        static void CheckDependency(PluginNode node, List<PluginNode> path = null)
        {
            if (node.DependencyOk)
                return;
            else {
                var a = path.ToList();

                if (a.Contains(node)) {
                    while (a[0] != node)
                        a.RemoveAt(0);

                    throw new Exception(
                        "Can't load plugins, because there's a curcular" +
                        "dependency between the following plugins: " +
                        string.Join(" <-> ", a));
                }

                a.Add(node);

                foreach (var child in node.Dependencies) {
                    CheckDependency(child, a);
                }
                return;
            }
        }
        static bool AllDependenciesLoaded(PluginNode node)
        {
            foreach (var child in node.Dependencies) {
                if (!child.Loaded)
                    return false;
            }

            return true;
        }
        static bool AllDependentsUnloaded(PluginNode node)
        {
            foreach (var child in node.Dependencies) {
                if (child.Loaded)
                    return false;
            }

            return true;
        }

        static void LoadPlugins(Dictionary<string, PluginNode> nodes)
        {
            var allLoaded = false;

            var failed = 0;

            while (!allLoaded) {
                allLoaded = true;
                foreach (var node in nodes) {
                    var depsOk = AllDependenciesLoaded(node.Value);

                    if (depsOk) {
                        if (!node.Value.Node.TryLoad())
                            failed++;
                        else {
                            IAllLoadedHandler.TryCall(node.Value.Node.Plugin);
                            IAssetsPlugin.TryCall(node.Value.Node.Plugin, new SimplePluginFileManager(node.Value.Node.DllPath + "/.."));
                        }
                    }
                    else
                        allLoaded = false;
                }
            }

            if (failed == 0)
                Console.WriteLine("All plugins loaded");
            else if (failed == 1)
                Console.WriteLine("1 plugin failed to load");
            else
                Console.WriteLine("{0} plugins failed to load", failed);

            foreach (var nodePair in nodes) {
                var node = nodePair.Value;
            }
        }
        static void UnloadPlugins(Dictionary<string, PluginNode> nodes)
        {
            var allUnloaded = false;

            var failed = 0;

            while (!allUnloaded) {
                allUnloaded = true;

                foreach (var nodePair in nodes) {
                    var node = nodePair.Value;
                    var dependentsOk = AllDependentsUnloaded(node);

                    if (dependentsOk) {
                        if (!node.Node.TryUnload())
                            failed++;
                    }
                    else
                        allUnloaded = false;
                }
            }

            if (failed == 0)
                Console.WriteLine("All plugins loaded");
            else if (failed == 1)
                Console.WriteLine("1 plugin failed to load");
            else
                Console.WriteLine("{0} plugins failed to load", failed);
        }

        static Dictionary<string, PluginNode> ResolvePlugins()
        {
            var pluginPaths = Directory.GetDirectories(
                Path.GetFullPath(Config.PluginFolder)
            );
            var plugins = new List<PluginShell>();
            var nodes = new Dictionary<string, PluginNode>();

            foreach (var path in pluginPaths) {
                var parsed = ParsePlugin(path);
                if (parsed is null) {
                    Console.WriteLine($"Can't load a plugin at {path}, since the file format was incorrect");
                }
                else
                    plugins.Add(parsed);
            }
            foreach (var plugin in plugins) {
                nodes[plugin.Config.Name] = new PluginNode(plugin, plugin.Config.Dependencies.Length == 0);
            }

            if (plugins.Count != 0 && plugins.FirstOrDefault(v => v.Config.Dependencies.Length == 0) == null)
                throw new Exception("Can't load plugins, because there's no independent nodes!");

            foreach (var plugin in nodes) {
                foreach (var dep in plugin.Value.Node.Config.Dependencies) {
                    if (nodes.TryGetValue(dep, out var depNode)) {
                        plugin.Value.Dependencies.Add(depNode);
                        depNode.Dependents.Add(plugin.Value);
                    }
                    else
                        throw new Exception($"The plugin {dep} doesn't exist");
                }
            }

            foreach (var node in nodes) {
                CheckDependency(node.Value);
            }

            return nodes;
        }
        #endregion

        public static Config Config { get; private set; }


        private static Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();

        public static bool Initialised { get; private set; } = false;

        static void Main(string[] args)
        {
            Config = new Config();
            if (File.Exists("./config.json")) {
                Config = JsonSerializer.Deserialize<Config>(File.ReadAllText("./config.json"));
            }

            var a = ResolvePlugins();

            LoadPlugins(a);
            DisplayerPlugin?.Initialise();

            Initialised = true;

            UnloadPlugins(a);
            Initialised = false;
        }

        public static T GetPlugin<T>(string name) where T : IPlugin
        {
            if (plugins.TryGetValue(name, out var genericPlugin)) {
                if (genericPlugin is T plugin)
                    return plugin;
                else
                    throw new Exception(
                        $"The plugin {name} is of type {name.GetType()}, " +
                        $"but selected type was {typeof(T)}"
                    );
            }
            else
                throw new Exception($"Can't find the plugin {name}");
        }
        public static IWindowHost DisplayerPlugin { get; set; } = null;
    }
}
