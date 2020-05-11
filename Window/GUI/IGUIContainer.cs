using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window;
using Window.MainWindow;

namespace Window.GUI
{
    /// <summary>
    /// The bare b bones element
    /// </summary>
    public abstract class GUIElement
    {
        private Position position = Position.Zero;

        /// <summary>
        /// Fired when the position of the element changed
        /// </summary>
        public event EventHandler PositionChanged;
        /// <summary>
        /// The position of the element
        /// </summary>
        public Position Position {
            get => position;
            set {
                position = value;
                PositionChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public interface IGUIContainer
    {

    }
}
