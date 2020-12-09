using NetGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftNetCore
{
    public class HitboxModel: ICloneable
    {
        public List<Vector3> Points { get; } = new List<Vector3>();
        public List<uint> Indicies { get; } = new List<uint>();

        public HitboxModel(Vector3[] points, uint[] indicies)
        {
            Points.AddRange(points);
            Indicies.AddRange(indicies);
        }
        public HitboxModel() : this(new Vector3[0], new uint[0]) { }

        public static HitboxModel Combine(params HitboxModel[] models)
        {
            var model = new HitboxModel(models[0].Points.ToArray(), models[0].Indicies.ToArray());

            for (int i = 1; i < models.Length; i++) {
                model.Indicies.AddRange(models[i].Indicies.Select(v => v + (uint)model.Indicies.Count));
                model.Points.AddRange(models[i].Points);
            }

            return model;
        }

        public HitboxModel Clone() => HitboxModel.Combine(this);
        object ICloneable.Clone() => Clone();

        public void Concatenate(params HitboxModel[] models)
        {
            foreach (var model in models) {
                Indicies.AddRange(model.Indicies.Select(v => v + (uint)Indicies.Count));
                Points.AddRange(model.Points);
            }
        }
    }
}
