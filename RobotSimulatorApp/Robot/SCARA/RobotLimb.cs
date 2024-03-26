

using RobotSimulatorApp.GlConfig;
using System.Xml.Linq;
using OpenTK.Mathematics;
using System;
using OpenTK.WinForms;
using RobotSimulatorApp.OpenGL;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RobotLimb
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
        private Cone? Cone;
        private Cylinder? Cylinder;

        public RobotLimb(GLControl glControl, string name, Geometry shape, Vector3 position, float maxMovement)
        {
            gl = glControl;
            Name = name;
            Geometry = shape;
            Position = position;
            MaximumDistance = maxMovement;
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
            if (Geometry != Geometry.Cylinder)
            {
                throw new InvalidOperationException($"Tried to call function for Cylinder while current geometry is {Geometry}");
            }

            Cylinder = new(gl, Position, radius, height);
            Center = Cylinder.Center;
        }

        public void CreateCone(float radius, float height)
        {
            if (Geometry != Geometry.Cone)
            {
                throw new InvalidOperationException($"Tried to call function for Cone while current geometry is {Geometry}");
            }

            Cone = new(gl, Position, radius, height);
            Center = Cone.Center;
        }

        public void MoveRevolute(float angle, Vector3 centerOfRotation)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.Transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(angle), centerOfRotation);
                    break;

                case Geometry.Cylinder:
                    Cylinder.Transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(angle), centerOfRotation);
                    break;

                case Geometry.Cone:
                    Cone.Transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(angle), centerOfRotation);
                    break;
            }
        }

        public void MoveLinear(float distance, Vector3 centerOfRotation)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.Transformation = Matrix4.CreateTranslation(new Vector3(0, distance, 0));
                    break;

                case Geometry.Cylinder:
                    Cylinder.Transformation = Matrix4.CreateTranslation(new Vector3(0, distance, 0));
                    break;

                case Geometry.Cone:
                    Cone.Transformation = Matrix4.CreateTranslation(new Vector3(0, distance, 0));
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

                case Geometry.Cylinder:
                    return Cylinder.GetCenterPoint();

                case Geometry.Cone:
                    return Cone.GetCenterPoint();

                default:
                    return Vector3.Zero;
            }
        }

        public Vector3 GetApexPoint()
        {
            if (Geometry != Geometry.Cone)
            {
                throw new InvalidOperationException($"Tried to call function for Cone while current geometry is {Geometry}");
            }

            return Cone.GetApexPosition();
        }

        public void UpdateModel()
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.UpdateBaseModel();
                    break;
                case Geometry.Cylinder:
                    Cylinder.UpdateBaseModel();
                    break;
                case Geometry.Cone:
                    Cone.UpdateBaseModel();
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
                    Cylinder.RenderCylinder(view, projection);
                    break;

                case Geometry.Cone:
                    Cone.RenderCone(view, projection);
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
                    Cylinder.SetColor(color);
                    break;

                case Geometry.Cone:
                    Cone.SetColor(color);
                    break;
            }
        }
    }
}
