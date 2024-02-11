using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RobotLimb
    {
        public enum JointTypes
        {
            Linear,
            Revolute
        }


        private Vector3 FirstCenter {  get; set; }
        public Vector3 RotationCenter { get; set; }
        public Matrix4 DHMatrix { get; set; }
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
        public RobotLimb(Cube cube, string name, float maximumMovement, JointTypes type, Vector3 rotationCenter)
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
        public void MoveJoint_Angular(float angle, Vector3 centerOfRotation, Axis axis)
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

        //public void UpdateCenter(float angle, Matrix4 prevCent)
        //{
        //    RotationCenter = FirstCenter * Cube.CreateRotationYAroundPoint(angle, new Vector3(prevCent.M41, prevCent.M42, prevCent.M43));
        //}

        public void SetRotationCenter(Vector3 rot)
        {
            RotationCenter = rot;
        }

        //public void UpdateModel(float angle, Matrix4 prevCent)
        //{
        //    RotationCenter = FirstCenter * Cube.CreateRotationYAroundPoint(angle, new Vector3(prevCent.M41, prevCent.M42, prevCent.M43));
        //    //FirstCenter = RotationCenter;
        //    Cube.UpdateBaseModel();
        //}

        public Matrix4 CreateDHMatrix(float theta, float alpha, float a, float d, Vector3 prevCenter)
        {

            //Matrix4 result = Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationX(alpha);
            Matrix4 result = Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(a, 0, 0)) * Matrix4.CreateRotationX(alpha);
            //Matrix4 result = Matrix4.CreateTranslation(-prevCenter) * Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationX(alpha) * Matrix4.CreateTranslation(prevCenter);
            //Matrix4 result = Matrix4.CreateTranslation(new Vector3(-17,-10,-17)) * Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationX(alpha) * Matrix4.CreateTranslation(new Vector3(17,10,17));
            //Matrix4 result = Matrix4.CreateTranslation(-prevCenter) * Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationX(alpha) * Matrix4.CreateTranslation(prevCenter);

            DHMatrix = result;
            return result;
        }

        //public void UpdateCenter(float angle, Vector3 prevCent)
        //{
        //    angle = MathHelper.DegreesToRadians(angle);
        //    Vector3 oldRot = FirstCenter;
        //    float tX = RotationCenter.X - prevCent.X;
        //    float tZ = RotationCenter.Z - prevCent.Z;

        //    //float tX = - prevCent.X;
        //    //float tZ = - prevCent.Z;

        //    //float rX = tX * MathF.Cos(angle) - tZ * MathF.Sin(angle);
        //    //float rZ = tX * MathF.Sin(angle) + tZ * MathF.Cos(angle);

        //    float rX = tX * MathF.Cos(angle) + tZ * MathF.Sin(angle);
        //    float rZ = tX * -MathF.Sin(angle) + tZ * MathF.Cos(angle);

        //    rX = (float)Math.Round(rX, 3);
        //    rZ = (float)Math.Round(rZ, 3);
        //    RotationCenter = new(rX + prevCent.X, FirstCenter.Y, rZ + prevCent.Z);
        //    //RotationCenter = new(rX + prevCent.X, FirstCenter.Y, rZ + prevCent.Z);
        //    //Cube.Model = Cube.BaseModel * Matrix4.CreateTranslation(oldRot - RotationCenter) * Cube.Model;
        //   // Cube.Model = Matrix4.Identity;
        //    Debug.WriteLine($"rot {RotationCenter}");

        //    //RotationCenter = new Vector3(17f, 10f, 52f);
        //}
    }
}
