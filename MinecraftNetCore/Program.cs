using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MinecraftNetCore
{
    class Program
    {
        public class ByteWriteStream
        {
            private List<byte> bytes = new List<byte>();

            public ByteWriteStream Write(byte value)
            {
                bytes.Add(value);
                return this;
            }
            public ByteWriteStream Write(IEnumerable<byte> values)
            {
                bytes.AddRange(values);
                return this;
            }
            public ByteWriteStream Write(params byte[] values)
            {
                bytes.AddRange(values);
                return this;
            }

            public ByteWriteStream Write(long value)
            {
                bytes.AddRange(BitConverter.GetBytes(value));
                return this;
            }
            public ByteWriteStream Write(int value)
            {
                bytes.AddRange(BitConverter.GetBytes(value));
                return this;
            }
            public ByteWriteStream Write(short value)
            {
                bytes.AddRange(BitConverter.GetBytes(value));
                return this;
            }

            public ByteWriteStream Write(bool value)
            {
                bytes.AddRange(BitConverter.GetBytes(value));
                return this;
            }

            public ByteWriteStream Write(char value)
            {
                bytes.Add((byte)value);
                return this;
            }
            public ByteWriteStream Write(IEnumerable<char> values)
            {
                bytes.AddRange(values.Select(v => (byte)v));
                return this;
            }
            public ByteWriteStream Write(params char[] values)
            {
                bytes.AddRange(values.Select(v => (byte)v));
                return this;
            }

            public ByteWriteStream Write(string value)
            {
                var tempValue = value;
                if (value == null)
                    tempValue = "";
                Write(tempValue.ToCharArray());
                return this;
            }

            public byte[] Flush()
            {
                var a = bytes.ToArray();
                bytes.Clear();

                return a;
            }
            public string FlushString()
            {
                return new string(Flush().Select(v => (char)v).ToArray());
            }
        }
        public class ByteReadStream
        {
            private byte[] bytes;
            int index = 0;

            public ByteReadStream(string data)
            {
                bytes = data.ToCharArray().Select(v => (byte)v).ToArray();
            }
            public ByteReadStream(byte[] data)
            {
                bytes = (byte[])data.Clone();
            }

            public byte ReadByte()
            {
                return ReadByteArray(1)[0];
            }
            public byte[] ReadByteArray(int amount)
            {
                var array = new byte[amount];

                for (int i = 0; i < amount; i++) {
                    if (index >= bytes.Length)
                        throw new Exception("Reached the end of the array");
                    array[i] = bytes[index];
                    index++;

                }

                return array;
            }

            public long ReadLong()
            {
                return BitConverter.ToInt64(ReadByteArray(8));
            }
            public int ReadInt()
            {
                return BitConverter.ToInt32(ReadByteArray(4));
            }
            public short ReadShort()
            {
                return BitConverter.ToInt16(ReadByteArray(2));
            }

            public bool ReadBool()
            {
                return BitConverter.ToBoolean(ReadByteArray(1));
            }

            public char ReadChar()
            {
                return BitConverter.ToChar(ReadByteArray(1));
            }
            public char[] ReadCharArray(int? length = null)
            {
                if (!length.HasValue)
                    length = bytes.Length - index;

                return ReadByteArray(length.Value).Select(v => (char)v).ToArray();
            }
            public string ReadString(int? length = null)
            {
                return new string(ReadCharArray(length));
            }
        }

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

            public Model GetBlockModel(bool top, bool bottom, bool left, bool right, bool front, bool back)
            {
                var newModel = new Model();

                if (TopFaceModel != null && (!TopFaceModel.Cull || !top))
                    newModel.Concatenate(TopFaceModel.Model);
                if (BottomFaceModel != null && (!BottomFaceModel.Cull || !bottom))
                    newModel.Concatenate(BottomFaceModel.Model);

                if (LeftFaceModel != null && (!LeftFaceModel.Cull || !left))
                    newModel.Concatenate(LeftFaceModel.Model);
                if (RightFaceModel != null && (!RightFaceModel.Cull || !right))
                    newModel.Concatenate(RightFaceModel.Model);

                if (FrontFaceModel != null && (!FrontFaceModel.Cull || !front))
                    newModel.Concatenate(FrontFaceModel.Model);
                if (BackFaceModel != null && (!BackFaceModel.Cull || !back))
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
        public class EmptyModel: BlockModel
        {
            public override FaceModel FrontFaceModel => null;

            public override FaceModel BackFaceModel => null;

            public override FaceModel TopFaceModel => null;

            public override FaceModel BottomFaceModel => null;

            public override FaceModel LeftFaceModel => null;

            public override FaceModel RightFaceModel => null;

            public override HitboxModel Hitbox => null;
        }

        public static class BlockTypes
        {
            private static Dictionary<string, HashSet<BlockType>> namespaces
                = new Dictionary<string, HashSet<BlockType>>();

            private static string mainNamespace = "minecraft";
            public static string MainNamespace {
                get => mainNamespace;
                set {
                    if (!Regex.IsMatch(value, "^([A-Za-z_]+)$"))
                        throw new ArgumentException(
                            "The namespace given doesn't match the identifier format (A-Za-z_)",
                            "value"
                        );

                    if (mainNamespace != value) {
                        if (NamespaceExists(value))
                            throw new ArgumentException(
                                "The namespace already exists! The main namespace can be transported" +
                                "only to an empty namespace",
                                "value"
                            );

                        var keySet = namespaces[mainNamespace];
                        namespaces.Remove(mainNamespace);
                        namespaces[value] = keySet;

                        mainNamespace = value;

                        foreach (var key in keySet) {
                            key.Identifier = new BlockIdentifier(
                                mainNamespace, key.Identifier.Name, key.Identifier.DisplayName
                            );
                        }
                    }
                }
            }

            public static BlockType[] GetNamespace(string @namespace)
            {
                return namespaces[@namespace].ToArray();
            }
            public static BlockType[] GetAll()
            {
                return namespaces.SelectMany(v => v.Value).ToArray();
            }

            public static bool TryGetNamespace(string @namespace, out BlockType[] blockTypes)
            {
                var b = namespaces.TryGetValue(@namespace, out var a);

                blockTypes = null;
                if (a != null)
                    blockTypes = a.ToArray();

                return b;
            }
            public static bool NamespaceExists(string @namespace)
            {
                return TryGetNamespace(@namespace, out _);
            }

            public static bool Exists(string nmspc, BlockType type)
            {
                return namespaces.TryGetValue(nmspc, out var val) && val.Contains(type);
            }
            public static bool Exists(BlockType type)
            {
                return namespaces.FirstOrDefault(v => v.Value.Contains(type)).Value != null;
            }
            public static bool Exists(string id)
            {
                return TryGet(id, out _);
            }

            public static bool TryGet(string id, out BlockType type)
            {
                var identifier = new BlockIdentifier(id);
                if (TryGetNamespace(identifier.Namespace, out var types)) {
                    var firstType = types.FirstOrDefault(v => v.Identifier == identifier);

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
            public static bool TryGet(BlockIdentifier identifier, out BlockType type)
            {
                if (TryGetNamespace(identifier.Namespace, out var types)) {
                    var firstType = types.FirstOrDefault(v => v.Identifier == identifier);

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
            public static bool TryGet(int id, out BlockType type)
            {
                type = null;
                return TryGetIdentifier(id, out var identifier) && TryGet(identifier, out type);
            }

            public static BlockType Get(string id) => Get(new BlockIdentifier(id));
            public static BlockType Get(int id) => Get(GetIdentifier(id));
            public static BlockType Get(BlockIdentifier identifier)
            {
                if (TryGetNamespace(identifier.Namespace, out var types)) {
                    var firstType = types.FirstOrDefault(v => v.Identifier == identifier);

                    if (firstType == null)
                        throw new Exception($"The block {identifier} doesn't exist");
                    else
                        return firstType;
                }
                else
                    throw new Exception($"The block {identifier} doesn't exist");
            }

            public static BlockIdentifier GetIdentifier(int id)
            {
                var block = GetAll().FirstOrDefault(v => v.Identifier.Id == id);

                if (block == null)
                    throw new Exception("The block doesn't exist");
                else
                    return block.Identifier;
            }
            public static BlockIdentifier GetIdentifier(string id) => Get(new BlockIdentifier(id)).Identifier;

            public static bool TryGetIdentifier(int id, out BlockIdentifier identifier)
            {
                var block = GetAll().FirstOrDefault(v => v.Identifier.Id == id);

                identifier = null;

                if (block == null)
                    return false;
                else {
                    identifier = block.Identifier;
                    return true;
                }
            }
            public static bool TryGetIdentifier(string id, out BlockIdentifier identifier)
            {
                var b = TryGet(id, out var a);
                identifier = a.Identifier;
                return b;
            }

            private static int nextId = 0;
            private static Queue<int> freeBlocks = new Queue<int>();

            private static int GetAndAllocateNextID()
            {
                if (freeBlocks.Count > 0)
                    return freeBlocks.Dequeue();

                else
                    return nextId++;
            }
            private static void DeallocateID(int id)
            {
                freeBlocks.Enqueue(id);
            }

            public static bool AddBlock(BlockType type)
            {
                if (type.Identifier.Id != -1)
                    throw new Exception("The block has been already added!");

                if (Exists(type)) {
                    throw new Exception($"The block {type.Identifier} already exists");
                }
                else {
                    if (namespaces.TryGetValue(type.Identifier.Namespace, out var hashSet)) {
                        type.Identifier.Id = GetAndAllocateNextID();
                        return hashSet.Add(type);
                    }
                    else {
                        var set = new HashSet<BlockType>();
                        set.Add(type);

                        type.Identifier.Id = GetAndAllocateNextID();

                        namespaces[type.Identifier.Namespace] = set;

                        return true;
                    }
                }
            }
            public static void RemoveBlock(BlockType type)
            {
                if (type.Identifier.Id == -1)
                    throw new Exception("The block isn't registered");
                if (!Exists(type))
                    throw new Exception($"The block {type.Identifier} doesn't exist");
                else {
                    var set = namespaces[type.Identifier.Namespace];
                    set.Remove(type);
                    DeallocateID(type.Identifier.Id);
                }
            }

            static BlockTypes()
            {
                namespaces.Add(MainNamespace, new HashSet<BlockType>());
            }
        }

        public class BlockIdentifier
        {
            public const string IdentifierRegEx = "^([A-Za-z_]+";

            public string Namespace { get; }
            public string Name { get; }

            public int Id { get; internal set; } = -1;

            public string DisplayName { get; set; }

            private bool IsIdentifier(string val)
            {
                return new Regex("[A-Za-z_]+").IsMatch(val);
            }

            public override string ToString()
            {
                if (DisplayName.Equals($"{Namespace}:{Name}"))
                    return $"{Namespace}:{Name}";
                else
                    return $"{Namespace}:{Name} ({DisplayName})";
            }

            public override bool Equals(object obj)
            {
                return obj is BlockIdentifier identifier &&
                       Namespace == identifier.Namespace &&
                       Name == identifier.Name;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Namespace, Name);
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
                Name = id;
            }
            public BlockIdentifier(string id, string displayId = null)
            {
                if (new Regex("^([A-Za-z_]+:[A-Za-z_]+)$").IsMatch(id)) {
                    var a = id.Split(':');

                    var nmspc = a[0];
                    var _id = a[1];

                    if (displayId == null)
                        displayId = nmspc + ":" + _id;

                    DisplayName = displayId;
                    Namespace = nmspc.ToLower();
                    Name = _id.ToLower();
                }
                else if (new Regex("^([A-Za-z_]+)$").IsMatch(id)) {
                    var nmspc = BlockTypes.MainNamespace;
                    var _id = id;

                    if (displayId == null)
                        displayId = nmspc + ":" + _id;

                    DisplayName = displayId;
                    Namespace = nmspc.ToLower();
                    Name = _id;
                }
                else
                    throw new Exception("Invalid ID");
            }

            public static bool operator ==(BlockIdentifier a, BlockIdentifier b) => a.Equals(b);
            public static bool operator !=(BlockIdentifier a, BlockIdentifier b) => !a.Equals(b);
        }

        public interface IBlockDataFactory
        {
            IBlockData Parse(string raw);
            IBlockData Default { get; }
        }
        public interface IBlockData
        {
            string Stringify();
        }

        public class EmptyBlockDataFactory: IBlockDataFactory
        {
            public IBlockData Default => new EmptyBlockData();

            public IBlockData Parse(string raw)
            {
                return new EmptyBlockData();
            }
        }
        public class EmptyBlockData: IBlockData
        {
            public string Stringify()
            {
                return null;
            }
        }

        public class BlockType
        {
            public BlockIdentifier Identifier { get; internal set; }
            public BlockModel Model { get; set; }
            public Texture2D Texture { get; }
            public IBlockDataFactory DataFactory { get; }

            public BlockType(BlockIdentifier id,
                BlockModel model = null, Texture2D texture = null,
                IBlockDataFactory dataFactory = null)
            {
                Identifier = id;
                Model = model ?? new SolidBlockModel();
                Texture = texture;
                DataFactory = dataFactory ?? new EmptyBlockDataFactory();
            }
            public BlockType(string id,
                BlockModel model = null, Texture2D texture = null,
                IBlockDataFactory dataFactory = null)
            {
                Identifier = new BlockIdentifier(id);
                Model = model ?? new SolidBlockModel();
                Texture = texture;
                DataFactory = dataFactory ?? new EmptyBlockDataFactory();
            }
            public BlockType(string id, string displayName,
                BlockModel model = null, Texture2D texture = null,
                IBlockDataFactory dataFactory = null)
            {
                Identifier = new BlockIdentifier(id, displayName);
                Model = model ?? new SolidBlockModel();
                Texture = texture;
                DataFactory = dataFactory ?? new EmptyBlockDataFactory();
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

            public static bool operator ==(BlockType a, BlockType b)
            {
                if (a is null) {
                    return b is null;
                }
                else {
                    return a.Equals(b);
                }
            }
            public static bool operator !=(BlockType a, BlockType b) => !a.Equals(b);
        }
        public class Block
        {
            public BlockType Type { get; set; }
            public IBlockData Data { get; }

            public Block(BlockType type)
            {
                Type = type;
                Data = type.DataFactory.Default;
            }
            public Block(BlockType type, string data)
            {
                Type = type;
                Data = type.DataFactory.Parse(data);
            }
            public Block(BlockType type, IBlockData data)
            {
                Type = type;
                Data = data;
            }

            public Block(ShortBlock shortBlock)
            {
                Type = BlockTypes.Get(shortBlock.Id);
                Data = Type.DataFactory.Parse(shortBlock.Data);
            }
        }

        public struct ShortBlock
        {
            public int Id { get; set; }
            public string Data { get; set; }

            public ShortBlock(Block block)
            {
                Data = block.Data.Stringify();
                Id = block.Type.Identifier.Id;
            }
            public string Stringify()
            {
                var a = new ByteWriteStream();

                a.Write(Id);
                a.Write(Data ?? "");

                return a.FlushString();
            }
            public static ShortBlock Parse(string raw)
            {
                var stream = new ByteReadStream(raw);

                var id = stream.ReadInt();
                var data = stream.ReadString();

                if (data.Length == 0)
                    data = null;

                return new ShortBlock() {
                    Data = data,
                    Id = id,
                };
            }
        }

        public interface IWorld
        {
            Block this[int x, int y, int z] { get; set; }
        }
        public class World: IWorld
        {
            private List<Chunk> chunks = new List<Chunk>();

            private int getChunkHash(Chunk chunk)
            {
                return HashCode.Combine(
                    chunk.RelativeLocation.X,
                    chunk.RelativeLocation.Y,
                    chunk.RelativeLocation.Z
                );
            }

            public Block this[int x, int y, int z] {
                get {
                    var chunkX = (int)Math.Floor(x / 16f);
                    var chunkY = (int)Math.Floor(y / 16f);

                    var blockX = x - chunkX * 16;
                    var blockY = y - chunkY * 16;

                    var currChunk = chunks.FirstOrDefault(v =>
                        v.RelativeLocation.X == chunkX &&
                        v.RelativeLocation.Y == chunkY
                    );

                    if (currChunk == null)
                        return new Block(BlockTypes.Get("air"));

                    return null;
                }
                set => throw new NotImplementedException();
            }
        }

        public interface IChunkFactory<ChunkT> where ChunkT : IChunk
        {
            int Width { get; }
            int Height { get; }
            int Depth { get; }

            ChunkT Deminify(string raw);
        }
        public interface IChunk
        {
            VectorI3 RelativeLocation { get; set; }

            Block this[int x, int y, int z] { get; set; }
            Block this[VectorI3 location] { get; set; }

            string Minify();
        }

        public class ChunkFactory: IChunkFactory<Chunk>
        {
            public int Width => 16;
            public int Height => 255;
            public int Depth => 16;

            public Chunk Create()
            {
                return new Chunk();
            }

            public Chunk Deminify(string data)
            {
                var stream = new ByteReadStream(data);
                var chunk = new Chunk();

                for (var y = 0; y < 255; y++) {
                    for (var z = 0; z < 16; z++) {
                        for (var x = 0; x < 16; x++) {
                            var length = stream.ReadInt();
                            var id = stream.ReadInt();
                            var blockData = stream.ReadString(length - 4);

                            chunk.SetShortBlock(x, y, z, new ShortBlock() {
                                Id = id,
                                Data = blockData,
                            });
                        }
                    }
                }

                return chunk;
            }
        }
        public class Chunk: IChunk
        {
            private ShortBlock[,,] blocks = new ShortBlock[16, 255, 16];

            public VectorI3 RelativeLocation { get; set; } = new VectorI3(0, 0, 0);

            public Block this[VectorI3 location] {
                get => this[location.X, location.Y, location.Z];
                set => this[location.X, location.Y, location.Z] = value;
            }
            public Block this[int x, int y, int z] {
                get {
                    if (x < 0 || x >= 16)
                        throw new Exception("The x component must be between 0 and 15");
                    if (y < 0 || y >= 16)
                        throw new Exception("The y component must be between 0 and 15");
                    if (x < 0 || x >= 255)
                        throw new Exception("The z component must be between 0 and 255");

                    return new Block(blocks[x, y, z]);
                }
                set {
                    if (x < 0 || x >= 16)
                        throw new Exception("The x component must be between 0 and 15");
                    if (y < 0 || y >= 16)
                        throw new Exception("The y component must be between 0 and 15");
                    if (x < 0 || x >= 255)
                        throw new Exception("The z component must be between 0 and 255");

                    blocks[x, y, z] = new ShortBlock(value);
                }
            }

            public ShortBlock GetShortBlock(int x, int y, int z) => blocks[x, y, z];
            public void SetShortBlock(int x, int y, int z, ShortBlock value) => blocks[x, y, z] = value;

            public string Minify()
            {
                var raw = new ByteWriteStream();

                for (var y = 0; y < 255; y++) {
                    for (var z = 0; z < 16; z++) {
                        for (var x = 0; x < 16; x++) {
                            var data = blocks[x, y, z].Stringify();
                            raw.Write(data.Length);
                            raw.Write(data);
                        }
                    }
                }

                return raw.FlushString();
            }

            internal Chunk() { }
        }

        static string stringifyString(string val)
        {
            if (val == null)
                val = "";
            return BitConverter.ToString(val
                .ToCharArray()
                .Select(v => (byte)v)
                .ToArray());
        }

        public class GrassBlockDataFactory: IBlockDataFactory
        {
            public IBlockData Default => new GrassBlockData();

            public IBlockData Parse(string raw)
            {
                var snowy = new ByteReadStream(raw).ReadBool();

                return new GrassBlockData(snowy);
            }
        }

        public class GrassBlockData: IBlockData
        {
            public bool Snowy { get; set; } = false;

            public string Stringify()
            {
                return new ByteWriteStream().Write(Snowy).FlushString();
            }

            public GrassBlockData(bool snowy = false)
            {
                Snowy = snowy;
            }
        }

        static void Main(string[] args)
        {
            BlockTypes.AddBlock(new BlockType("air"));
            BlockTypes.MainNamespace = "test";


            Console.WriteLine(BlockTypes.Get("air").Identifier);
        }
    }
}
