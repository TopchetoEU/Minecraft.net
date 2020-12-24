namespace MinecraftNet.MenuSystem
{
    public interface IMouseCatcher
    {
        void MoveMouse(int x, int y);
        void PressMouse(MouseButton button);
        void ReleaseMouse(MouseButton button);
    }
}
