﻿using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System.Collections.Generic;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {

        private Vector3 Position { get; set; }
        public List<RobotLimb> RobotJoints = [];
        //public List<RobotLimb2> RobotJoints = [];
        public List<Matrix4> DenavitHartenbergTable = new List<Matrix4>(4);
        /// <summary>
        /// X = theta, Y = a, Z = d, W = alpha
        /// </summary>
        public List<Vector4> DHParameters = [];
        public List<Vector3> JointCenters = [];
        private GLControl GLControl { get; set; }
        public Cube RobotBase;

        public Cube marker1;
        public Cube marker2;
        public Cube marker3;
        public Cube marker4;

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

            marker1 = new(GLControl, Vector3.Zero, 1, 500, 1);
            marker2 = new(GLControl, Vector3.Zero, 1, 500, 1);
            marker3 = new(GLControl, Vector3.Zero, 1, 500, 1);
            marker4 = new(GLControl, Vector3.Zero, 1, 500, 1);

            marker1.SetColor(Color4.Yellow);
            marker2.SetColor(Color4.Yellow);
            marker3.SetColor(Color4.Yellow);
            marker4.SetColor(Color4.Yellow);

            RobotBase = new(GLControl, Vector3.Zero, 34f, 20f, 34f);
            RobotBase.SetColor(Color4.DarkOrange);

            RobotJoints.Add(CreateRectangularLimb("J1", new Vector3(9f, 20f, 9f), 40f, 6f, 14f, 9f));
            RobotJoints.Add(CreateRectangularLimb("J2", new Vector3(44f, 26f, 6f), 35f, 20f, 15f, 30f));
            RobotJoints.Add(CreateCylindricalLimb("J3", new Vector3(79f, 6.5f, 13.5f), 7.95f, 68.3f, 25f));
            RobotJoints.Add(CreateRectangularLimb("J4", new Vector3(74f, 6.5f, 6f), 3.5f, 4.8f, 2.8f, 21f));

            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].SetColor(Color4.LightSlateGray);
                DenavitHartenbergTable.Add(Matrix4.Identity);
            }
            CreateJointCenters();
        }

        public void MoveJoint(int jointId, float value)
        {
            marker1.SetPosition(RobotJoints[0].GetRotationCenter());
            marker2.SetPosition(RobotJoints[1].GetRotationCenter());
            marker3.SetPosition(RobotJoints[2].GetRotationCenter());
            marker4.SetPosition(RobotJoints[3].GetRotationCenter());

            for (int i = 0; i < RobotJoints.Count; i++)
            {
                JointCenters[i + 1] = RobotJoints[i].GetRotationCenter();
            }

            for (int i = jointId; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].Move(value - RobotJoints[jointId].Distance, JointCenters[jointId]);
            }
        }

        public void CreateJointCenters()
        {
            float th1, th2, d3, th4;
            th1 = th2 = d3 = th4 = 0;

            DHParameters.Add(new Vector4(th1, 0, 35f, 0));
            DHParameters.Add(new Vector4(th2, 0, 30f, 0));
            DHParameters.Add(new Vector4(0, d3, 0, 0));
            DHParameters.Add(new Vector4(th4, 0, 0, 0));

            Vector3 j0 = RobotBase.Center;
            Vector3 j1 = new(40f, RobotJoints[0].Center.Y, RobotJoints[0].Center.Z);
            Vector3 j2 = new(35f, RobotJoints[1].Center.Y, RobotJoints[1].Center.Z);
            Vector3 j3 = RobotJoints[2].Center;
            Vector3 j4 = RobotJoints[3].Center;

            marker1.SetPosition(j1);
            marker2.SetPosition(j2);
            marker3.SetPosition(j3);
            marker4.SetPosition(j4);

            RobotJoints[0].SetRotationCenter(j1);
            RobotJoints[1].SetRotationCenter(j2);
            RobotJoints[2].SetRotationCenter(j3);
            RobotJoints[3].SetRotationCenter(j4);

            JointCenters.Add(j0);
            JointCenters.Add(j1);
            JointCenters.Add(j2);
            JointCenters.Add(j3);
            JointCenters.Add(j4);
        }

        public void UpdateJointValues(float th1, float th2, float d3, float th4)
        {
            RobotJoints[0].Distance = th1;
            RobotJoints[1].Distance = th2;
            RobotJoints[2].Distance = d3;
            RobotJoints[3].Distance = th4;

            DHParameters[0] = new Vector4(th1, 0, 0, 0);
            DHParameters[1] = new Vector4(th2, 0, 0, 0);
            DHParameters[2] = new Vector4(0, d3, 0, 0);
            DHParameters[3] = new Vector4(th4, 0, 0, 0);
        }

        public void UpdateModels()
        {
            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].UpdateModel();
            }
        }

        public void RenderRobot(Matrix4 view, Matrix4 projection)
        {
            RobotBase.RenderCube(view, projection);

            foreach (RobotLimb joint in RobotJoints)
            {
                joint.RenderModel(view, projection);
            }

            //marker1.RenderCube(view, projection);
            //marker2.RenderCube(view, projection);
            //marker3.RenderCube(view, projection);
            //marker4.RenderCube(view, projection);
        }

        public void SaveToFile(string filePath)
        {
            SaveToFile();
        }

        public override void CreateKinematicChain()
        {
            //TODOs
        }

        public RobotLimb CreateRectangularLimb(string name,  Vector3 position, float sizeX, float sizeY, float sizeZ, float maxMovement)
        {
            RobotLimb limb = new(GLControl, name, Geometry.Cube, position, maxMovement);
            limb.CreateCube(sizeX, sizeY, sizeZ);
            return limb;
        }

        public RobotLimb CreateCylindricalLimb(string name, Vector3 position, float radius, float height, float maxMovement)
        {
            RobotLimb limb = new(GLControl, name, Geometry.Cylinder, position, maxMovement);
            limb.CreateCylinder(radius, height);
            return limb;
        }
    }
}
