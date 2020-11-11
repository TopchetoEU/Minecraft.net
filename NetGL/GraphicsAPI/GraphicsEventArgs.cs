namespace NetGL.GraphicsAPI
{
    public delegate void GraphicsEventHandler(object sender, GraphicsEventArgs e);
    public delegate void CancellableGraphicsEventHandler(object sender, CancellableGraphicsEventArgs e);
    /// <summary>
    /// Arguments for any events, including graphics manipulation
    /// </summary>
    public class GraphicsEventArgs
    {
        /// <summary>
        /// The graphics, allocated for drawing in the current event,
        /// they might break everything, if used outside the event
        /// </summary>
        public Graphics Graphics { get; }

        /// <summary>
        /// Creates new graphics event
        /// </summary>
        /// <param name="grph">The graphics to use</param>
        public GraphicsEventArgs(Graphics grph)
        {
            Graphics = grph;
        }
    }
    public class CancellableGraphicsEventArgs: CancelEventArgs
    {
        public Graphics Graphics { get; }

        public CancellableGraphicsEventArgs(Graphics graphics, bool cancelled = false): base(cancelled)
        {
            Graphics = graphics;
        }
    }
}
