using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MinecraftNetCore
{
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
            BlockTypes.AddBlock(new BlockType("air", new EmptyModel()));
        }
    }
}
