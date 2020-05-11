using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// Regular container
    /// </summary>
    public class GUIRegularContainer : GUIContainer
    {
        /// <summary>
        /// The horizontal alignment inside the container
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }
        /// <summary>
        /// The vertical alignment inside the container
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <inheritdoc/>
        public override Point2D CalculateChildPosition(int childIndex)
        {
            var x = 0;
            var y = 0;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    x = 0;
                    break;
                case HorizontalAlignment.Centered:
                    x = (int)Size.Width / 2;
                    break;
                case HorizontalAlignment.Right:
                    x = (int)Size.Width;
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    y = 0;
                    break;
                case VerticalAlignment.Centered:
                    y = (int)Size.Width / 2;
                    break;
                case VerticalAlignment.Bottom:
                    y = (int)Size.Width;
                    break;
                default:
                    break;
            }

            return Children[childIndex].Position - Children[childIndex].Origin + new Point2D(x, y) + Position;
        }
    }
}
