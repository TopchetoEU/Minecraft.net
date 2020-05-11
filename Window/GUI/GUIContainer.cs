using System.Collections.Generic;
using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// A element, able to contain a element
    /// </summary>
    public abstract class GUIContainer : GUIElement
    {
        /// <summary>
        /// All the children of the container
        /// </summary>
        public List<GUIElement> Children { get; }

        /// <summary>
        /// Calculates a given child's position, relative to the container parent coordinate system
        /// </summary>
        /// <param name="childIndex">Child index</param>
        /// <returns>The position of the child</returns>
        public abstract Point2D CalculateChildPosition(int childIndex);

        /// <summary>
        /// Renders the container (does nothing, in order to save memory)
        /// </summary>
        protected override void Render()
        {
        }
    }
}
