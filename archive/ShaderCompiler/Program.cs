using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCompiler
{
    class Program: GameWindow
    {
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Close();
            base.OnKeyDown(e);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Pick a shader path:");
            var vshaderpath = Console.ReadLine();

            var typesShit = Enum.GetValues(typeof(ShaderType));
            var types = new ShaderType[typesShit.Length];
            typesShit.CopyTo(types, 0);

            foreach (var type in types)
            {
                Console.WriteLine(type);
            }

            var i = int.Parse(Console.ReadLine());

            var a = new GameWindow();
            a.Run();

            while (true)
            {
                var glsl = File.ReadAllText(vshaderpath);

                int id = GL.CreateShader(types[i]);
                GL.ShaderSource(id, glsl);

                GL.CompileShader(id);

                string infoLogFrag = GL.GetShaderInfoLog(id);

                if (infoLogFrag != String.Empty)
                    Console.WriteLine(infoLogFrag);

                Console.ReadKey();
            }

        }
    }
}
