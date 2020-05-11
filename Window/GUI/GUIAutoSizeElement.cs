using System.Drawing;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// Element, whose size can be auto adjusted
    /// </summary>
    public abstract class GUIAutoSizeElement : GUIElement
    {

        /// <summary>
        /// Werther or not the size is determined by the element or the user
        /// </summary>
        public bool AutoSize { get; set; }

        private Size size = Size.Empty;

        /// <inheritdoc/>
        public new Size Size {
            get => size;
            set {
                if (AutoSize) size = CalculateAutoSize();
                else size = value;
            }
        }

        /// <summary>
        /// Updates the dimensions of the element (if they're auto adjustable)
        /// </summary>
        public void UpdateSize()
        {
            if (AutoSize) size = CalculateAutoSize();
        }

        /// <summary>
        /// Calculates the size of the element
        /// </summary>
        /// <returns>The auto size of the element</returns>
        protected abstract Size CalculateAutoSize();
    }
}
