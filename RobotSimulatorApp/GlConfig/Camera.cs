using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;

namespace RobotSimulatorApp.GlConfig
{
    public class Camera
    {
        public float AspectRatio;
        private Vector3 Position;
        private Vector3 Target;
        private Vector3 Front;

        private Vector3 View;
        private Vector3 Forward;

        public Camera(float aspectRatio)
        {
            AspectRatio = aspectRatio;

        }
       
        public void Move(INativeInput input)
        {

        }

        public void LookAround()
        {

        }

        public void Zoom() 
        {

        }

        public void Rotate()
        {

        }
    }
}
