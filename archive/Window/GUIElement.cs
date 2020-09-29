using System.Drawing;

namespace MinecraftNetWindow
{
    public abstract class GUIElement
    {
        private PointF position = new PointF(0, 0);
        public PointF Position {
            get => position;
            set {
                position = value;
                Update();
            }
        }

        protected SizeF size = new SizeF(1, 1);
        public SizeF Size {
            get => size;
            set {
                if (AutoSize == AutoSizeDirection.None)
                {
                    size = value;
                }
                else
                {
                    UpdateSize();
                }
                Update();
            }
        }

        public GUISprite[] Sprites { get; protected set; }

        private AutoSizeDirection autoSize = AutoSizeDirection.None;
        public AutoSizeDirection AutoSize {
            get => autoSize;
            set {
                autoSize = value;
                UpdateSize();
            }
        }

        public void Update()
        {
            UpdateSize();

            Render();
        }

        protected abstract void UpdateSize();

        protected abstract void Render();
    }
}
