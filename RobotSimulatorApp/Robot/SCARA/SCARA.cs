﻿using OpenTK.Graphics.Egl;
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
                //Debug.WriteLine($"Joint: {0} angle: {value - RobotJoints[0].Distance} ");
                //Debug.WriteLine($"Joint: {1} angle: {value - RobotJoints[1].Distance} ");
                //Debug.WriteLine($"Joint: {2} angle: {value - RobotJoints[2].Distance} ");
                //Debug.WriteLine($"Joint: {3} angle: {value - RobotJoints[3].Distance} ");

                marker.SetPosition(JointCenters[0]);
                marker1.SetPosition(JointCenters[1]);
                marker2.SetPosition(JointCenters[2]);
                marker3.SetPosition(JointCenters[3]);

                for (int i = jointId; i < RobotJoints.Count; i++)
                {
                    RobotJoints[i].MoveJoint_Angular(value - RobotJoints[jointId].Distance, JointCenters[jointId], joint.Axis);

                    //RobotJoints[i].MoveJoint_Angular(value, JointCenters[jointId], joint.Axis);
                    //RobotJoints[i].MoveJoint_Angular(value - RobotJoints[jointId].Distance, JointCenters[jointId], joint.Axis);
                    Debug.WriteLine($"Joint: {i} angle: {value - RobotJoints[jointId].Distance} value {value} Disance {RobotJoints[jointId].Distance}");

                    //RobotJoints[0].MoveJoint_Angular(value, JointCenters[jointId], joint.Axis);
                }
                //RobotJoints[0].MoveJoint_Angular(value, JointCenters[jointId], joint.Axis);


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

        public void UpdateJointValues(float th1, float th2, float d3, float th4)
        {
            RobotJoints[0].Distance = th1;
            RobotJoints[1].Distance = th2;
            RobotJoints[2].Distance = d3;
            RobotJoints[3].Distance = th4;

            DHParameters[0] = new Vector4(th1, 0, 0, 0);
            DHParameters[1] = new Vector4(th2, 0, 0, 0);
            DHParameters[2] = new Vector4(0, 0, d3, 0);
            DHParameters[3] = new Vector4(th4, 0, 0, 0);
            //UpdateModels([th1, th2, d3, th4]);
            //UpdateModels();
        }
        
        public Matrix4 CalculateTransformation(int jointId)
        {
            Matrix4 t1, t2, t3, t4;
            t1 = t2 = t3 = t4 = Matrix4.Identity;
            Matrix4 transformation = Matrix4.Identity;  

            //transformation = Helpers.CreateRotationYAroundPoint(DHParameters[0].X, JointCenters[0]);
            t1 = Helpers.CreateRotationYAroundPoint(DHParameters[0].X, JointCenters[0]);
            t2 = Helpers.CreateRotationYAroundPoint(DHParameters[1].X, JointCenters[1]);
            t3 = Helpers.CreateRotationYAroundPoint(DHParameters[2].X, JointCenters[2]);
            t4 = Helpers.CreateRotationYAroundPoint(DHParameters[3].X, JointCenters[3]);

            for (int i = 0; i < DHParameters.Count; i++)
            {
                //transformation *= Helpers.CreateRotationYAroundPoint(DHParameters[i].X, JointCenters[i]);
                transformation *= Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(DHParameters[i].X), JointCenters[i]);
            }

            //transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(176f), JointCenters[1]);
            return transformation;
        }

        public void UpdateModels()
        {
            //int i = 0;
            //RobotJoints[0].Cube.UpdateBaseModel(DHParameters[i].X);
            //RobotJoints[1].Cube.UpdateBaseModel(DHParameters[i].Y);
            //RobotJoints[2].Cube.UpdateBaseModel(DHParameters[i].Z);
            //RobotJoints[3].Cube.UpdateBaseModel(DHParameters[i].W);
            //RobotJoints[jointId].Cube.UpdateBaseModel();

            //Matrix4[] trans = {
            //RobotJoints[0].Cube.Transformation,
            //RobotJoints[1].Cube.Transformation,
            //RobotJoints[2].Cube.Transformation,
            //RobotJoints[3].Cube.Transformation,
            //};

            Matrix4 transformation = Matrix4.Identity;
            for (int i = 0; i < RobotJoints.Count; i++)
            {
                //transformation *= RobotJoints[i].Cube.Transformation;
                //RobotJoints[i].Cube.Transformation = transformation;
                //Debug.WriteLine(i);
                //RobotJoints[i].Cube.UpdateBaseModel(CalculateTransformation(i));
                RobotJoints[i].Cube.UpdateBaseModel();
                //RobotJoints[1].Cube.UpdateBaseModel(CalculateTransformation(1));
            }

//Debug.WriteLine(RobotJoints[1].Cube.Transformation);

            //foreach (RobotLimb joint in RobotJoints)
            //{
            //    joint.Cube.UpdateBaseModel(transformation);
            //}
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

    }
}
