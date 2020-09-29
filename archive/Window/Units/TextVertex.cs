using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class TextVertex: IVertex
    {
        public const string fontLayout =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
            "0123456789,.?!`~@#$%^&*()_-+=\\|<>/{}[]" +
            "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЮЯ\"'§;: ";
        //"абвгдежзийклмнопрстуфхцчшщъьюя";

        public ShaderArgumentMap[] ShaderArguments { get; } =
        {
            new ShaderArgumentMap("inPosition", 2),
            new ShaderArgumentMap("inCharacter", 1),
        };

        public float X { get; }
        public float Y { get; }

        public float Character { get; }

        public float[] ToFloatAray()
        {
            return new[] { X, Y, Character };
        }

        public TextVertex(float x, float y, char character, Font font)
        {
            X = x;
            Y = y;
            Character = font.GetCharacterIndex(character);
        }
    }

    public class Font: IDisposable
    {
        public Texture Texture { get; }

        private Bitmap bitmap { get; }

        public string Name { get; }
        public Size CharacterSize { get; }
        public int HorisontalCharCount => Texture.Size.Width / CharacterSize.Width;
        public int VerticalCharCount => Texture.Size.Height / CharacterSize.Height;

        public string CharacterLayout { get; }
        public int InvalidCharacterIndex { get; }

        public int GetCharacterIndex(char character)
        {
            var index = CharacterLayout.IndexOf(character);

            if (index < 0) return InvalidCharacterIndex;
            else           return index;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Texture.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        public const string DefaultCharLayout = 
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789" +
            ",.?!`~@#$%^&*()_-+=\\|<>/{}[]\"'§;: ";

        public Font(string path, char invalidChar = '?', Size? characterSize = null, string charLayout = DefaultCharLayout)
        {
            Texture = new Texture(path);
            bitmap = new Bitmap(path);
            var fileInfo = new FileInfo(path);

            Name = fileInfo.Name.Replace(fileInfo.Extension, "");
            CharacterSize = characterSize ?? new Size(16, 24);

            CharacterLayout = charLayout;
            var invalidCharIndex = charLayout.IndexOf(invalidChar);
            if (invalidCharIndex < 0) throw new ArgumentException("invalidChar", "The invalid character specified wasn't in the character layout.");
            InvalidCharacterIndex = invalidCharIndex;
        }

        public int GetCharacterWidth(char character)
        {
            var charIndex = GetCharacterIndex(character);

            var charX = charIndex % (Texture.Size.Width / CharacterSize.Width);
            var charY = charIndex / (Texture.Size.Width / CharacterSize.Width);

            var x = charX * CharacterSize.Width;
            var y = (charY + 1) * CharacterSize.Height - 9;

            for (int i = 0; i < CharacterSize.Width; i++)
            {
                var color = bitmap.GetPixel(x + i, y);

                if (color.R != color.G || color.G != color.B)
                {
                    return i + 1;
                }
            }

            return 0;
        }
    }
}
