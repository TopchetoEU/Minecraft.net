using NetGL;

namespace MinecraftNetCore
{
    public class SolidBlockModel: BlockModel
    {
        public override FaceModel FrontFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 0, 0),
                        new Vertice(0, 1, 0),
                        new Vertice(1, 1, 0),
                        new Vertice(1, 0, 0),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Front
        );
        public override FaceModel BackFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(1, 1, 1),
                        new Vertice(1, 0, 1),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Back
        );

        public override FaceModel LeftFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(0, 1, 0),
                        new Vertice(0, 0, 0),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Right
        );
        public override FaceModel RightFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 0, 1),
                        new Vertice(0, 1, 1),
                        new Vertice(0, 1, 0),
                        new Vertice(0, 0, 0),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Right
        );

        public override FaceModel TopFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 1, 0),
                        new Vertice(0, 1, 1),
                        new Vertice(1, 1, 1),
                        new Vertice(1, 1, 0),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Top
        );
        public override FaceModel BottomFaceModel { get; } = new FaceModel(
                new Model(new Vertice[] {
                        new Vertice(0, 0, 0),
                        new Vertice(0, 0, 1),
                        new Vertice(1, 0, 1),
                        new Vertice(1, 0, 0),
                },
                new uint[] { 0, 1, 2, 0, 3, 2 }),
            Face.Bottom
        );

        public override HitboxModel Hitbox { get; } = new HitboxModel(
            new Vector3[] {
                    new Vector3(0, 0, 0), //0
                    new Vector3(0, 1, 0), //1
                    new Vector3(1, 1, 0), //2
                    new Vector3(1, 0, 0), //3

                    new Vector3(0, 0, 1), //4
                    new Vector3(0, 1, 1), //5
                    new Vector3(1, 1, 1), //6
                    new Vector3(1, 0, 1), //7
            },
            new uint[] {
                    0, 1, 2, 0, 3, 2, // front
                    4, 5, 6, 4, 7, 6, // back

                    0, 1, 5, 0, 4, 5, // left
                    3, 7, 6, 3, 2, 6, // right

                    1, 2, 6, 1, 7, 6, // top
                    0, 3, 7, 0, 4, 7, // back
            }
        );
    }
}
