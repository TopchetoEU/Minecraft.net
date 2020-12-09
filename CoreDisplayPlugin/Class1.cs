using MinecraftNetCore;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System.Collections.Generic;
using MouseButton = MinecraftNetCore.MouseButton;

namespace CoreDisplayPlugin
{
    public interface IRenderable
    {
        void Render(Graphics graphics);
    }

    public interface IMenuChain
    {
        IMenu this[int index] { get; set; }

        void GetTopMenu();
        void CloseTopMenu();
        void ShowMenuOnTop(IMenu menu);
    }
    public interface IMenu: IMouseCatcher, IRenderable
    {
    }

    public enum ControlType
    {
        Input,
        Label,
        Switch,
        MultiOptionButton,
        Slider,
        MenuButton,
    }

    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
        Stretch
    }
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom,
        Stretch
    }

    public class CollectionChangeEventArgs<T>
    {
        public int Index { get; }
        public T Element { get; }

        public CollectionChangeEventArgs(int index, T element)
        {
            Index = index;
            Element = element;
        }
    }

    public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangeEventArgs<T> e);

    public interface IContainerControl: IRenderable, IMouseCatcher, IKeyboardCatcher
    {
        ObservedCollection<IControl> Children { get; set; }
    }
    public interface IControl: IRenderable, IMouseCatcher, IKeyboardCatcher
    {
        string Text { get; set; }

        HorizontalAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }


    }

    public class TextRenderer
    {
        public void DrawText(string text) { }
    }

    public interface IMouseCatcher
    {
        void MoveMouse(int x, int y);
        void PressMouse(MouseButton button);
        void ReleaseMouse(MouseButton button);
    }
    public interface IKeyboardCatcher
    {
        void PressKey(Key key);
        void ReleaseKey(Key key);
    }

    public class CoreDisplayer: IPlugin, IWindowHost
    {
        public Window MainWindow { get; private set; }

        public void Initialise()
        {
            MainWindow = new Window("Minecraft.net", new VectorI2(1280, 720));

            MainWindow.ShowAsMain();
        }

        public void Load()
        {
            Program.DisplayerPlugin = this;
        }

        public void Unload()
        {
            MainWindow.Dispose();
        }
    }
}
