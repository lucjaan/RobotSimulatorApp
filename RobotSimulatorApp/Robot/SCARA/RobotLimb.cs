using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RobotLimb
    {
        public enum JointTypes
        {
            Linear,
            Revolute
        }


        private Matrix4 FirstCenter {  get; set; }
        public Matrix4 RotationCenter { get; set; }
        /// <summary>
        /// Distance between previous and current joint center, used to keep the model rigid after rotation.
        /// </summary>
        public static Vector3 Delta { get; set; }
        public string Name { get; set; }
        public Cube Cube { get; set; }
        public Axis Axis { get; set; }
        /// <summary>
        /// In linear joints it's the distance in one of the axes, in revolute joints it's maximum angle from starting postition
        /// </summary>
        public float MaximumMovement { get; set; }
        public JointTypes JointType { get; set; }
        public RobotLimb(Cube cube, string name, float maximumMovement, JointTypes type, Matrix4 rotationCenter)
        {
            Name = name;
            Cube = cube;
            MaximumMovement = maximumMovement;
            JointType = type;
            FirstCenter = RotationCenter = rotationCenter;

            if (type == JointTypes.Revolute)
            {
                Axis = Axis.Y;
            }
        }

        /// <summary>
        /// When we move one revolute joint, all joints higher on kinematic chain get rotated in relation to it's axis
        /// </summary>
        public void MoveJoint_Angular(float angle, Matrix4 centerOfRotation, Axis axis)
        {
            Debug.WriteLine($"MJ_A {centerOfRotation}");
            Cube.RotateCube(angle, centerOfRotation, axis);
            //UpdateCenter(angle, RotationCenter);
        }

        /// <summary>
        /// When we move one linear joint, all joints higher on kinematic chain get transposed through the same values
        /// </summary>
        public void MoveJoint_Linear(Vector3 translationVector) => Cube.TranslateCube(translationVector);

        //public void UpdateCenter(float angle, Vector3 prevCent)
        //{
        //    float d = MathF.Sqrt(MathF.Pow(prevCent.X - RotationCenter.X, 2) + MathF.Pow(prevCent.Z - RotationCenter.Z, 2));
        //    angle = MathHelper.DegreesToRadians(angle);
        //    float X = prevCent.X + d * (float)Math.Cos(angle);
        //    float Z = prevCent.Z + d * (float)Math.Sin(angle);
        //    RotationCenter = new(X, RotationCenter.Y, Z);
        //}

        public void UpdateCenter(float angle, Matrix4 prevCent)
        {
            RotationCenter = FirstCenter * Cube.CreateRotationYAroundPoint(angle, new Vector3(prevCent.M41, prevCent.M42, prevCent.M43));
        }

        public void UpdateModel(float angle, Matrix4 prevCent)
        {
            RotationCenter = FirstCenter * Cube.CreateRotationYAroundPoint(angle, new Vector3(prevCent.M41, prevCent.M42, prevCent.M43));
            FirstCenter = RotationCenter;
            Cube.UpdateBaseModel();
        }
    }
}
