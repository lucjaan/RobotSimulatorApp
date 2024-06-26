﻿using OpenTK.Mathematics;
using System;
using OpenTK.WinForms;
using RobotSimulatorApp.Shapes;

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
        public float Length { get; set; }
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
            Length = 0f;
        }

        public void CreateCube(float distanceToPointB, float paddingX, float sizeY, float sizeZ)
        {
            if (Geometry != Geometry.Cube)
            {
                throw new InvalidOperationException($"Tried to call function for Cube while current geometry is {Geometry}");
            }

            Cube = new(Position, distanceToPointB, paddingX, sizeY, sizeZ);
            Center = Cube.Center;
        }

        public void CreateCylinder(float radius, float height)
        {
            if (Geometry != Geometry.Cylinder)
            {
                throw new InvalidOperationException($"Tried to call function for Cylinder while current geometry is {Geometry}");
            }

            Cylinder = new(Position, radius, height);
            Center = Cylinder.Center;
        }

        public void CreateCone(float radius, float height)
        {
            if (Geometry != Geometry.Cone)
            {
                throw new InvalidOperationException($"Tried to call function for Cone while current geometry is {Geometry}");
            }

            Cone = new(Position, radius, height);
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

        public float GetLength()
        {
            float result = 0;
            switch (Geometry)
            {
                case Geometry.Cube:
                    return Cube.Length;
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;
            }
            return result;
        }

        public void SetRotationCenter(Vector3 rotationCenter)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.SetRotationCenter(rotationCenter);
                    break; 
                case Geometry.Cylinder:
                case Geometry.Cone:
                    break;
            }
            RotationCenter = rotationCenter;
        }

        public Vector3 GetRotationCenter()
        {
            return Geometry switch
            {
                Geometry.Cube => Cube.GetRotationCenter(),
                Geometry.Cylinder => Cylinder.GetCenterPoint(),
                Geometry.Cone => Cone.GetCenterPoint(),
                _ => Vector3.Zero,
            };
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

        public void RenderModel(Matrix4 view, Matrix4 projection, bool borderShown = false)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.RenderCube(view, projection, borderShown);
                    break;

                case Geometry.Cylinder:
                    Cylinder.RenderCylinder(view, projection, borderShown);
                    break;

                case Geometry.Cone:
                    Cone.RenderCone(view, projection, borderShown);
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
