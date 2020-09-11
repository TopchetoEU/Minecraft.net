using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNetWindow.Geometry
{
    public class Scene
    {
        public ICamera Camera { get; set; }

        public bool AlphaBlending { get; set; } = false;
        public bool DepthTest { get; set; } = true;

        public List<Mesh> Meshes { get; } = new List<Mesh>();

        public void Draw()
        {
            if (DepthTest)     GL.Enable(EnableCap.DepthTest);
            else               GL.Disable(EnableCap.DepthTest);

            if (AlphaBlending) GL.Enable(EnableCap.Blend);
            else               GL.Disable(EnableCap.Blend);

            foreach (var mesh in Meshes)
            {
                mesh.Shader.Use();

                mesh.Shader.SetUniformMatrix(Camera.UniformLayouts.CameraMatrixName, Camera.GetCameraMatrix());
                mesh.Shader.SetUniformMatrix(Camera.UniformLayouts.ViewMatrixName,   Camera.GetViewMatrix());

                mesh.Draw();
            }
        }
    }
}