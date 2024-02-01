using OpenTK.Graphics.Egl;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {

        private Vector3 Position { get; set; }
        public List<RobotLimb> RobotJoints = [];
        protected Dictionary<int, Vector3> JointCenters { get; set; }
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

            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, RobotBase.Center * Matrix4.CreateTranslation(new Vector3(35f, 0f, 0f))));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, RobotBase.Center * Matrix4.CreateTranslation(new Vector3(65f, 0f, 0f ))));
            RobotJoints.Add(CreateLinearJoint(
                GLControl, "J4", new Vector3(74f, 6.5f, 6f), new Vector3(3.5f, 4.8f, 2.8f), 21f, RobotBase.Center * Matrix4.CreateTranslation(new Vector3(65f, 0f, 0f))));

            RobotBase.SetColor(Color4.DarkOrange);
            RobotJoints[0].Cube.SetColor(Color4.OrangeRed);
            RobotJoints[1].Cube.SetColor(Color4.OrangeRed);
            RobotJoints[2].Cube.SetColor(Color4.DarkGray);
            RobotJoints[3].Cube.SetColor(Color4.CadetBlue);

        }

        public void MoveJoint(int jointId, float value)
        {
            var joint = RobotJoints[jointId];

            if (joint.JointType == RobotLimb.JointTypes.Revolute)
            {
                joint.MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
                for (int i = jointId + 1; i < RobotJoints.Count; i++)
                //for (int i = jointId + 1; i < 2; i++)
                {
                    //RobotJoints[i].UpdateCenter(value, RobotJoints[i - 1].RotationCenter);
                    RobotJoints[i].MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
                }
            }
        }

        public void UpdateModels()
        {
            foreach(RobotLimb joint in RobotJoints)
            {

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

        public RobotLimb CreateRevoluteJoint(GLControl glc, string name, Vector3 position, Vector3 size, float maximumDistance, Matrix4 movementPoint)
            => new(new Cube(glc, position, size), name, maximumDistance, RobotLimb.JointTypes.Revolute, movementPoint);

        public RobotLimb CreateLinearJoint(GLControl glc, string name, Vector3 position, Vector3 size, float maximumAngle, Matrix4 rotationCenter)
           => new(new Cube(glc, position, size), name, maximumAngle, RobotLimb.JointTypes.Linear, rotationCenter);
    }
}
