using NetGL.GraphicsAPI;
using System;

namespace MinecraftNet.MenuSystem
{
    public interface IRenderable
    {
        Mesh Render(Graphics graphics);
    }
}
