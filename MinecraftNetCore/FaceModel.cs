namespace MinecraftNetCore
{
    public class FaceModel
    {
        public Model Model { get; set; }
        public Face Face { get; set; }
        public bool Cull { get; set; }

        public FaceModel(Model model, Face face, bool cull = true)
        {
            Model = model;
            Face = face;
            Cull = cull;
        }
    }
}
