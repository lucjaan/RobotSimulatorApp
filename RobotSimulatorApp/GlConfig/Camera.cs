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
        public float Sensitivity =1f;
        private Vector3 Position;
        private Vector3 Target;
        private Vector3 startPos;

        private Vector3 Front = -Vector3.UnitZ;
        private Vector3 Right =Vector3.UnitX;
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
            View = Matrix4.LookAt(Position, Position + Front, Vector3.UnitY); 
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

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            //Debug.WriteLine($"CAM: P:{Position}, F:{Front}");
            View = Matrix4.LookAt(Position, Position + Front, Vector3.UnitY);
            //Debug.WriteLine($"Pos {Position} {Front}");
        }

        private void LookAround(INativeInput input)
        {
            float x, y;

            if (firstMove)
            {
                lastPosition = (Front.X, Front.Y);
            }
            x = lastPosition.X;
            y = lastPosition.Y;

            KeyboardState keyboardState = input.KeyboardState;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                x++;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                y++;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                y--;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                x--;
            }

            Yaw += x * Sensitivity;
            Pitch -= y * Sensitivity;

            lastPosition.X = x; 
            lastPosition.Y = y;

            Front.X = MathF.Cos(Pitch) * MathF.Cos(Yaw);
            Front.Y = MathF.Sin(Pitch);
            Front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);
            Debug.WriteLine($"Pitch: {Pitch}, Yaw: {Yaw}, Front: {Front}");

            Front = Vector3.Normalize(Front);

            //var lastEvent = new Vector2(0,0);
            //int i = 0;
            //input.MouseMove += (e) =>
            //{
            //    //Debug.WriteLine($"{e.DeltaX}, {e.DeltaY}");
            //    // Apply the camera pitch and yaw (we clamp the pitch in the camera class)

            //    //float myDeltaX = e.X - lastEvent.X;
            //    //float myDeltaY = e.Y - lastEvent.Y;
            //    //log += $"myDelta = ({myDeltaX}, {myDeltaY})     e.Delta = ({e.DeltaX}, {e.DeltaY}) \n";


            //    //lastEvent = new (e.X, e.Y);
            //    //i++;
            //    var mouse = e.Position;

            //    if (mouse != null)
            //    {
            //        if (firstMove)
            //        {
            //            lastPosition = new Vector2(mouse.X, mouse.Y);
            //            firstMove = false;
            //        }   
            //        else

            //        {
            //            //{
            //            //    var deltaX = mouse.X - _lastPos.X;
            //            //    var deltaY = mouse.Y - _lastPos.Y;
            //            //    _lastPos = new Vector2(mouse.X, mouse.Y);

            //            //    _camera.Yaw += deltaX * sensitivity;
            //            //    _camera.Pitch -= deltaY * sensitivity;
            //            //}

            //            var deltaX = mouse.X - lastPosition.X;
            //            var deltaY = mouse.Y - lastPosition.Y;
            //            lastPosition = new Vector2(mouse.X, mouse.Y);
            //            Debug.WriteLine($"Mouse: ({mouse.X}, {mouse.Y}), Delta: ({deltaX}, {deltaY})");

            //            Yaw += deltaX * Sensitivity;
            //            Pitch -= deltaY * Sensitivity;
            //        }
            //    }
            //        //_lastPos = new Vector2(mouse.X, mouse.Y);




            //    //Yaw += e.DeltaX * Sensitivity;
            //    //Pitch -= e.DeltaY * Sensitivity; // Reversed since y-coordinates range from bottom to top

            //    //Yaw += e.DeltaX;
            //    //Pitch -= e.DeltaY; // Reversed since y-coordinates range from bottom to top

            //    Pitch = MathHelper.Clamp(Pitch, -89f, 89f);
            //    //Yaw = MathHelper.Clamp(Yaw, -89f, 89f);

            //    Yaw = MathHelper.DegreesToRadians(Yaw);
            //    Pitch = MathHelper.DegreesToRadians(Pitch);

            //    //Yaw = (float)Math.Round(Yaw, 15);
            //    //Pitch = (float)Math.Round(Pitch, 15);
            //    //Yaw *= Sensitivity;
            //    //Pitch *= Sensitivity;

            //    //Front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
            //    ////for some bizzare reason it always equals 1?? 
            //    ////Front.X = 0; //HERE


            //    //Front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            //    //Front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));

            //    Front.X = MathF.Cos(Pitch) * MathF.Cos(Yaw);
            //    Front.Y = MathF.Sin(Pitch);
            //    Front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);
            //    //Front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);
            //    //.WriteLine(Front);
            //    //Debug.WriteLine($"Pitch: {Pitch}, Yaw: {Yaw}, Front: {Front}");

            //    Front = Vector3.Normalize(Front);
            //};

        }

        private void Zoom(INativeInput input) 
        {
            //Position.Z = Position.Z > 0 ? Position.Z : 0;
            //Position.Z = Position.Z < 40 ? Position.Z : 40;

            Position.Z = MathHelper.Clamp(Position.Z, 0.1f, 100f);

            input.MouseWheel += (e) =>
                Position.Z -= e.OffsetY * 0.01f;
        }

        private void Rotate(INativeInput input)
        {
           //Debug.WriteLine("")

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

            FileStream fs = new(@"C:\Users\Luk\Desktop\deltaLog.txt", FileMode.OpenOrCreate);
            using (StreamWriter sr = new StreamWriter(fs))
            {
                sr.WriteLine(log);
                sr.Close();
            }
        }
    }
}
