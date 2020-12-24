using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftNet
{
    public class Model: ICloneable
    {
        public List<Vertice> Points { get; } = new List<Vertice>();
        public List<uint> Indicies { get; } = new List<uint>();

        public Model(Vertice[] points, uint[] indicies)
        {
            Points.AddRange(points);
            Indicies.AddRange(indicies);
        }
        public Model() : this(new Vertice[0], new uint[0]) { }

        public static Model Combine(params Model[] models)
        {
            var model = new Model(models[0].Points.ToArray(), models[0].Indicies.ToArray());

            for (int i = 1; i < models.Length; i++) {
                model.Indicies.AddRange(models[i].Indicies.Select(v => v + (uint)model.Indicies.Count));
                model.Points.AddRange(models[i].Points);
            }

            return model;
        }

        public Model Clone() => Model.Combine(this);
        object ICloneable.Clone() => Clone();

        public void Concatenate(params Model[] models)
        {
            foreach (var model in models) {
                Indicies.AddRange(model.Indicies.Select(v => v + (uint)Indicies.Count));
                Points.AddRange(model.Points);
            }
        }

        public static implicit operator HitboxModel(Model mdl)
        {
            return new HitboxModel(mdl.Points.Select(v => v.position).ToArray(), mdl.Indicies.ToArray());
        }

        public Mesh ToMesh(ShaderProgram shader, Graphics g, string translationMatrix = "meshMatrix")
        {
            var mesh = new Mesh(g, shader, transMatrixName: translationMatrix);

            mesh.LoadVertices(Points.ToArray(), Indicies.ToArray());

            return mesh;
        }
    }
}
