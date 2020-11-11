namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// The type of clearing of screen
    /// </summary>
    public enum ClearType
    {
        /// <summary>
        /// Clears the color information from the screen, effectively resetting it
        /// </summary>
        ColorBuffer = 0x4000,
        /// <summary>
        /// Clears any depth buffer sotred data (it is required for depth testing to work propertly)
        /// </summary>
        DepthBuffer = 0x100,
    }
}
