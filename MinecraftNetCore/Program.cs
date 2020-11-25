using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MinecraftNetCore
{
    class Program
    {
        public struct Vertice
        {
            public float LightLevel { get; set; }
            public Vector3 position { get; set; }
            public Vector2 TexCoord { get; set; }

            public Vertice(float x, float y, float z, float s = 0, float t = 0, float light = 1)
            {
                LightLevel = light;
                position = new Vector3(x, y, z);
                TexCoord = new Vector2(s, t);
            }
        }
        public enum Face
        {
            Top,
            Bottom,
            Left,
            Right,
            Front,
            Back
        }

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
        public class FaceModel
        {
            public Model Model { get; set; }
            public Face Face { get; set; }
            public bool Cull { get; set; }

            public FaceModel(Model model, Face face, bool cull = true)
            {
                Model = model;
                Face = face;
                Cull = cull;
            }
        }
        public abstract class BlockModel
        {
            public abstract FaceModel FrontFaceModel { get; }
            public abstract FaceModel BackFaceModel { get; }

            public abstract FaceModel TopFaceModel { get; }
            public abstract FaceModel BottomFaceModel { get; }

            public abstract FaceModel LeftFaceModel { get; }
            public abstract FaceModel RightFaceModel { get; }

            public abstract HitboxModel Hitbox { get; }

            Model GetBlockModel(bool top, bool bottom, bool left, bool right, bool front, bool back)
            {
                var newModel = new Model();

                if (!TopFaceModel.Cull || !top)
                    newModel.Concatenate(TopFaceModel.Model);
                if (!BottomFaceModel.Cull || !bottom)
                    newModel.Concatenate(BottomFaceModel.Model);

                if (!LeftFaceModel.Cull || !bottom)
                    newModel.Concatenate(LeftFaceModel.Model);
                if (!RightFaceModel.Cull || !bottom)
                    newModel.Concatenate(RightFaceModel.Model);

                if (!FrontFaceModel.Cull || !bottom)
                    newModel.Concatenate(FrontFaceModel.Model);
                if (!BackFaceModel.Cull || !bottom)
                    newModel.Concatenate(BackFaceModel.Model);

                return newModel;
            }
        }

        public class SolidBlockModel: BlockModel
        {
            public override FaceModel FrontFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 0, 0),
                        new Vertice(0, 1, 0),
                        new Vertice(1, 1, 0),
                        new Vertice(1, 0, 0),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Front
            );
            public override FaceModel BackFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(1, 1, 1),
                        new Vertice(1, 0, 1),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Back
            );

            public override FaceModel LeftFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(0, 1, 0),
                        new Vertice(0, 0, 0),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Right
            );
            public override FaceModel RightFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(0, 1, 0),
                        new Vertice(0, 0, 0),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Right
            );

            public override FaceModel TopFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 1, 0),
                        new Vertice(0, 1, 1),
                        new Vertice(1, 1, 1),
                        new Vertice(1, 1, 0),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Top
            );
            public override FaceModel BottomFaceModel { get; } = new FaceModel(
                    new Model(new Vertice[] {
                        new Vertice(0, 0, 0),
                        new Vertice(0, 0, 1),
                        new Vertice(1, 0, 1),
                        new Vertice(1, 0, 0),
                    },
                    new uint[] { 0, 1, 2, 0, 3, 2 }),
                Face.Bottom
            );

            public override HitboxModel Hitbox { get; } = new HitboxModel(
                new Vector3[] {
                    new Vector3(0, 0, 0), //0
                    new Vector3(0, 1, 0), //1
                    new Vector3(1, 1, 0), //2
                    new Vector3(1, 0, 0), //3

                    new Vector3(0, 0, 1), //4
                    new Vector3(0, 1, 1), //5
                    new Vector3(1, 1, 1), //6
                    new Vector3(1, 0, 1), //7
                },
                new uint[] {
                    0, 1, 2, 0, 3, 2, // front
                    4, 5, 6, 4, 7, 6, // back

                    0, 1, 5, 0, 4, 5, // left
                    3, 7, 6, 3, 2, 6, // right

                    1, 2, 6, 1, 7, 6, // top
                    0, 3, 7, 0, 4, 7, // back
                }
            );
        }

        public static class Blocks
        {

            private static Dictionary<string, HashSet<BlockType>> namespaces
                = new Dictionary<string, HashSet<BlockType>>();

            public static BlockType[] GetNamespace(string @namespace)
            {
                return namespaces[@namespace].ToArray();
            }

            public static bool TryGetNamespace(string @namespace, out BlockType[] blockTypes)
            {
                var b = namespaces.TryGetValue(@namespace, out var a);

                blockTypes = a.ToArray();

                return b;
            }
            public static bool NamespacExists(string @namespace)
            {
                return TryGetNamespace(@namespace, out _);
            }

            public static bool Exists(string nmspc, BlockType type)
            {
                return namespaces.TryGetValue(nmspc, out var val) && val.Contains(type);
            }
            public static bool Exists(BlockType type)
            {
                return namespaces.First(v => v.Value.Contains(type)).Value != null;
            }
            public static bool Exists(string id)
            {
                return TryGet(id, out _);
            }

            public static bool TryGet(string id, out BlockType type)
            {
                var identifier = new BlockIdentifier(id);
                if (TryGetNamespace(identifier.Namespace, out var types)) {
                    var firstType = types.First(v => v.Identifier == identifier);

                    type = null;
                    if (firstType == null)
                        return false;
                    else {
                        type = firstType;
                        return true;
                    }
                }
                else {
                    type = null;
                    return false;
                }
            }
            public static BlockType Get(string id)
            {
                var identifier = new BlockIdentifier(id);

                if (TryGetNamespace(identifier.Namespace, out var types)) {
                    var firstType = types.First(v => v.Identifier == identifier);

                    if (firstType == null)
                        throw new Exception($"The block {id} doesn't exist");
                    else
                        return firstType;
                }
                else
                    throw new Exception($"The block {id} doesn't exist");
            }

            public static bool AddBlock(BlockType type)
            {
                if (Exists(type)) {
                    throw new Exception($"The block {type.Identifier} already exists");
                }
                else {
                    if (namespaces.TryGetValue(type.Identifier.Namespace, out var hashSet)) {
                        return hashSet.Add(type);
                    }
                    else {
                        var set = new HashSet<BlockType>();
                        set.Add(type);

                        namespaces[type.Identifier.Namespace] = set;

                        return true;
                    }
                }
            }
            public static void RemoveBlock(BlockType type)
            {
                if (!Exists(type))
                    throw new Exception($"The block {type.Identifier} doesn't exist");
                else {
                    var set = namespaces[type.Identifier.Namespace];
                    set.Remove(type);
                }
            }
        }

        public class BlockIdentifier
        {
            public string Namespace { get; }
            public string Id { get; }

            public string DisplayName { get; set; }

            private bool IsIdentifier(string val)
            {
                return new Regex("[A-Za-z_]+").IsMatch(val);
            }

            public override bool Equals(object obj)
            {
                return obj is BlockIdentifier identifier &&
                       Namespace == identifier.Namespace &&
                       Id == identifier.Id;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Namespace, Id);
            }

            public BlockIdentifier(string nmspc, string id, string displayId = null)
            {
                if (!IsIdentifier(nmspc))
                    throw new Exception("Invalid namespace");
                if (!IsIdentifier(id))
                    throw new Exception("Invalid id");

                if (displayId == null)
                    displayId = nmspc + ":" + id;

                DisplayName = displayId;
                Namespace = nmspc;
                Id = id;
            }
            public BlockIdentifier(string id, string displayId = null)
            {
                if (new Regex("[A-Za-z_]+:[A-Za-z_]+").IsMatch(id)) {
                    var a = id.Split(':');

                    var nmspc = a[0];
                    var _id = a[1];

                    if (displayId == null)
                        displayId = nmspc + ":" + id;

                    DisplayName = displayId.ToLower();
                    Namespace = nmspc.ToLower();
                    Id = _id;
                }
                else
                    throw new Exception("Invalid ID");
            }

            public static bool operator ==(BlockIdentifier a, BlockIdentifier b) => a.Equals(b);
            public static bool operator !=(BlockIdentifier a, BlockIdentifier b) => !a.Equals(b);
        }
        public class BlockType
        {
            public BlockIdentifier Identifier { get; }
            public BlockModel Model { get; set; }
            public Texture2D Texture { get; }

            public BlockType(BlockIdentifier id, BlockModel model, Texture2D texture)
            {
                Identifier = id;
                Model = model;
                Texture = texture;
            }

            public override bool Equals(object obj)
            {
                return obj is BlockType type &&
                       EqualityComparer<BlockIdentifier>.Default.Equals(Identifier, type.Identifier);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Identifier);
            }

            public static bool operator ==(BlockType a, BlockType b) => a.Equals(b);
            public static bool operator !=(BlockType a, BlockType b) => !a.Equals(b);
        }

        public class Block
        {
            public BlockType Type { get; set; }
            public VectorI3 Location { get; set; }

            public Block(BlockType type, VectorI3 location)
            {
                Type = type;
                Location = location;
            }
        }

        public interface IWorld
        {
            Block GetBlock(VectorI3 location);
        }
        public class World: IWorld
        {
            public Block GetBlock(VectorI3 location)
            {
                throw new NotImplementedException();
            }
        }
        public class Chunk
        {

        }
        static void Main(string[] args)
        {
            var a = new Window("Minecraft.net");

            a.ShowAsMain();
        }
    }
}
