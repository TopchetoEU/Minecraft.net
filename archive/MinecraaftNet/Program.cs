using MinecraftNetWindow;
using MinecraftNetWindow.Geometry;
using MinecraftNetWindow.Units;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNet
{
    class Program
    {
        static Window window;
        static Scene scene;
        static Mesh mesh;
        static Font font;

        [Obsolete()]
        static void boo()
        {

        }

        static void Main(string[] args)
        {
            boo();
            window = new Window();

            window.Setup += WindowSetup;
            window.BeforeDraw += WindowBeforeDraw;

            window.Run(0);
        }

        private static void WindowBeforeDraw(object sender, EventArgs e)
        {
            ((PrespectiveCamera)scene.Camera).Ratio = (float)window.Width / window.Height;

            mesh.Uniforms = new IUniform[] {
                new Vector2Uniform("windowSize", new Vector2(window.Width, window.Height)),
                new Matrix4Uniform("meshUniform", mesh.GetMatrix()),

            };
        }

        private static void WindowSetup(object sender, EventArgs e)
        {
            var shader = ShaderProgram.LoadFromFiles("text");
            mesh = new Mesh(shader);

            scene = new Scene();
            scene.Meshes.Add(mesh);
            scene.Camera = new PrespectiveCamera(70, .1f, 100f, 1);

            font = new Font(Path.Combine(Environment.CurrentDirectory, "fonts/Arial.bmp"));

            window.Scenes.Add(scene);
        }
    }
}
