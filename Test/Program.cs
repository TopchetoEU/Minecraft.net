using System;
using System.Drawing;
using System.Linq;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;

namespace Test
{
    class Program
    {
        static Window wnd;
        static PrespectiveCamera camera;
        static Mesh b;

        static MouseControl mouseControl;
        static KeyboardControl keyboardControl;

        static Vector3 pos = new Vector3(0);
        static float lastPitch = 0, lastYaw = 0;

        private static void LockMode()
        {
            controlMode = !controlMode;
            if (controlMode)
                wnd.Mouse.Move(new VectorI2(0, 0));
            else {
            }
            wnd.LockCursor = !wnd.LockCursor;
        }

        static void Main(string[] args)
        {
            wnd = new Window("Test");


            wnd.Drawing += A_Drawing;
            wnd.Loaded += Wnd_Loaded;
            wnd.Mouse.Moved += (s, e) => {
                if (controlMode) {
                }
            };
            wnd.Keyboard.Pressed += (s, e) => {
                if (e.Key == Key.Escape)
                    LockMode();
            };

            wnd.ShowAsMain();
        }

        private static void Wnd_Loaded(object sender, GraphicsEventArgs e)
        {
            var scene = new Scene();
            camera = new PrespectiveCamera(50, .1f, 100);
            scene.Camera = camera;

            mouseControl = new MouseControl(wnd.Mouse);
            mouseControl.Smoothness = 1f;
            keyboardControl = new KeyboardControl(wnd.Keyboard);

            var vert = e.Graphics.CreateShaderFromFile(@"D:\shaders\test.vsh", ShaderType.Vertex);
            var geom = e.Graphics.CreateShaderFromFile(@"D:\shaders\test.gsh", ShaderType.Geometry);
            var frag = e.Graphics.CreateShaderFromFile(@"D:\shaders\test.fsh", ShaderType.Fragment);

            var texture3 = e.Graphics.CreateTexture2DFromBitmap(new Bitmap("D:/test3.png"));
            texture3.Interpolation = InterpolationType.Nearest;
            var shader = e.Graphics.CreateShaderProgram(vert, geom, frag);
            shader.ApplyUniform(texture3, "tex3");

            b = new Mesh(e.Graphics, shader);
            b.LoadVertices(
                new vertex[] {
                //front
                new vertex(0, 0, 0, 0, 0),
                new vertex(0, 1, 0, 0, 1),
                new vertex(1, 1, 0, 1, 1),
                new vertex(1, 0, 0, 1, 0),

                //back
                new vertex(0, 0, 1, 1, 0),
                new vertex(0, 1, 1, 1, 1),
                new vertex(1, 1, 1, 0, 1),
                new vertex(1, 0, 1, 0, 0),

                //right
                new vertex(1, 0, 0, 0, 0),
                new vertex(1, 1, 0, 0, 1),
                new vertex(1, 1, 1, 1, 1),
                new vertex(1, 0, 1, 1, 0),

                //left
                new vertex(0, 0, 0, 1, 0),
                new vertex(0, 1, 0, 1, 1),
                new vertex(0, 1, 1, 0, 1),
                new vertex(0, 0, 1, 0, 0),
                
                //bottom
                new vertex(0, 1, 0, 0, 0),
                new vertex(0, 1, 1, 0, 1),
                new vertex(1, 1, 1, 1, 1),
                new vertex(1, 1, 0, 1, 0),

                //top
                new vertex(0, 0, 0, 0, 1),
                new vertex(0, 0, 1, 0, 0),
                new vertex(1, 0, 1, 1, 0),
                new vertex(1, 0, 0, 1, 1),
            },
                new uint[] {
                0, 1, 2,  2, 3, 0, //front
                6, 5, 4,  4, 7, 6, //back
                8, 9, 10, 10, 11, 8, //left
                14, 13, 12, 12, 15, 14, //right
                16, 17, 18, 18, 19, 16, //top
                22, 21, 20, 20, 23, 22 //bottom
            }
            );
            b.Transformation = new Transform(
                new Vector3(1, 1, 0),
                b.Transformation.Rotation,
                b.Transformation.Scale,
                b.Transformation.TransformationCenter);

            scene.Meshes.Add(b);

            wnd.Scenes.Add(scene);

            wnd.Fullscreen(Monitor.PrimaryMonitor);

            e.Graphics.BackgorundColor = new Vector4(0, 0, 1, 1);

            LockMode();
            wnd.LockCursor = true;
        }

        struct vertex
        {
            public Vector3 position { get; set; }
            public Vector2 texCoord { get; set; }

            public vertex(float x, float y, float z, float s, float t)
            {
                position = new Vector3(x, y, z);
                texCoord = new Vector2(s, t);
            }
        }

        static int i = 0;
        static bool controlMode = false;

        private static void A_Drawing(object sender, CancellableGraphicsEventArgs e)
        {
            if (controlMode) {
            }

            mouseControl.Update();
            keyboardControl.Yaw = mouseControl.Rotation.Y;
            keyboardControl.Update(wnd.DeltaTime);

            ((IObject3D)camera).Position = keyboardControl.Position;
            ((IObject3D)camera).Rotation = mouseControl.Rotation;

            b.Program.ApplyUniform(camera.Transformation.Position, "cameraPos");

            e.Graphics.Clear(ClearType.ColorBuffer | ClearType.DepthBuffer);

            i++;
        }
    }
}
