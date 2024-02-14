using OpenTK.Graphics.Egl;
using OpenTK.Graphics.ES11;
using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {

        private Vector3 Position { get; set; }
        //protected Dictionary<int, RobotJoint> RobotJoints { get; set; }
        public List<RobotLimb> RobotJoints = [];
        public List<Matrix4> DenavitHartenbergTable = new List<Matrix4>(4);
        /// <summary>
        /// X = theta, Y = a, Z = d, W = alpha
        /// </summary>
        public List<Vector4> DHParameters = [];
        public List<Vector3> JointCenters = [];
        public List<Vector3> FirstJointCenters = [];
        private GLControl GLControl { get; set; }
        public Cube RobotBase;
        public Cube marker;
        public Cube marker1;
        public Cube marker2;
        public Cube marker3;
        private Cube Manipulator;

        public SCARA_Robot(GLControl glc, string name)
        {
            RobotType = RobotTypes.SCARA;
            Name = name;
            GLControl = glc;
            CreateRobot();
        }

        public void CreateRobot()
        {
            RobotBase = new(GLControl, Vector3.Zero, new Vector3(34f, 20f, 34f));
            marker = new(GLControl, Vector3.Zero, new Vector3(1, 70f, 1));
            marker1 = new(GLControl, Vector3.Zero, new Vector3(1, 70f, 1));
            marker2 = new(GLControl, Vector3.Zero, new Vector3(1, 70f, 1));
            marker3 = new(GLControl, Vector3.Zero, new Vector3(1, 70f, 1));
            marker.SetColor(Color4.Red);
            marker1.SetColor(Color4.Green);
            marker2.SetColor(Color4.Blue);
            marker3.SetColor(Color4.White);
            //RobotBase = new(GLControl, Vector3.Zero, new Vector3(34f, 20f, 34f));
            RobotBase.SetColor(Color4.DarkOrange);

            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, RobotBase.Center + new Vector3(35f, 0f, 0f)));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, RobotBase.Center + new Vector3(65f, 0f, 0f )));
            RobotJoints.Add(CreateLinearJoint(
                GLControl, "J4", new Vector3(74f, 6.5f, 6f), new Vector3(3.5f, 4.8f, 2.8f), 21f, RobotBase.Center + new Vector3(65f, 0f, 0f)));


            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].Cube.SetColor(Color4.LightSlateGray);
                DenavitHartenbergTable.Add(Matrix4.Identity);
            }
            CreateJointCenters();

        }

        public void MoveJoint(int jointId, float value)
        {
            RobotLimb joint = RobotJoints[jointId];

            if (joint.JointType == RobotLimb.JointTypes.Revolute)
            {

                for (int i = jointId; i < RobotJoints.Count; i++)
                {
                    RobotJoints[i].MoveJoint_Angular(value, JointCenters[jointId], joint.Axis);
                }
                marker.SetPosition(JointCenters[0]);
                marker1.SetPosition(JointCenters[1]);
                marker2.SetPosition(JointCenters[2]);
                marker3.SetPosition(JointCenters[3]);
            }
            CalculateCenters();
        }

        public void CreateJointCenters()
        {
            Vector3 j0 = RobotBase.Center;
            Vector3 j1 = new(j0.X + 35, j0.Y, j0.Z);
            Vector3 j2 = new(j1.X + 30, j1.Y, j1.Z);
            Vector3 j3 = new(j2.X, j2.Y, j2.Z);
            Vector3 j4 = new(j3.X, j3.Y, j3.Z);

            float th1, th2, d3, th4;
            th1 = th2 = d3 = th4 = 0;

            DHParameters.Add(new Vector4(th1, 0, 35f, 0));
            DHParameters.Add(new Vector4(th2, 0, 30f, 0));
            DHParameters.Add(new Vector4(0, d3, 0, 0));
            DHParameters.Add(new Vector4(th4, 0, 0, 0));

            JointCenters.Add(j0);
            JointCenters.Add(j1);
            JointCenters.Add(j2);
            JointCenters.Add(j3);
            JointCenters.Add(j4);

            FirstJointCenters.Add(j0);
            FirstJointCenters.Add(j1);
            FirstJointCenters.Add(j2);
            FirstJointCenters.Add(j3);
            FirstJointCenters.Add(j4);

        }

        public void CalculateCenters()
        {
            for(int i = 1; i < JointCenters.Count; i++)
            {
                JointCenters[i] = UpdateCenter(i);
            }
        }

        public Vector3 CalculateCenter(int jointID)
        {

            Vector4 dh = DHParameters[jointID];
            //float angleY, distanceY, angleX, distanceX;
            //angleY = distanceY = angleX = distanceX = 0;
            //angleY = 90;
            float aY = MathHelper.DegreesToRadians(dh.X);
            Vector3 dY = new(0, dh.Y, 0);
            Vector3 dX = new(dh.Z, 0, 0);
            float aX = MathHelper.DegreesToRadians(dh.W);

            Matrix4 baseMinus = Matrix4.CreateTranslation(-RobotBase.Center);
            Matrix4 basePlus = Matrix4.CreateTranslation(RobotBase.Center);
            //Matrix4 dhMatrix = Matrix4.Identity;

            //if (jointID == 0)
            //{
            //    dhMatrix = RobotBase.RotateCube(dh.X, RobotBase.Center, Axis.Y) * Matrix4.CreateTranslation(dY) * Matrix4.CreateTranslation(dX) * RobotBase.RotateCube(dh.W, RobotBase.Center, Axis.X);
            //}
            //else
            //{
            //    Matrix4 oldM = DenavitHartenbergTable[jointID - 1];
            //    Vector3 oldV = new(
            //    oldM.M41 * oldM.M11 + oldM.M42 * oldM.M21 + oldM.M43 * oldM.M31,
            //    oldM.M41 * oldM.M12 + oldM.M42 * oldM.M22 + oldM.M43 * oldM.M32,
            //    oldM.M41 * oldM.M13 + oldM.M42 * oldM.M23 + oldM.M43 * oldM.M33
            //    );
            //    dhMatrix = RobotBase.RotateCube(dh.X, oldV, Axis.Y) * Matrix4.CreateTranslation(dY) * Matrix4.CreateTranslation(dX) * RobotBase.RotateCube(dh.W, oldV, Axis.X);
            //}


            //Matrix4 dhMatrix = Matrix4.CreateRotationY(aY) * Matrix4.CreateTranslation(dY) * Matrix4.CreateTranslation(dX) * Matrix4.CreateRotationX(aX);
            Matrix4 dhMatrix = Matrix4.CreateTranslation(dY);
            dhMatrix *= Matrix4.CreateRotationY(aY);
            dhMatrix *= Matrix4.CreateTranslation(dX);
            dhMatrix *= Matrix4.CreateRotationX(aX);

            float sinT = (float)MathHelper.Sin(aY);
            float cosT = (float)MathHelper.Cos(aY);
            float sinA = (float)MathHelper.Sin(aX);
            float cosA = (float)MathHelper.Cos(aX);

            var dh1 = dhMatrix;

            Matrix4 dh2 = new Matrix4(
                cosT, -sinT * cosA, sinT * sinA, dh.Z * cosT,
                sinT, cosT * cosA, -cosT * sinA, dh.Z * sinT,
                0, sinA, cosA, dh.Y,
                0, 0, 0, 1
                );
            //dh2.Transpose();
            //dhMatrix = dh2;
            //dhMatrix *= Matrix4.CreateTranslation(dY);
            //dhMatrix *= Matrix4.CreateTranslation(dX)
            if (jointID == 0)
            {
                dhMatrix = basePlus * dhMatrix;
            }
            else
            {
                dhMatrix = DenavitHartenbergTable[jointID - 1] * dhMatrix;
            }

            DenavitHartenbergTable[jointID] = dhMatrix;
            var dhx = DenavitHartenbergTable;
            //DenavitHartenbergTable[jointID] = dhMatrix * basePlus;


            Matrix4 rM = dhMatrix;
            //Matrix4 rM = basePlus * dhMatrix * baseMinus;

            //substract
            float vX = (float)Math.Round(rM.M41, 2);
            float vY = (float)Math.Round(rM.M42, 2);
            float vZ = (float)Math.Round(rM.M43, 2);
            float newX = vX * (float)Math.Round(rM.M11, 2) + vY * (float)Math.Round(rM.M21, 2) + vZ * (float)Math.Round(rM.M31, 2);
            float newY = vX * (float)Math.Round(rM.M12, 2) + vY * (float)Math.Round(rM.M22, 2) + vZ * (float)Math.Round(rM.M32, 2);
            float newZ = vX * (float)Math.Round(rM.M13, 2) + vY * (float)Math.Round(rM.M23, 2) + vZ * (float)Math.Round(rM.M33, 2);

            Vector3 cen = JointCenters[jointID] - RobotBase.Center;
            newX = cen.X * (float)MathHelper.Cos(aY) + cen.Z * (float)MathHelper.Sin(aY);
            newZ = cen.X * -(float)MathHelper.Sin(aY) + cen.Z * (float)MathHelper.Cos(aY);
            //float newX = rM.M41 * rM.M11 + rM.M42 * rM.M21 + rM.M43 * rM.M31;
            //float newY = rM.M41 * rM.M12 + rM.M42 * rM.M22 + rM.M43 * rM.M32;
            //float newZ = rM.M41 * rM.M13 + rM.M42 * rM.M23 + rM.M43 * rM.M33;
            //addd

            Vector3 result = new Vector3(cen.X, 0, cen.Z) + RobotBase.Center;
            Vector3 xcv = new Vector3(rM.M41, rM.M42, rM.M43);
            //Debug.WriteLine($"{xcv} ID {jointID}");

            //return new Vector3(newX, newY, newZ);
            return result;
            //return new Vector3(rM.M41, rM.M42, rM.M43);
        }

        public Vector3 UpdateCenter(int jointID)
        {
            Vector4 dh = DHParameters[jointID - 1];
            float aY = MathHelper.DegreesToRadians(dh.X);
            Vector3 dY = new(0, dh.Y, 0);
            Vector3 dX = new(dh.Z, 0, 0);
            float aX = MathHelper.DegreesToRadians(dh.W);
            Vector3 prevCent = JointCenters[jointID - 1];

            //Move point to origin
            Vector3 vec = FirstJointCenters[jointID] - prevCent;
            //Translate Y
            vec += dY;
            //Rotate around Y axis
            float x = vec.X * (float)MathHelper.Cos(aY) + vec.Z * (float)MathHelper.Sin(aY);
            float y = vec.Y;
            float z = vec.X * -(float)MathHelper.Sin(aY) + vec.Z * (float)MathHelper.Cos(aY);
            vec = new Vector3(x, y, z);
            //Translate X
            vec += dX;
            //Rotate around X axis
            x = vec.X;
            y = vec.Y * (float)MathHelper.Cos(aX) * vec.Z * -(float)MathHelper.Sin(aX);
            z = vec.Y * (float)MathHelper.Sin(aX) * vec.Z * (float)MathHelper.Cos(aX);
            vec = new Vector3(x, vec.Y + y, vec.Z + z);
            //Return from origin
            vec += prevCent;
            return vec;
        }

        //public void MoveJoint(int jointId, float value)
        //{
        //    //value = MathHelper.Clamp(value, -RobotJoints[jointId].MaximumMovement / 2, RobotJoints[jointId].MaximumMovement / 2);
        //    var joint = RobotJoints[jointId];

        //    if (joint.JointType == RobotLimb.JointTypes.Revolute)
        //    {
        //        joint.MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
        //        for (int i = jointId + 1; i < 2; i++)
        //        {
        //            RobotJoints[i].MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
        //        }
        //    }
        //}

        //public void MoveRobot()
        //{
        //    //Matrix4 rotation = Matrix4.CreateTranslation(-17, -10, -17);
        //    Matrix4 rotation = Matrix4.Identity;
        //    Matrix4 J0, J1, J2, J3, J4;
        //    J1 = J2 = J3 = J4 = Matrix4.Identity;
        //    //for (int i = 0; i < DenavitHartenbergTable.Count; i++)
        //    //{
        //    //    rotation *= DenavitHartenbergTable[i];
        //    //    //RobotJoints[i].Cube.SetTransformation(rotation * Matrix4.CreateTranslation(17, 10, 17));
        //    //    //RobotJoints[i].SetRotationCenter(rotation * Matrix4.CreateTranslation(17, 10, 17));
        //    //    RobotJoints[i].SetRotationCenter(rotation * Matrix4.CreateTranslation(52, 10, 17));
        //    //}
        //    J0 = Matrix4.CreateTranslation(RobotBase.Center);
        //    J1 = DenavitHartenbergTable[0] * Matrix4.CreateTranslation(17, 10, 17);
        //    J2 = DenavitHartenbergTable[0] * DenavitHartenbergTable[1] * Matrix4.CreateTranslation(17, 10, 17);
        //    J3 = DenavitHartenbergTable[0] * DenavitHartenbergTable[1] * DenavitHartenbergTable[2] * Matrix4.CreateTranslation(17, 10, 17);

        //    List<Vector3> centers = new List<Vector3>();
        //    List<Matrix4> rotM = new List<Matrix4>() { J0, J1, J2, J3 };
        //    int i = 0;
        //    foreach(RobotLimb limb in RobotJoints)
        //    {
        //        //centers.Add(limb.RotationCenter);
        //        limb.MoveJoint(rotM[i]);
        //        i++;
        //    }

        //    var z = centers;
        //}

        //public void MoveRobot(int jointID, float angle)
        public void MoveRobot(float angle1, float angle2 = 0)
        {
            //float angleY, float distanceY, float angleX, float distanceX
            angle1 = MathHelper.DegreesToRadians(angle1);
            angle2 = MathHelper.DegreesToRadians(angle2);
            float angleY, distanceY, angleX, distanceX;
            angleY = angle1;
            //angleY = MathHelper.DegreesToRadians(180);
            distanceY = angleX = distanceX = 0;
            Matrix4 j0 = Matrix4.CreateTranslation(-RobotBase.Center);
            Matrix4 j0DH = Matrix4.CreateRotationY(angleY) * Matrix4.CreateTranslation(new Vector3(0, distanceY, 0)) * 
                Matrix4.CreateTranslation(new Vector3(distanceX, 0, 0)) * Matrix4.CreateRotationX(angleX);
            j0 = j0 * j0DH * Matrix4.CreateTranslation(RobotBase.Center);
            RobotJoints[0].Cube.SetTransformation(j0);

            angleY = angle2;
            Matrix4 j1 = Matrix4.CreateTranslation(-(RobotBase.Center.X +35), -RobotBase.Center.Y, -RobotBase.Center.Z);
            Matrix4 j1DH = j0DH * Matrix4.CreateRotationY(angleY) * Matrix4.CreateTranslation(new Vector3(0, distanceY, 0)) *
                Matrix4.CreateTranslation(new Vector3(35f, 0, 0)) * Matrix4.CreateRotationX(angleX);
            j1 = j1 * j1DH * Matrix4.CreateTranslation(RobotBase.Center.X + 35, RobotBase.Center.Y, RobotBase.Center.Z);
            RobotJoints[1].Cube.SetTransformation(j1);

        }


        public void UpdateJointValues(float th1, float th2, float d3, float th4)
        {

            DHParameters[0] = new Vector4(th1, 0, 0, 0);
            DHParameters[1] = new Vector4(th2, 0, 0, 0);
            DHParameters[2] = new Vector4(0, 0, d3, 0);
            DHParameters[3] = new Vector4(th4, 0, 0, 0);

        }

        public void UpdateModels()
        {
            foreach(RobotLimb joint in RobotJoints)
            {
                joint.Cube.UpdateBaseModel();
            }
        }

        public void RenderRobot(Matrix4 view, Matrix4 projection)
        {
            RobotBase.RenderCube(view, projection);
            marker.RenderCube(view, projection);
            marker1.RenderCube(view, projection);
            marker2.RenderCube(view, projection);
            marker3.RenderCube(view, projection);
            //Manipulator.RenderCube(view, projection);
            foreach (RobotLimb joint in RobotJoints)
            {
                joint.Cube.RenderCube(view, projection);
            }
        }

        public void SaveToFile(string filePath)
        {
            SaveToFile();
        }

        public override void CreateKinematicChain()
        {
            //TODOs
        }

        public RobotLimb CreateRevoluteJoint(GLControl glc, string name, Vector3 position, Vector3 size, float maximumDistance, Vector3 movementPoint)
            //=> new(new Cube(glc, position, size.X, size.Y, size.Z), name, maximumDistance, RobotJoint.JointTypes.Revolute, movementPoint);
            => new(new Cube(glc, position, size), name, maximumDistance, RobotLimb.JointTypes.Revolute, movementPoint);

        public RobotLimb CreateLinearJoint(GLControl glc, string name, Vector3 position, Vector3 size, float maximumAngle, Vector3 rotationCenter)
           //=> new(new Cube(glc, position, size.X, size.Y, size.Z), name, maximumAngle, RobotJoint.JointTypes.Linear, rotationCenter);
           => new(new Cube(glc, position, size), name, maximumAngle, RobotLimb.JointTypes.Linear, rotationCenter);

        //Cube cube = new Cube(glc, position, length, height, width);
        //RobotJoints.Add(RobotJoints.Keys.Count, rj);


        //public RobotJoint CreateLinearJoint(GLControl glc, string name, Vector3 position, float length, float height, float width, float maximumAngle, Vector3 rotationCenter)
        //{
        //    RobotJoint rj = CreateJoint(glc, name, position, length, height, width, maximumAngle);
        //    rj.RotationCenter = rotationCenter;
        //    return rj;
        //}

        //        public RobotJoint(GLControl glc, Vector3 position, int x, int y, int z, float maximumAngle, Vector3 rotationCenter)

    }
}
