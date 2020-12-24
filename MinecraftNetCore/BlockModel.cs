namespace MinecraftNet
{
    public abstract class BlockModel
    {
        public abstract FaceModel FrontFaceModel { get; }
        public abstract FaceModel BackFaceModel { get; }

        public abstract FaceModel TopFaceModel { get; }
        public abstract FaceModel BottomFaceModel { get; }

        public abstract FaceModel LeftFaceModel { get; }
        public abstract FaceModel RightFaceModel { get; }

        public abstract HitboxModel Hitbox { get; }

        public Model GetBlockModel(bool top, bool bottom, bool left, bool right, bool front, bool back)
        {
            var newModel = new Model();

            if (TopFaceModel != null && (!TopFaceModel.Cull || !top))
                newModel.Concatenate(TopFaceModel.Model);
            if (BottomFaceModel != null && (!BottomFaceModel.Cull || !bottom))
                newModel.Concatenate(BottomFaceModel.Model);

            if (LeftFaceModel != null && (!LeftFaceModel.Cull || !left))
                newModel.Concatenate(LeftFaceModel.Model);
            if (RightFaceModel != null && (!RightFaceModel.Cull || !right))
                newModel.Concatenate(RightFaceModel.Model);

            if (FrontFaceModel != null && (!FrontFaceModel.Cull || !front))
                newModel.Concatenate(FrontFaceModel.Model);
            if (BackFaceModel != null && (!BackFaceModel.Cull || !back))
                newModel.Concatenate(BackFaceModel.Model);

            return newModel;
        }
    }
}
