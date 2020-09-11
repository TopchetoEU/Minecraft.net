using System;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using OpenTK;
using MinecraftNetWindow.Units;
using System.Collections.Generic;

namespace MinecraftNetWindow.Geometry
{
    public class CommonUnifromsLayout
    {
        public string CameraMatrixName { get; set; } = "cameraMatrix";
        public string ViewMatrixName { get; set; } = "viewMatrix";
        public string MeshMatrixName { get; set; } = "meshMatrix";
        public string WindowSizeName { get; set; } = "windowSize"; 
    }

    public class Mesh: IDisposable, ITransformable
    {
        private int VBO;
        private int EAO;
        private int VAO;

        private static int LastVBO;
        private static int LastEAO;
        private static int LastVAO;

        public int VerticesCount { get; private set; } = 0;
        public int IndiciesCount { get; private set; } = 0;

        public IndicieType IndicieType { get; private set; }
        public ArgumentMap[] ArgumentMaps { get; private set; }

        public IUniform[] Uniforms { get; set; }

        public Transformation Transformation { get; set; }
        public ShaderProgram Shader { get; set; }


        private bool render = true;

        public void LoadGeometry(IVertex[] vertices, IMeshIndicie[] indicies)
        {
            if (indicies.Length <= 0)
            {
                render = false;

                VerticesCount = vertices.Length;
                IndiciesCount = 0;
            }

            if (ArgumentMaps != null)
                foreach (var map in ArgumentMaps)
                {
                    GL.DisableVertexAttribArray(map.Location);
                }

            VerticesCount = vertices.Length;
            IndiciesCount = indicies.Length;

            render = true;

            ArgumentMaps = Shader.MapArguments(vertices[0].ShaderArguments);
            IndicieType = indicies[0].IndicieType;

            var verticesData = vertices.SelectMany(v => v.ToFloatAray()).ToArray();
            var indiciesData = indicies.SelectMany(v => v.ToUintArray()).ToArray();

            UseVAO();

            UseVBO();
            GL.BufferData(BufferTarget.ArrayBuffer, verticesData.Length * sizeof(float),
                verticesData, BufferUsageHint.StreamDraw);

            UseEAO();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indiciesData.Length * sizeof(uint),
                indiciesData, BufferUsageHint.StreamDraw);

            var stride = ArgumentMaps.Sum(v => v.Size) * sizeof(float);
            var offset = 0;

            foreach (var map in ArgumentMaps)
            {
                GL.EnableVertexAttribArray(map.Location);
                GL.VertexAttribPointer(map.Location, map.Size, VertexAttribPointerType.Float, false,
                    stride, offset);

                offset += map.Size * sizeof(float);
            }
        }

        public Matrix4 GetMatrix()
        {
            return Transformation.GetMatrix();
        }

        private void UseVBO()
        {
            if (VBO != LastVBO) GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            LastVBO = VBO;
        }
        private void UseVAO()
        {
            if (VAO != LastVAO) GL.BindVertexArray(VAO);
            LastVAO = VAO;
        }
        private void UseEAO()
        {
            if (EAO != LastEAO) GL.BindBuffer(BufferTarget.ElementArrayBuffer, EAO);
            LastEAO = EAO;
        }

        public void Use()
        {
            UseVBO();
            UseVAO();
            UseEAO();
        }

        public void Draw()
        {
            if (render)
            {
                foreach (var uniform in Uniforms)
                {
                    uniform.ApplyToShader(Shader);
                }

                Use();

                ushort indicieTypeCount = 1;

                switch (IndicieType)
                {
                    case IndicieType.Points:    indicieTypeCount = 1; break;
                    case IndicieType.Lines:     indicieTypeCount = 2; break;
                    case IndicieType.Triangles: indicieTypeCount = 3; break;
                    case IndicieType.Quads:     indicieTypeCount = 4; break;
                }

                Shader.Use();
                GL.DrawElements(PrimitiveType.Points, IndiciesCount * indicieTypeCount, DrawElementsType.UnsignedInt, 0);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DeleteBuffer(VBO);
                    GL.DeleteVertexArray(VAO);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        public Mesh(ShaderProgram shader)
        {
            VBO = GL.GenBuffer();
            EAO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            Shader = shader;
        }
    }
}
