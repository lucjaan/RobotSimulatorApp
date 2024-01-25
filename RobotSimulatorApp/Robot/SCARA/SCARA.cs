using OpenTK.Graphics.Egl;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    internal class SCARA_Robot : Robot
    {

        private Vector3 Position {  get; set; }
        //protected Dictionary<int, RobotJoint> RobotJoints { get; set; }
        protected List<RobotLimb> RobotJoints = [];
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
            //RobotBase = new(GLControl, Vector3.Zero, new Vector3(34f, 20f, 34f));
            RobotBase.SetColor(Color4.DarkOrange);
            //RobotBase.SetColor();

            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, new Vector3(RobotBase.Center.X + 35f, RobotBase.Center.Y, RobotBase.Center.Z)));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, new Vector3(RobotBase.Center.X + 65f, RobotBase.Center.Y, RobotBase.Center.Z)));
            RobotJoints.Add(CreateLinearJoint(
                GLControl, "J4", new Vector3(74f, 6.5f, 6f), new Vector3(3.5f, 4.8f, 2.8f), 21f, new Vector3(RobotBase.Center.X + 65f, RobotBase.Center.Y, RobotBase.Center.Z)));
            //Manipulator = new(GLControl, new Vector3(74f, 6.5f, 6f), 2.7f, 4.8f, 4.8f,
            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].Cube.SetColor(Color4.OrangeRed);
            }
            //RobotJoints[0].Cube.SetColor(Color4.Red);
            //Cube Manipulator;

        }

        public void Rotate(int jointId, float angle)
        {
            angle = MathHelper.Clamp(angle, -RobotJoints[jointId].MaximumMovement / 2, RobotJoints[jointId].MaximumMovement / 2);
            
        }

        public void RenderRobot(Matrix4 view, Matrix4 projection)
        {
            RobotBase.RenderCube(view, projection);
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


        public override void MoveJoint(int jointId)
        {
            //
        }
    }
}
