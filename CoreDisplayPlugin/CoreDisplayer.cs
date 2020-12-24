using MinecraftNet.MenuSystem;
using MinecraftNet;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System.Collections.Generic;
using MouseButton = MinecraftNet.MouseButton;
using System;

namespace MinecraftNet
{
    public class CoreDisplayer: IPlugin, IWindowHost
    {
        internal static CoreDisplayer Instance;

        public Window MainWindow { get; private set; }
        private Dictionary<string, IMenu> menus = new Dictionary<string, IMenu>();

        public MenuChain MenuChain { get; }

        public bool RegisterMenu(string name, IMenu menu)
        {
            return menus.TryAdd(name, menu);
        }
        public IMenu GetMenu(string name)
        {
            return menus.TryGetValue(name, out var menu) ? menu : null;
        }

        public void Initialise()
        {
            MainWindow = new Window("Minecraft.net", new VectorI2(1280, 720));

            Instance = this;

            MainWindow.Drawn += MainWindow_Drawn;

            MainWindow.ShowAsMain();
        }

        private void MainWindow_Drawn(object sender, GraphicsEventArgs e)
        {
            while (renderableQueue.Count > 0) {
                renderableQueue.Dequeue().Render(e.Graphics);
            }
        }

        public void Load()
        {
            Program.DisplayerPlugin = this;
        }
        public void Unload()
        {
            MainWindow.Dispose();
        }

        private Queue<IStaticRenderable> renderableQueue = new Queue<IStaticRenderable>();

        public void AddToRenderQueue(IStaticRenderable renderable)
        {
            renderableQueue.Enqueue(renderable);
        }
    }
}
