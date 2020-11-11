namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// An object that can be drawn on screen
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the object on screen
        /// </summary>
        /// <param name="graphics">The graphics being used to draw the object</param>
        void Draw(Graphics graphics);
    }
}
