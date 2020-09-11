using MinecraftNetWindow.Units;
using OpenTK;
using System;

namespace MinecraftNetWindow.Geometry
{
    public class PrespectiveCamera: ICamera
    {
        private float fov = 1;
        private float nearclip = .1f;
        private float farclip = 10f;
        private float ratio = 1;

        public float FOV {
            get => fov * 180 / (float)Math.PI;
            set => fov = value * (float) Math.PI / 180; 
        }
        public float Nearclip {
            get => nearclip;
            set {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", "Nearclip must be bigger than 0");
                else nearclip = value;
            }
        }
        public float Farclip {
            get => farclip;
            set {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", "Farclip must be bigger than 0");
                else farclip = value;
            }
        }
        public float Ratio {
            get => ratio;
            set {
                if (ratio <= 0) throw new ArgumentException("value", "Ratio can't be less or equal to zero");
                else
                {
                    ratio = value;
                }
            }
        }

        public CameraUniformLayouts UniformLayouts => 
            new CameraUniformLayouts("viewMatrix", "cameraMatrix");

        public Transformation Transformation { get; set; } = 
            new Transformation(
                new PointF3D(0, 0, 0), 
                new SizeF3D(1, 1, 1), 
                new RotationF3D(0, 0, 0)
            );

        public Matrix4 GetCameraMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, Ratio, Nearclip, Farclip);
        }
        public Matrix4 GetViewMatrix()
        {
            return Transformation.GetMatrix();
        }

        public PrespectiveCamera(float fov, float nearClip, float farClip, float ratio)
        {
            FOV = fov;
            Nearclip = nearClip;
            Farclip = farClip;
            Ratio = ratio;
        }
    }
}