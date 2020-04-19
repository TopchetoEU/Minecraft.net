using OpenTK.Input;

namespace Minecraft.MainWindow
{
    /// <summary>
    /// Arguments for keyboard event
    /// </summary>
    public class KeyboardEventArgs
    {
        /// <summary>
        /// The key causing the event
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// Creates new arguments for keyboard event
        /// </summary>
        /// <param name="key">The key causing this event</param>
        public KeyboardEventArgs(Key key)
        {
            Key = key;
        }
    }
}