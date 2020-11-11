using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NetGL.GraphicsAPI
{
    public class ShaderProgram: IDisposable, IUsable
    {
        private bool disposedValue;

        public uint Id { get; }
        private List<Shader> attachedShaders = new List<Shader>();

        public Shader[] AttachedShaders {
            get {
                var a = new Shader[attachedShaders.Count];
                attachedShaders.CopyTo(a);

                return a;
            }
        }

        private void AttachShader(Shader shader)
        {
            LLGraphics.graphics_attachShaderToProgram(Id, shader.Id);

            attachedShaders.Add(shader);
        }
        private void DetachShader(Shader shader)
        {
            int index = attachedShaders.FindIndex(v => v.Id == shader.Id);
            if (index < 0)
                throw new Exception(
                    "The shader could not be detached," +
                    "since it wasn't attached to the program in the first place"
                );

            LLGraphics.graphics_detachShaderFromProgram(Id, shader.Id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LLGraphics.graphics_destroyShaderProgram(Id);
                    attachedShaders = null;
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Use()
        {
            LLGraphics.graphics_setShaderProgram(Id);
        }


        public delegate bool EqualsDelegate<T>(T a, T b);
        public delegate int HashDelegate<T>([NotNull] T val);
        public class CustomEqualityComparer<T>: IEqualityComparer<T>
        {
            private EqualsDelegate<T> equals = null;
            private HashDelegate<T> hash = null;

            public CustomEqualityComparer(EqualsDelegate<T> equals, HashDelegate<T> hash = null)
            {
                this.equals = equals;
                if (hash != null)
                    this.hash = hash;
                else this.hash = (v) => v.GetHashCode();
            }

            public bool Equals([AllowNull] T x, [AllowNull] T y)
            {
                if (equals != null) return equals(x, y);
                else return x.Equals(y);
            }
            public int GetHashCode([DisallowNull] T obj)
            {
                if (hash != null) return hash(obj);
                else return obj.GetHashCode();
            }
        }

        internal ShaderProgram(params Shader[] shaders)
        {
            Id = LLGraphics.graphics_createShaderProgram();

            shaders = shaders.Distinct(new CustomEqualityComparer<Shader>((a, b) => a.Id == b.Id)).ToArray();

            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }

            LLGraphics.graphics_compileShaderProgram(Id);

            foreach (var shader in shaders)
            {
                DetachShader(shader);
            }
        }

        public void ApplyUniform(IMatrix<float> matrix, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            Use();

            if (matrix.GetType().IsMatrix(out var attr))
            {
                var fl = matrix.Flattern();

                var index = LLGraphics.graphics_getUniformLocation(Id, name);

                switch (attr.Width + attr.Height * 10)
                {
                    case 22:
                        LLGraphics.graphics_setUniformMat2(index, fl[0], fl[1], fl[2], fl[3]);
                        break;
                    case 33:
                        LLGraphics.graphics_setUniformMat3(index,
                            fl[0], fl[1], fl[2],
                            fl[3], fl[4], fl[5],
                            fl[6], fl[7], fl[8]);
                        break;
                    case 44:
                        LLGraphics.graphics_setUniformMat4(index,
                            fl[0], fl[1], fl[2], fl[3],
                            fl[4], fl[5], fl[6], fl[7],
                            fl[8], fl[9], fl[10], fl[11],
                            fl[12], fl[13], fl[14], fl[15]);
                        break;
                    default:
                        throw new Exception("Matrix not supported");
                }
            }
        }
        public void ApplyUniform(IMatrix<double> matrix, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            Use();

            if (matrix.GetType().IsMatrix(out var attr))
            {
                var fl = matrix.Flattern();
                var index = LLGraphics.graphics_getUniformLocation(Id, name);

                switch (attr.Width + attr.Height * 10)
                {
                    case 22:
                        LLGraphics.graphics_setUniformMatd2(index, fl[0], fl[1], fl[2], fl[3]);
                        break;
                    case 33:
                        LLGraphics.graphics_setUniformMatd3(index,
                            fl[0], fl[1], fl[2],
                            fl[3], fl[4], fl[5],
                            fl[6], fl[7], fl[8]);
                        break;
                    case 44:
                        LLGraphics.graphics_setUniformMatd4(index,
                            fl[0], fl[1], fl[2], fl[3],
                            fl[4], fl[5], fl[6], fl[7],
                            fl[8], fl[9], fl[10], fl[11],
                            fl[12], fl[13], fl[14], fl[15]);
                        break;
                    default:
                        throw new Exception("Matrix not supported");
                }
            }
        }

        public void ApplyUniform(IVector<float> vector, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            Use();

            if (vector.GetType().IsVector(out var attr))
            {
                var fl = vector.Flattern();
                var index = LLGraphics.graphics_getUniformLocation(Id, name);

                switch (attr.Dimensions)
                {
                    case 2:
                        LLGraphics.graphics_setUniformVec2(index, fl[0], fl[1]);
                        break;
                    case 3:
                        LLGraphics.graphics_setUniformVec3(index, fl[0], fl[1], fl[2]);
                        break;
                    case 4:
                        LLGraphics.graphics_setUniformVec4(index, fl[0], fl[1], fl[2], fl[3]);
                        break;
                    default:
                        throw new Exception("Vector not supported");
                }
            }
        }
        public void ApplyUniform(IVector<int> vector, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            Use();

            if (vector.GetType().IsVector(out var attr))
            {
                var fl = vector.Flattern();
                var index = LLGraphics.graphics_getUniformLocation(Id, name);

                switch (attr.Dimensions)
                {
                    case 2:
                        LLGraphics.graphics_setUniformiVec2(index, fl[0], fl[1]);
                        break;
                    case 3:
                        LLGraphics.graphics_setUniformiVec3(index, fl[0], fl[1], fl[2]);
                        break;
                    case 4:
                        LLGraphics.graphics_setUniformiVec4(index, fl[0], fl[1], fl[2], fl[3]);
                        break;
                    default:
                        throw new Exception("Vector not supported");
                }
            }
        }
        public void ApplyUniform(IVector<double> vector, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            Use();

            if (vector.GetType().IsVector(out var attr))
            {
                var fl = vector.Flattern();
                var index = LLGraphics.graphics_getUniformLocation(Id, name);

                switch (attr.Dimensions)
                {
                    case 2:
                        LLGraphics.graphics_setUniformdVec2(index, fl[0], fl[1]);
                        break;
                    case 3:
                        LLGraphics.graphics_setUniformdVec3(index, fl[0], fl[1], fl[2]);
                        break;
                    case 4:
                        LLGraphics.graphics_setUniformdVec4(index, fl[0], fl[1], fl[2], fl[3]);
                        break;
                    default:
                        throw new Exception("Vector not supported");
                }
            }
        }
        public void ApplyUniform(Texture2D texture, string name)
        {
            if (name == null)
                throw new ArgumentException("Name can't be null", "name");
            var id = LLGraphics.graphics_getUniformLocation(Id, name);
            Use();
            texture.Bind(id);
        }
    }
}
