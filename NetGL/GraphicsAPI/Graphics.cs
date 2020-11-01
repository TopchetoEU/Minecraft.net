using NetGL.WindowAPI;

namespace NetGL.GraphicsAPI
{
    public class Graphics
    {
        private Window attachedWindow;

        public Vector4 BackgroundColor {
            get {
                float r = 0, g = 0, b = 0, a = 0;

                attachedWindow.Use();
                LLGraphics.graphics_getBackground(ref r, ref g, ref b, ref a);

                return new Vector4(r, g, b, a);
            }
            set {
                attachedWindow.Use();
                LLGraphics.graphics_setBackground(value.X, value.Y, value.Z, value.W);
            }
        }

        public void Clear()
        {
            attachedWindow.Use();
            LLGraphics.graphics_clear(0x00004000);
        }

        public Graphics(Window window)
        {
            attachedWindow = window;
        }
    }
}
