using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {
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
        public Vector3 Center;
        public Vector3 Tool;
        public float Height;
        public float Radius;
        public event EventHandler<Vector3> RobotMoved;
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

            //WIP
            Height = 100f;
            Radius = 150f;
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

            //RobotBase = new(GLControl, new Vector3(-17f, 0f, -17f), 34f, 20f, 34f);
            marker1.SetPosition(Vector3.Zero);
            RobotBase = new(GLControl, Vector3.Zero, 34f, 20f, 34f);
            Center = Helpers.GetPositionFromMatrix(RobotBase.CenterPoint);
            RobotBase.SetColor(Color4.DarkOrange);

            Vector3 p0 = RobotBase.GetRotationCenter();
            RobotLimb j1 = CreateRectangularLimb("J1", p0, 35f, 4.5f, 6f, 14f, 0f);
            marker2.SetColor(Color4.Yellow);
            Vector3 p1 = j1.GetRotationCenter();
            RobotLimb j2 = CreateRectangularLimb("J2", p1, 30f, 2.5f, 6f, 14f, 0f);
            Vector3 p2 = j2.GetRotationCenter();
            RobotLimb j3 = CreateCylindricalLimb("J3", new Vector3(p2.X, 6.5f, p2.Z), 7.95f, 68.3f, 25f);
            Vector3 p3 = j3.GetRotationCenter();
            RobotLimb j4 = CreateConicalLimb("Manipulator", p3, 6.3f, -3.8f, 21f);
            Vector3 p4 = j4.GetRotationCenter();

            RobotJoints.Add(j1);
            RobotJoints.Add(j2);
            RobotJoints.Add(j3);
            RobotJoints.Add(j4);

            JointCenters.Add(p0);
            JointCenters.Add(p1);
            JointCenters.Add(p2);
            JointCenters.Add(p3);
            JointCenters.Add(p4);

            //RobotJoints.Add(CreateRectangularLimb("J1", new Vector3(-6f, 20f, -6f), 40f, 6f, 14f, 9f));
            //RobotJoints.Add(CreateRectangularLimb("J2", new Vector3(27f, 26f, -9f), 35f, 20f, 15f, 30f));
            //RobotJoints.Add(CreateCylindricalLimb("J3", new Vector3(62f, 6.5f, -3.5f), 7.95f, 68.3f, 25f));
            //RobotJoints.Add(CreateConicalLimb("Manipulator", new Vector3(62f, 6.5f, -3.5f), 6.3f, -3.8f, 21f));


            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].SetColor(Color4.LightSlateGray);
                DenavitHartenbergTable.Add(Matrix4.Identity);
            }
            //CreateJointCenters();
        }

        public void MoveRevoluteJoint(int jointId, float value)
        {
            //marker1.SetPosition(RobotJoints[0].GetRotationCenter());
            //marker2.SetPosition(RobotJoints[1].GetRotationCenter());
            //marker3.SetPosition(RobotJoints[2].GetRotationCenter());
            //marker4.SetPosition(RobotJoints[3].GetRotationCenter());

            for (int i = 0; i < RobotJoints.Count; i++)
            {
                JointCenters[i + 1] = RobotJoints[i].GetRotationCenter();
            }

            for (int i = jointId; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].MoveRevolute(value - RobotJoints[jointId].Distance, JointCenters[jointId]);
            }

            OnRobotMoved(RobotJoints[RobotJoints.Count - 1].GetApexPoint());
        }

        public void MoveLinearJoint(int jointId, float value)
        {
            //marker1.SetPosition(RobotJoints[0].GetRotationCenter());
            //marker2.SetPosition(RobotJoints[1].GetRotationCenter());
            //marker3.SetPosition(RobotJoints[2].GetRotationCenter());
            //marker4.SetPosition(RobotJoints[3].GetRotationCenter());

            for (int i = 0; i < RobotJoints.Count; i++)
            {
                JointCenters[i + 1] = RobotJoints[i].GetRotationCenter();
            }

            for (int i = jointId; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].MoveLinear(value - RobotJoints[jointId].Distance, JointCenters[jointId]);
            }
            OnRobotMoved(RobotJoints[RobotJoints.Count -1].GetApexPoint());
        }

        public virtual void OnRobotMoved(Vector3 position)
        {
            RobotMoved?.Invoke(this, position);
            Tool = position;
        }

        public void CreateJointCenters()
        {
            float th1, th2, d3, th4;
            th1 = th2 = d3 = th4 = 0;

            DHParameters.Add(new Vector4(th1, 0, 35f, 0));
            DHParameters.Add(new Vector4(th2, 0, 30f, 0));
            DHParameters.Add(new Vector4(0, d3, 0, 0));
            DHParameters.Add(new Vector4(th4, 0, 0, 0));

            Vector3 j0 = Center;
            Vector3 j1 = new(40f, RobotJoints[0].Center.Y, RobotJoints[0].Center.Z);
            Vector3 j2 = new(35f, RobotJoints[1].Center.Y, RobotJoints[1].Center.Z);
            Vector3 j3 = RobotJoints[2].Center;
            Vector3 j4 = RobotJoints[3].Center;

            //marker1.SetPosition(j1);
            //marker2.SetPosition(j2);
            //marker3.SetPosition(j3);
            //marker4.SetPosition(j4);

            //HEAVY WIP
            //RobotJoints[0].Length = 40f;
            //RobotJoints[1].Length = 35f;

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

            //DHParameters[0] = new Vector4(th1, 0, 0, 0);
            //DHParameters[1] = new Vector4(th2, 0, 0, 0);
            //DHParameters[2] = new Vector4(0, d3, 0, 0);
            //DHParameters[3] = new Vector4(th4, 0, 0, 0);
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

            marker1.RenderCube(view, projection);
            //marker2.RenderCube(view, projection);
            //marker3.RenderCube(view, projection);
            //marker4.RenderCube(view, projection);
        }

        public void MoveToPosition(Vector3 position)
        {
            double d = MathHelper.Sqrt((position.X * position.X) + (position.Z * position.Z));
            //double a = 40f;
            //double b = 35f;
            double a = RobotJoints[0].GetLength() + 2;
            double b = RobotJoints[1].GetLength() + 2;
            double phi = MathHelper.RadiansToDegrees(MathHelper.Atan2(position.Z, position.X));
            double beta = MathHelper.RadiansToDegrees(MathHelper.Acos(((a * a) + (d * d) - (b * b)) / (2 * a * d)));
            double theta = MathHelper.RadiansToDegrees(MathHelper.Acos(((a * a) + (b * b) - (d * d)) / (2 * a * b)));

            float j1 = (float)(beta - phi);
            float j2 = (float)(theta - 180);
            float j3 = position.Y - RobotJoints[2].Position.Y;

            if (double.IsNaN(j1) || double.IsNaN(j2))
            {
                SendValues(RobotJoints[0].Distance, RobotJoints[1].Distance, j3);
                return;
            }
            SendValues(j1, j2, j3);
        }

        public void SendValues(float j1, float j2, float j3, float j4 = -1)
        {
            MoveRevoluteJoint(0, j1);
            UpdateModels();

            MoveRevoluteJoint(1, j2);
            UpdateModels();

            MoveLinearJoint(2, j3);
            UpdateModels();

            RobotJoints[0].Distance = j1;
            RobotJoints[1].Distance = j2;
            RobotJoints[2].Distance = j3;

            if (j4 != -1)
            {
                MoveRevoluteJoint(3, j4);
                RobotJoints[3].Distance = j4;
            }
        }

        public void SaveToFile(string filePath)
        {
            SaveToFile();
        }

        public RobotLimb CreateRectangularLimb(string name,  Vector3 position, float distanceToPointB, float paddingX, float sizeY, float sizeZ, float maxMovement)
        {
            RobotLimb limb = new(GLControl, name, Geometry.Cube, position, maxMovement);
            limb.CreateCube(distanceToPointB, paddingX, sizeY, sizeZ);
            return limb;
        }
        //public RobotLimb CreateRectangularLimb(string name, Vector3 position, float sizeX, float sizeY, float sizeZ, float maxMovement)
        //{
        //    RobotLimb limb = new(GLControl, name, Geometry.Cube, position, maxMovement);
        //    limb.CreateCube(sizeX, sizeY, sizeZ);
        //    return limb;
        //}

        public RobotLimb CreateCylindricalLimb(string name, Vector3 position, float radius, float height, float maxMovement)
        {
            RobotLimb limb = new(GLControl, name, Geometry.Cylinder, position, maxMovement);
            limb.CreateCylinder(radius, height);
            return limb;
        }

        public RobotLimb CreateConicalLimb(string name, Vector3 position, float radius, float height, float maxMovement)
        {
            RobotLimb limb = new(GLControl, name, Geometry.Cone, position, maxMovement);
            limb.CreateCone(radius, height);
            return limb;
        }
    }
}
