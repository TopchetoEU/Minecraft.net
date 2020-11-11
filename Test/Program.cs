using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;

namespace Test
{
    class Program
    {
        static Window wnd;
        static PrespectiveCamera camera;

        static Vector3 pos = new Vector3(0);
        static float lastPitch = 0, lastYaw = 0;

        private static void LockMode()
        {
            controlMode = !controlMode;
            if (controlMode)
                wnd.MousePosition = new VectorI2(0, 0);
            else {
                lastPitch = pitch;
                lastYaw = yaw;
            }
            wnd.LockCursor = !wnd.LockCursor;
        }

        static void Main(string[] args)
        {
            wnd = new Window("Test");

            wnd.Drawing += A_Drawing;
            wnd.Loaded += Wnd_Loaded;
            wnd.MouseMoved += (s, e) => {
                if (controlMode) {
                    float percs = 15;

                    yaw = lastYaw -e.X / percs;
                    pitch = lastPitch + e.Y / percs;

                    camera.Transformation = new Transform(
                        camera.Transformation.Position,
                        new Vector3(pitch, yaw, camera.Transformation.Rotation.Z),
                        camera.Transformation.Scale,
                        camera.Transformation.TransformationCenter
                    );
                }
            };
            wnd.KeyPressed += (s, e) => {
                if (e.Key == Key.Escape)
                    LockMode();
            };
            wnd.SizeChanged += (s, e) => {
            };

            wnd.ShowAsMain();
        }

        private static void Wnd_Loaded(object sender, GraphicsEventArgs e)
        {
            var scene = new Scene();
            camera = new PrespectiveCamera(wnd, 50, .1f, 100);
            scene.Camera = camera;

            var vert = e.Graphics.CreateShader(@"#version 330 core
in vec3 position;
in vec2 texCoord;

out vec2 _texCoord;

void main() {
    gl_Position = vec4(position, 1);
    _texCoord = texCoord;
}
", ShaderType.Vertex);
            var geom = e.Graphics.CreateShader(@"#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in vec2 _texCoord[];

uniform mat4 matrix;
uniform mat4 cameraMatrix;
uniform mat4 cameraTransformMatrix;
uniform mat4 sceneMatrix;
uniform mat4 meshMatrix;

out vec2 geom_texCoord;
out vec3 geom_normal;
out vec3 geom_pos;

vec3 getNormal(vec3 a, vec3 b, vec3 c) {
    return normalize(cross(b - a, c - a));
}

void main() {
    vec3 currNormal = vec3(0);
    for (int i = 0; i < gl_in.length; i ++) {
        if (i % 3 == 0) currNormal = getNormal(
            (matrix * gl_in[i].gl_Position).xyz,
            (matrix * gl_in[i + 1].gl_Position).xyz,
            (matrix * gl_in[i + 2].gl_Position).xyz
        );
        gl_Position = cameraMatrix * cameraTransformMatrix * sceneMatrix * meshMatrix * gl_in[i].gl_Position;
        geom_pos = gl_Position.xyz;
        geom_texCoord = _texCoord[i];
        geom_normal = currNormal;
        EmitVertex();
    }
    EndPrimitive();
}
", ShaderType.Geometry);
            var frag = e.Graphics.CreateShader(@"#version 330 core
in vec2 geom_texCoord;
in vec3 geom_normal;
in vec3 geom_pos;
out vec4 color;

uniform sampler2D tex;
uniform sampler2D tex2;
uniform sampler2D tex3;

const vec3 lightPos = vec3(10, 20, 10);

void main() {
    vec2 tc1 = geom_texCoord;

    vec2 tc2 = geom_texCoord;

    float light = max(dot(-geom_normal, normalize(lightPos)), .2);
    vec4 a = texture(tex, tc1);
    vec4 b = texture(tex2, tc1);
    vec4 c = texture(tex3, tc2);

    float t = 1;

    vec4 ab = (a * (a.a) + b * (1 - a.a));
    vec4 abc = ab * (1 - t) + c * t;
    color = vec4(abc.rgb, 1);
}
", ShaderType.Fragment);

            var texture3 = e.Graphics.CreateTexture2DFromBitmap(new Bitmap("D:/test3.png"));
            texture3.Interpolation = InterpolationType.Nearest;
            var shader = e.Graphics.CreateShaderProgram(vert, geom, frag);
            shader.ApplyUniform(texture3, "tex3");

            var b = new Mesh(e.Graphics, shader);
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

            wnd.Size = new VectorI2(1280, 720);

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

        static float i = 0;
        static float pitch = 0;
        static float yaw = 0;
        static bool controlMode = false;
        private static void A_Drawing(object sender, CancellableGraphicsEventArgs e)
        {
            if (controlMode) {
                var d = new Vector3(0);
                float b = 1.5f;

                if (wnd.IsKeyPressed(Key.W))
                    d.Z -= b;
                if (wnd.IsKeyPressed(Key.S))
                    d.Z += b;
                if (wnd.IsKeyPressed(Key.A))
                    d.X += b;
                if (wnd.IsKeyPressed(Key.D))
                    d.X -= b;

                if (wnd.IsKeyPressed(Key.Space))
                    d.Y += b;
                if (wnd.IsKeyPressed(Key.LeftShift))
                    d.Y -= b;

                var a = Matrix4.CreateRotationY(yaw) * new Vector4(d, 0) * wnd.DeltaTime;
                Console.WriteLine(wnd.FPS);

                pos += a['x', 'y', 'z'];

                camera.Transformation = new Transform(
                    pos,
                    camera.Transformation.Rotation,
                    camera.Transformation.Scale,
                    camera.Transformation.TransformationCenter
                );
            }

            e.Graphics.Clear(ClearType.ColorBuffer | ClearType.DepthBuffer);

            i += 1;
        }
    }
}
