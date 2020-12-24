using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;

namespace MinecraftNet.MenuSystem
{
    public class MenuChain: IStaticRenderable
    {
        private Stack<IMenu> menus = new Stack<IMenu>();

        public IMenu CloseTopMenu()
        {
            var popped = menus.Pop();

            CoreDisplayer.Instance.AddToRenderQueue(this);

            return popped;
        }
        public void OpenTopMenu(IMenu menu)
        {
            menus.Push(menu);
            CoreDisplayer.Instance.AddToRenderQueue(this);
        }
        public IMenu GetTopMenu()
        {
            return menus.Peek();
        }

        public void Render(Graphics graphics)
        {
            GetTopMenu().Render(graphics);
        }
    }
}
