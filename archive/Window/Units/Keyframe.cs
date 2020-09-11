using System.Collections.Generic;
using System.Linq;

namespace MinecraftNetWindow.Units
{
    public class Keyframe
    {
        public IInterpolateable OriginalInterpolateable { get; set; }

        private Dictionary<string, float> Properties = new Dictionary<string, float>();

        public void Add(string name, float value)
        {
            Properties.Add(name, value);
        }
        public void Remove(string name)
        {
            Properties.Remove(name);
        }
        public bool HasProperty(string name)
        {
            return Properties.ContainsKey(name);
        }
        public bool HasProperties(params string[] names)
        {
            foreach (var name in names)
            {
                if(!Properties.ContainsKey(name)) return false;
            }

            return true;
        }

        public string[] GetPropertyNames()
        {
            return Properties.Keys.ToArray();
        }

        public float this[string name] {
            get {
                return Properties[name];
            }
            set {
                if (Properties.ContainsKey(name))
                    Properties[name] = value;
                else
                    Properties.Add(name, value);
            }
        }

        public Keyframe(IInterpolateable interpolateable)
        {
            OriginalInterpolateable = interpolateable;

            var keyframe = interpolateable.ToKeyframe();
            Properties = keyframe.Properties;
        }
        public Keyframe()
        {

        }
    }
}
