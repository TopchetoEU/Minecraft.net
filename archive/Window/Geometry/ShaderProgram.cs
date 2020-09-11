using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MinecraftNetWindow.Geometry
{
    public class ShaderProgram: IDisposable
    {
        public static ShaderPathFormatter DefaultShaderPathFormatter = delegate (string currentDirectory, string shaderName, ShaderType shaderType)
        {
            return $"{currentDirectory}//shaders//{shaderName}.{shaderType.ToString()[0].ToString().ToLower()}sh";
        };

        public static void UseEmpty()
        {
            if (BoundID != 0) GL.UseProgram(0);
            BoundID = 0;
        }
        private static int BoundID = 0;
        public int ID { get; private set; }

        Shader[] Shaders;

        public static ShaderProgram LoadFromFiles(string shaderName, ShaderPathFormatter pathFormatter = null)
        {
            var formatter = pathFormatter ?? DefaultShaderPathFormatter;

            var shaders = new List<Shader>();

            var vertexPath = formatter(Environment.CurrentDirectory, shaderName, ShaderType.VertexShader);
            var fragmentPath = formatter(Environment.CurrentDirectory, shaderName, ShaderType.FragmentShader);
            var geometryPath = formatter(Environment.CurrentDirectory, shaderName, ShaderType.GeometryShader);

            if (File.Exists(vertexPath))
            {
                var file = File.ReadAllText(vertexPath);
                shaders.Add(new Shader(file, ShaderType.VertexShader));
            }

            if (File.Exists(fragmentPath))
            {
                var file = File.ReadAllText(fragmentPath);
                shaders.Add(new Shader(file, ShaderType.FragmentShader));
            }

            if (File.Exists(geometryPath))
            {
                var file = File.ReadAllText(geometryPath);
                shaders.Add(new Shader(file, ShaderType.GeometryShader));
            }

            return new ShaderProgram(shaders.ToArray());
        }

        public ShaderProgram(Shader[] shaders)
        {
            Shaders = shaders;
            ID = GL.CreateProgram();

            foreach (var shader in Shaders)
            {
                AttachShader(shader);
                Console.WriteLine(shader.Type.ToString() + ":");
                Console.WriteLine(shader.BuildResult);
            }

            GL.LinkProgram(ID);
        }

        public void Use()
        {
            if (BoundID != ID) GL.UseProgram(ID);
            BoundID = ID;
        }

        public void AttachShader(Shader shader)
        {
            GL.AttachShader(ID, shader.ID);
        }
        public void DetachShader(Shader shader)
        {
            GL.DetachShader(ID, shader.ID);
        }

        #region Uniforms
        public void SetUniform(string name, int value)
        {
            GL.UseProgram(ID);
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform1(location, value);
        }
        public void SetUniform(string name, float value)
        {
            GL.UseProgram(ID);
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform1(location, value);
        }
        public void SetUniform(string name, float x, float y)
        {
            GL.UseProgram(ID);
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform2(location, x, y);
        }
        public void SetUniform(string name, float x, float y, float z)
        {
            GL.UseProgram(ID);
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform3(location, x, y, z);
        }
        public void SetUniform(string name, float x, float y, float z, float w)
        {
            GL.UseProgram(ID);
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform4(location, x, y, z, w);
        }

        public void SetUniformMatrix(string name, Matrix2 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix2(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix2x3 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix2x3(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix2x4 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix2x4(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix3 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix3(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix3x2 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix3x2(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix3x4 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix3x4(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix4 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix4(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix4x2 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix4x2(pos, false, ref matrix);
        }
        public void SetUniformMatrix(string name, Matrix4x3 matrix)
        {
            Use();
            var pos = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix4x3(pos, false, ref matrix);
        }

        #endregion

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(ID, name);
        }

        public ArgumentMap[] MapArguments(ShaderArgumentMap[] arguments)
        {
            return arguments.Select(v => v.Map(this)).ToArray();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var shader in Shaders)
                    {
                        DetachShader(shader);
                        shader.Dispose();
                    }

                    UseEmpty();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public delegate string ShaderPathFormatter(string currentDirectory, string shaderName, ShaderType shaderType);
}