

using RobotSimulatorApp.GlConfig;
using static RobotSimulatorApp.Robot.SCARA.RobotLimb2;
using System.Xml.Linq;
using OpenTK.Mathematics;
using System;
using OpenTK.WinForms;
using RobotSimulatorApp.OpenGL;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RobotLimb3
    {
        public Geometry Geometry { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 RotationCenter { get; set; }
        public Vector3 Center { get; set; }
        public float Distance { get; set; }
        public float MaximumDistance { get; set; }
        private GLControl gl;
        private Cube? Cube;
        private Cylinder? cylinder { get; set; }

        public RobotLimb3(GLControl glControl, string name, Geometry shape, Vector3 position, float maxMovement)
        {
            gl = glControl;
            Name = name;
            Geometry = shape;
            Position = position;
            //Geometry;
            MaximumDistance = maxMovement;
            //switch (Geometry)
            //{
            //    case Geometry.Cube
            //}
            //RotationCenter = rotationCenter;

        }

        public void CreateCube(float sizeX, float sizeY, float sizeZ)
        {
            if (Geometry != Geometry.Cube)
            {
                throw new InvalidOperationException($"Tried to call function for Cube while current geometry is {Geometry}");
            }

            Cube = new(gl, Position, sizeX, sizeY, sizeZ);
            Center = Cube.Center;
        }

        public void CreateCylinder(float radius, float height)
        {
            if (Geometry != Geometry.Cube)
            {
                throw new InvalidOperationException($"Tried to call function for Cylinder while current geometry is {Geometry}");
            }

            Cylinder c = new(gl, Position, radius, height);
            Center = c.Center;
        }

        public void Move(float angle, Vector3 centerOfRotation)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    //angle = MathHelper.DegreesToRadians(angle);
                    Cube.Transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(angle), centerOfRotation);
                    break;
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;

            }
        }

        public void SetRotationCenter(Vector3 rotationCenter)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.SetPoint(rotationCenter);
                    break; 
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;
            }
            RotationCenter = rotationCenter;
        }

        public Vector3 GetRotationCenter()
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    return Helpers.GetPositionFromMatrix(Cube.Point);
                    break;

                case Geometry.Cylinder:
                case Geometry.Cone:
                    return Vector3.Zero;
                    break;

                default:
                    return Vector3.Zero;
                    break;              
            }
        }

        public void UpdateModel()
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.UpdateBaseModel();
                    break;
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;
            }
        }

        public void RenderModel(Matrix4 view, Matrix4 projection)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.RenderCube(view, projection);
                    break;
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;
            }
        }

        public void SetColor(Color4 color)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.SetColor(color);
                    break;
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;

            }
        }
    }
}
