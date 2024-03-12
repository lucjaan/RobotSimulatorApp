using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System.Collections.Generic;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {

        private Vector3 Position { get; set; }
        public List<RobotLimb> RobotJoints = [];
        public List<Matrix4> DenavitHartenbergTable = new List<Matrix4>(4);
        /// <summary>
        /// X = theta, Y = a, Z = d, W = alpha
        /// </summary>
        public List<Vector4> DHParameters = [];
        public List<Vector3> JointCenters = [];
        private GLControl GLControl { get; set; }
        public Cube RobotBase;

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

            RobotBase = new(GLControl, Vector3.Zero, new Vector3(34f, 20f, 34f));
            RobotBase.SetColor(Color4.DarkOrange);

            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, RobotBase.Center + new Vector3(35f, 0f, 0f)));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, RobotBase.Center + new Vector3(65f, 0f, 0f)));
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

                for(int i = 0; i < RobotJoints.Count; i++)
                {
                    JointCenters[i + 1] = Helpers.GetPositionFromMatrix(RobotJoints[i].Cube.Point);
                }

                for (int i = jointId; i < RobotJoints.Count; i++)
                {
                    RobotJoints[i].MoveJoint_Angular(value - RobotJoints[jointId].Distance, JointCenters[jointId], joint.Axis);
                }
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
            Vector3 j1 = new(40f, RobotJoints[0].Cube.Size.Y / 2, RobotJoints[0].Cube.Size.Z / 2);
            Vector3 j2 = new(35f, RobotJoints[1].Cube.Size.Y / 2, RobotJoints[1].Cube.Size.Z / 2);
            Vector3 j3 = new(RobotJoints[2].Cube.Size.X / 2, RobotJoints[2].Cube.Size.Y / 2, RobotJoints[2].Cube.Size.Z / 2);
            Vector3 j4 = new(RobotJoints[3].Cube.Size.X / 2, RobotJoints[3].Cube.Size.Y / 2, RobotJoints[3].Cube.Size.Z / 2);

            RobotJoints[0].Cube.SetPoint(j1);
            RobotJoints[1].Cube.SetPoint(j2);
            RobotJoints[2].Cube.SetPoint(j3);
            RobotJoints[3].Cube.SetPoint(j4);

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
                RobotJoints[i].Cube.UpdateBaseModel();
            }
        }

        public void RenderRobot(Matrix4 view, Matrix4 projection)
        {
            RobotBase.RenderCube(view, projection);

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
            => new(new Cube(glc, position, size), name, maximumDistance, RobotLimb.JointTypes.Revolute, movementPoint);

        public RobotLimb CreateLinearJoint(GLControl glc, string name, Vector3 position, Vector3 size, float maximumAngle, Vector3 rotationCenter)
           => new(new Cube(glc, position, size), name, maximumAngle, RobotLimb.JointTypes.Linear, rotationCenter);

    }
}
