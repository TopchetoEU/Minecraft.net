using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftNet.MenuSystem
{
    public class GenericMenu: IMenu
    {
        public IUnalignedContainerControl InnerContainer { get; set; }

        public void MoveMouse(int x, int y)
        {
            InnerContainer.MoveMouse(x, y);
        }
        public void PressMouse(MouseButton button)
        {
            InnerContainer.PressMouse(button);
        }
        public void ReleaseMouse(MouseButton button)
        {
            InnerContainer.ReleaseMouse(button);
        }

        public void PressKey(Key key)
        {
            InnerContainer.PressKey(key);
        }
        public void ReleaseKey(Key key)
        {
            InnerContainer.ReleaseKey(key);
        }

        public void Render(Graphics graphics)
        {
            InnerContainer.Render(graphics);
        }

        public GenericMenu(IUnalignedContainerControl container)
        {
            InnerContainer = container;

            container.LayoutChanged += Container_LayoutChanged;
        }

        private void Container_LayoutChanged(object sender, EventArgs e)
        {
            Program.GetPlugin<CoreDisplayer>("core-displayer").AddToRenderQueue(this);
        }
    }
}
