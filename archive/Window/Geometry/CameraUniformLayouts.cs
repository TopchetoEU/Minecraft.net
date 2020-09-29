namespace MinecraftNetWindow.Geometry
{
    public class CameraUniformLayouts
    {
        public string ViewMatrixName { get; }
        public string CameraMatrixName { get; }

        public CameraUniformLayouts(string viewName, string cameraName)
        {
            ViewMatrixName = viewName;
            CameraMatrixName = cameraName;
        }
    }
}