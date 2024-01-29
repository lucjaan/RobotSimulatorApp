using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    internal class RobotLimb
    {
        public enum JointTypes
        {
            Linear,
            Revolute
        }


        public Vector3 RotationCenter { get; set; }
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
            RotationCenter = rotationCenter;

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
            angle = MathHelper.DegreesToRadians(angle);
            Cube.RotateCube(angle, centerOfRotation, axis);
            //UpdateCenter(angle, RotationCenter);
        }

        /// <summary>
        /// When we move one linear joint, all joints higher on kinematic chain get transposed through the same values
        /// </summary>
        public void MoveJoint_Linear(Vector3 translationVector) => Cube.TranslateCube(translationVector);


        //public void UpdateCenter(float angle, Vector3 externalCenter)
        //{
        //    angle = MathHelper.DegreesToRadians(angle);

        //    Vector3 goToOrigin = - externalCenter;
        //    Vector3 translateOnOrigin = new(
        //        (MathF.Cos(angle) * goToOrigin.X) - (MathF.Sin(angle) * goToOrigin.Z),
        //        goToOrigin.Y,
        //        (MathF.Sin(angle) * goToOrigin.X) + (MathF.Cos(angle) * goToOrigin.Z)
        //        );

        //    RotationCenter = translateOnOrigin + externalCenter;
        //    //RotationCenter = translateOnOrigin + externalCenter;
        //}

        public void UpdateCenter(float angle, Vector3 prevCent)
        {
            angle = MathHelper.DegreesToRadians(angle);
            //Vector3 goToVector = RotationCenter - prevCent;
            //Vector3 translateOnOrigin = new(
            //    (MathF.Cos(angle) * goToVector.X) - (MathF.Sin(angle) * goToVector.Z),
            //    goToVector.Y,
            //    (MathF.Sin(angle) * goToVector.X) + (MathF.Cos(angle) * goToVector.Z)
            //    );
            //Cube.RotateCube(angle, prevCent, Axis.Y);
            //var x = Cube.Model;
            //Cube.UpdateCenter(angle);
            //RotationCenter = Cube.Center;
            //var z = Cube.Model;
            //Vector3 v = RotationCenter; //- prevCent;
            //Vector3 newCenter = new(
            //    v.X * MathF.Cos(angle) - v.X * MathF.Sin(angle),
            //    v.Y,
            //    v.X * MathF.Sin(angle) + v.X * MathF.Cos(angle)
            //    );
            //RotationCenter = newCenter;


            //Matrix4 tr0 = Matrix4.CreateTranslation(-RotationCenter);
            //Matrix4 tr1 = Matrix4.CreateRotationY(angle);
            //Matrix4 tr2 = Matrix4.CreateTranslation(RotationCenter);

            //Matrix4 tr01 = tr0 * tr1;
            //Matrix4 tr12 = tr01 * tr2;
            //Matrix4 tr = Matrix4.CreateTranslation(-RotationCenter) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(RotationCenter);

            //Vector4 result = new(
            //    tr.M11 * RotationCenter.X + tr.M12 * RotationCenter.Y + tr.M13 * RotationCenter.Z + tr.M14 * 1,
            //    tr.M21 * RotationCenter.X + tr.M22 * RotationCenter.Y + tr.M23 * RotationCenter.Z + tr.M24 * 1,
            //    tr.M31 * RotationCenter.X + tr.M32 * RotationCenter.Y + tr.M33 * RotationCenter.Z + tr.M34 * 1,
            //    1f);

            //RotationCenter = new(result.X, result.Y, result.Z);

            //RotationCenter = prevCent + translateOnOrigin;
        }
    }
}
