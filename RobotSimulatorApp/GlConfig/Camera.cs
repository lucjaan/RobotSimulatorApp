using Microsoft.VisualBasic.Logging;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.WinForms;
using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace RobotSimulatorApp.GlConfig
{
    public class Camera
    {
        public float AspectRatio;
        public float Sensitivity = 0.01f;
        public Vector3 Position;
        private Vector3 Target;
        private Vector3 startPos;

        public Vector3 Front = -Vector3.UnitZ;
        private Vector3 Right = Vector3.UnitX;
        private Vector3 Up = Vector3.UnitY;


        private float Pitch, Yaw;
        public Matrix4 View;
        private bool firstMove = true;
        private Vector2 lastPosition;

        string log = string.Empty;

        public Camera(Vector3 position, float aspectRatio)
        {
            AspectRatio = aspectRatio;
            Position = position;
            //Front = new (0f, 0f, -1f);
            View = Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void SetView(Vector3 position, Vector3 front)
        {
            Position = position;
            Front = front;
            UpdateVectors();
        }

        public void Move(INativeInput input)
        {

            //INativeInput input = glControl.EnableNativeInput();
            if (input != null)
            {
                LookAround(input);
                Zoom(input);
                Rotate(input);
                Home(input);
                //Debug.WriteLine("aaaaaaaaa");
            }

            //Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            //Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            //Debug.WriteLine($"CAM: P:{Position}, F:{Front}");
            View = Matrix4.LookAt(Position, Position + Front, Up);
            //Debug.WriteLine($"Pos {Position} {Front}");
        }

        public void UpdateVectors()
        {

            //Debug.WriteLine($"pre-normal: {Front}");
            Front = Vector3.Normalize(Front);
            //Debug.WriteLine($"normalized: {Front}");

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            View = Matrix4.LookAt(Position, Position + Front, Up);
        }

        private void LookAround(INativeInput input)
        {
            input.MouseMove += (e) =>
            {
                Yaw += e.DeltaX * Sensitivity;
                Pitch -= MathHelper.Clamp(e.DeltaY * Sensitivity, -89f, 89f); // Reversed since y-coordinates range from bottom to top
                Debug.WriteLine($"e{e.Delta}");

                Front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
                //for some bizzare reason it always equals 1?? 
                //Front.X = 0; //HERE
                Front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
                Front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
                //Debug.WriteLine($"pre-normal: {Front}");
                Front = Vector3.Normalize(Front);
                //Debug.WriteLine($"normalized: {Front}");
            };
        }

        private void Zoom(INativeInput input)
        {
            Position.Z = MathHelper.Clamp(Position.Z, 0.1f, 100f);

            input.MouseWheel += (e) =>
                Position.Z -= e.OffsetY * 0.01f;
        }

        private void Rotate(INativeInput input)
        {
//TODO
        }

        private void Home(INativeInput input)
        {
            input.KeyUp += (e) =>
            {
                if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Home)
                {
                    Front = new(0f, 0f, -1f);
                    Position = startPos;
                    Position.Z = 15f;
                    Debug.WriteLine(Position.ToString());
                }
            };

            //FileStream fs = new(@"C:\Users\Luk\Desktop\deltaLog.txt", FileMode.OpenOrCreate);
            //using (StreamWriter sr = new StreamWriter(fs))
            //{
            //    sr.WriteLine(log);
            //    sr.Close();
            //}
        }
    }
}
