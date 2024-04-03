using OpenTK.Mathematics;
using System;

namespace RobotSimulatorApp.GlConfig
{
    public class Camera
    {
        public float AspectRatio;
        public Vector3 Position;
        public Vector3 Front = -Vector3.UnitZ;
        private Vector3 Right = Vector3.UnitX;
        private Vector3 Up = Vector3.UnitY;
        public float Pitch, Yaw;
        public Matrix4 View;

        public Camera(Vector3 position, float pitch, float yaw, float aspectRatio)
        {
            AspectRatio = aspectRatio;
            Position = position;
            Pitch = pitch;
            Yaw = yaw;
            UpdateVectors();
        }

        public void UpdateVectors()
        {
            Front.X = MathF.Cos(Pitch) * MathF.Cos(Yaw);
            Front.Y = MathF.Sin(Pitch);
            Front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);

            Front = Vector3.Normalize(Front);
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            View = Matrix4.LookAt(Position, Position + Front, Up);
        }
    }
}
