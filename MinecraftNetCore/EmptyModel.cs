namespace MinecraftNetCore
{
    public class EmptyModel: BlockModel
    {
        public override FaceModel FrontFaceModel => null;

        public override FaceModel BackFaceModel => null;

        public override FaceModel TopFaceModel => null;

        public override FaceModel BottomFaceModel => null;

        public override FaceModel LeftFaceModel => null;

        public override FaceModel RightFaceModel => null;

        public override HitboxModel Hitbox => null;
    }
}
