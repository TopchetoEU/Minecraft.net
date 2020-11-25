using NetGL.WindowAPI;
using System;
using System.Collections.Generic;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// A transformation in 3D space
    /// </summary>
    public struct Transform
    {
        /// <summary>
        /// The all-zero transformation, except scale, whitch is all-one
        /// </summary>
        public static Transform Zero { get; } = new Transform(Vector3.Zero, Vector3.Zero, new Vector3(1));

        /// <summary>
        /// Creates new transformation
        /// </summary>
        /// <param name="pos">The translation</param>
        /// <param name="rot">The rotation</param>
        /// <param name="scale">the scale</param>
        public Transform(Vector3 pos, Vector3 rot, Vector3 scale)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
            TransformationCenter = new Vector3(0, 0, 0);
        }
        /// <summary>
        /// Creates new transformation with a transformation center
        /// </summary>
        /// <param name="pos">The translation</param>
        /// <param name="rot">The rotation</param>
        /// <param name="scale">the scale</param>
        /// <param name="center">The transformation center</param>
        public Transform(Vector3 pos, Vector3 rot, Vector3 scale, Vector3 center)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
            TransformationCenter = center;
        }

        /// <summary>
        /// the tranformation center
        /// </summary>
        public Vector3 TransformationCenter { get; set; }

        /// <summary>
        /// The position
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// The rotation
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// The scale
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// The matrix, that expresses the same translation in the following order:
        /// transformationCenter, scale, rotationRow, tranformationYaw, transformationPitch,
        /// -transformationCenter, position
        /// </summary>
        public Matrix4 Matrix {
            get {
                var mat =
                    Matrix4.CreateTranslation(Position) *
                    Matrix4.CreateTranslation(TransformationCenter) *
                    Matrix4.CreateScale(Scale) *
                    Matrix4.CreateRotationZ(Rotation.Z) *
                    Matrix4.CreateRotationY(Rotation.Y) *
                    Matrix4.CreateRotationX(Rotation.X) *
                    Matrix4.CreateTranslation(-TransformationCenter);

                return mat;
            }
        }
    }

    /// <summary>
    /// An object in 3D space
    /// </summary>
    public interface IObject3D
    {
        /// <summary>
        /// The transformation ofthe object
        /// </summary>
        Transform Transformation { get; set; }
        /// <summary>
        /// The transformation's matrix
        /// </summary>
        Matrix4 TransformMatrix { get; }
        /// <summary>
        /// The matrix uniform name
        /// </summary>
        string TransformationMatrixName { get; }

        public Vector3 Position {
            get => Transformation.Position;
            set => Transformation = new Transform(
                value,
                Transformation.Rotation,
                Transformation.Scale,
                Transformation.TransformationCenter);
        }
        public Vector3 Rotation {
            get => Transformation.Rotation;
            set => Transformation = new Transform(
                Transformation.Position,
                value,
                Transformation.Scale,
                Transformation.TransformationCenter);
        }
        public Vector3 Scale {
            get => Transformation.Scale;
            set => Transformation = new Transform(
                Transformation.Position,
                Transformation.Rotation,
                value,
                Transformation.TransformationCenter);
        }
        public Vector3 TransformCenter {
            get => Transformation.TransformationCenter;
            set => Transformation = new Transform(
                Transformation.Position,
                Transformation.Rotation,
                Transformation.Scale,
                value);
        }
    }
    /// <summary>
    /// A camera, that gives camera matrix
    /// </summary>
    public interface ICamera: IObject3D
    {
        /// <summary>
        /// The camera's camera matrix
        /// </summary>
        Matrix4 GetCameraMatrix(float ratio);
        /// <summary>
        /// The name of the camera matrix uniform
        /// </summary>
        string CameraMatrixName { get; }
    }

    /// <summary>
    /// A prespective camera, that has the property of objects getting smaller, as they get further from the camera
    /// </summary>
    public class PrespectiveCamera: ICamera
    {
        /// <summary>
        /// The ratio between width and height
        /// </summary>
        public float FOV { get; set; } = 70;
        /// <summary>
        /// The near clipping plane
        /// </summary>
        public float Nearclip { get; set; } = 0.1f;
        /// <summary>
        /// The far clipping plane
        /// </summary>
        public float Farclip { get; set; } = 100f;

        /// <summary>
        /// The transformation of the camera
        /// </summary>
        public Transform Transformation { get; set; } = Transform.Zero;

        public Matrix4 GetCameraMatrix(float ratio)
        {
            return Matrix4.CreateProjection(70, ratio, Nearclip, Farclip);
        }
        public Matrix4 TransformMatrix =>
            Transformation.Matrix;

        public string CameraMatrixName { get; } = "cameraMatrix";
        public string TransformationMatrixName { get; set; } = "cameraTransformMatrix";

        /// <summary>
        /// Creates new prespective camera (the closer an object is, the bigger it appears)
        /// </summary>
        /// <param name="fov">filed of view</param>
        /// <param name="near">Near clipping plane</param>
        /// <param name="far">Far clipping plane</param>
        public PrespectiveCamera(float fov, float near, float far)
        {
            FOV = fov;
            Nearclip = near;
            Farclip = far;
        }
    }

    /// <summary>
    /// All the types of supported comparative functions
    /// </summary>
    public enum ComparativeFunc
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LessOrEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GreaterOrEqual = 0x0206,
        Always = 0x0207,
    }
    /// <summary>
    /// All the typeos of supported blending functions
    /// </summary>
    public enum BlendingFunc
    {
        Zero = 0,
        One = 1,
        SourceColor = 0x300,
        OneMinusSourceColor = 0x301,
        DestinationColor = 0x306,
        OneMinusDestinationColor = 0x307,
        SourceAlpha = 0x302,
        OneMinusSourceAlpha = 0x303,
        DestinationAlpha = 0x304,
        OneMinusDestinationAlpha = 0x305,
        ConstantColor = 0x8001,
        OneMinusConstantColor = 0x8002,
        ConstantAlpha = 0x8003,
        OneMinusConstantAlpha = 0x8004,
        SourceAlphaStaurate = 0x308,
        Source1Color = 0x88F9,
        OneMinusSource1Color = 0x88FA,
        Source1Alpha = 0x8589,
        OneMinusSource1Alpha = 0x88FB
    }

    /// <summary>
    /// Scene, containing a camera and different objects
    /// </summary>
    public class Scene: IDrawable, IObject3D
    {
        /// <summary>
        /// All the meshes in the scene
        /// </summary>
        public List<Mesh> Meshes { get; } = new List<Mesh>();
        /// <summary>
        /// The scene's camera
        /// </summary>
        public ICamera Camera { get; set; }

        public Transform Transformation { get; set; } = Transform.Zero;
        public string TransformationMatrixName { get; set; } = "sceneMatrix";
        public Matrix4 TransformMatrix => Transformation.Matrix;

        public event CancellableGraphicsEventHandler Drawing;
        public event GraphicsEventHandler Drawn;

        public float ViewRation { get; set; } = 1;


        /// <summary>
        /// Draws the scene on screen
        /// </summary>
        public void Draw(Graphics graphics)
        {
            var e =  new CancellableGraphicsEventArgs(graphics, false);
            Drawing?.Invoke(this, e);
            if (!e.Cancelled) {
                foreach (var mesh in Meshes) {
                    mesh.Program.ApplyUniform(Camera.TransformMatrix, Camera.TransformationMatrixName);
                    mesh.Program.ApplyUniform(TransformMatrix, TransformationMatrixName);
                    mesh.Program.ApplyUniform(mesh.TransformMatrix, mesh.TransformationMatrixName);
                    mesh.Program.ApplyUniform(Camera.GetCameraMatrix(ViewRation), Camera.CameraMatrixName);

                    mesh.Draw(graphics);
                }
                Drawn?.Invoke(this, new GraphicsEventArgs(graphics));
            }
        }
    }

    public interface IMovementController
    {
        Vector3 Position { get; }

        void Update(float delta);
    }
    public interface ICameraController
    {
        Vector3 Rotation { get; }

        void Update();
    }
}
