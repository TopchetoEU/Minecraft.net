using OpenTK;
using MinecraftNetWindow.Geometry;
using System;

namespace MinecraftNetWindow.Units
{
    public class RotationF3D
    {
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }

        public Matrix4 GetMatrix()
        {
            return
                   Matrix4.CreateRotationY(Yaw * (float)Math.PI / 180) *
                   Matrix4.CreateRotationX(Pitch * (float)Math.PI / 180) *
                   Matrix4.CreateRotationZ(Roll * (float)Math.PI / 180);
        }

        public RotationF3D(float pitch, float yaw, float roll)
        {
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }
    }
}