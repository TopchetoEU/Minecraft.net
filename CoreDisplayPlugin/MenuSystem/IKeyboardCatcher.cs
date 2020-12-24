using NetGL.WindowAPI;

namespace MinecraftNet.MenuSystem
{
    public interface IKeyboardCatcher
    {
        void PressKey(Key key);
        void ReleaseKey(Key key);
    }
}
