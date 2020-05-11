using System;
using MinecraftNetWindow.MainWindow;
using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// The bare bones element
    /// </summary>
    public abstract class GUIElement
    {
        public Mouse Mouse { get; } = new Mouse();
        public Keyboard Keyboard { get; } = new Keyboard();

        /// <summary>
        /// Element's textures in the atlas
        /// </summary>
        public GUIMaterial[] Textures { get; protected set; }

        /// <summary>
        /// Updates the element
        /// </summary>
        public void Update()
        {
            Render();
        }

        /// <summary>
        /// Renders the element
        /// </summary>
        protected abstract void Render();

        private Point2D position = Point2D.Zero;

        /// <summary>
        /// Fired when the position of the element changed
        /// </summary>
        public event EventHandler PositionChanged;
        /// <summary>
        /// The position of the element
        /// </summary>
        public Point2D Position {
            get => position;
            set {
                position = value;
                PositionChanged?.Invoke(this, new EventArgs());
            }
        }

        private Size2D size = new Size2D(100, 100);

        /// <summary>
        /// Size of the element
        /// </summary>
        public Size2D Size {
            get => size;
            set {
                size = value;
                SizeChanged?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// Fired when the size changes
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// The horizontal alignment of the origin
        /// </summary>
        public HorizontalAlignment HorizontalOriginAlignment { get; set; }
        /// <summary>
        /// The vertical alignment of the origin
        /// </summary>
        public VerticalAlignment VerticalOriginAlignment { get; set; }

        /// <summary>
        /// Calculates the origin of the element
        /// </summary>
        public Point2D Origin {
            get {
                var x = 0f;
                var y = 0f;

                switch (HorizontalOriginAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = 0;
                        break;
                    case HorizontalAlignment.Centered:
                        x = Size.Width / 2;
                        break;
                    case HorizontalAlignment.Right:
                        x = Size.Width;
                        break;
                }

                switch (VerticalOriginAlignment)
                {
                    case VerticalAlignment.Top:
                        y = 0;
                        break;
                    case VerticalAlignment.Centered:
                        y = Size.Height / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        y = size.Height;
                        break;
                    default:
                        break;
                }

                return new Point2D(x, y);
            }
        }
    }
}
