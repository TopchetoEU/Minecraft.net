using System;
using System.Text.RegularExpressions;

namespace MinecraftNet
{
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
}
