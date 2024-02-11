using OpenTK.Graphics.Egl;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class SCARA_Robot : Robot
    {

        private Vector3 Position { get; set; }
        //protected Dictionary<int, RobotJoint> RobotJoints { get; set; }
        public List<RobotLimb> RobotJoints = [];
        public List<Matrix4> DenavitHartenbergTable = []; 
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

            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, RobotBase.Center + new Vector3(35f, 0f, 0f)));
            RobotJoints.Add(CreateRevoluteJoint(
                GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, RobotBase.Center + new Vector3(65f, 0f, 0f )));
            RobotJoints.Add(CreateLinearJoint(
                GLControl, "J4", new Vector3(74f, 6.5f, 6f), new Vector3(3.5f, 4.8f, 2.8f), 21f, RobotBase.Center + new Vector3(65f, 0f, 0f)));

            //Placeholders so we know which value is a variable
            float th1, th2, d3, th4;
            th1 = th2 = d3 = th4 = 0f;

            DenavitHartenbergTable.Add(RobotJoints[0].CreateDHMatrix(th1, 0, 35, 0, Vector3.Zero));
            DenavitHartenbergTable.Add(RobotJoints[1].CreateDHMatrix(th2, 0, 35, 0, Vector3.Zero));
            DenavitHartenbergTable.Add(RobotJoints[2].CreateDHMatrix(0, 0, 0, d3, Vector3.Zero));
            DenavitHartenbergTable.Add(RobotJoints[3].CreateDHMatrix(th4, 0, 0, 0, Vector3.Zero));


            //RobotJoints.Add(CreateRevoluteJoint(
            //    GLControl, "J1", new Vector3(9f, 20f, 9f), new Vector3(40f, 6f, 14f), 9f, RobotBase.Center, Vector3.Zero));
            //RobotJoints.Add(CreateRevoluteJoint(
            //    GLControl, "J2", new Vector3(44f, 26f, 6f), new Vector3(35f, 20f, 15f), 30f, RobotJoints[0].RotationCenter, new Vector3(35f, 0f, 0f)));
            //RobotJoints.Add(CreateRevoluteJoint(
            //    GLControl, "J3", new Vector3(74f, 6.5f, 6f), new Vector3(15f, 68.3f, 10f), 25f, RobotJoints[1].RotationCenter, new Vector3(28f, 0f, 0f)));
            //RobotJoints.Add(CreateRevoluteJoint(

            for (int i = 0; i < RobotJoints.Count; i++)
            {
                RobotJoints[i].Cube.SetColor(Color4.OrangeRed);
            }
            //RobotJoints[0].Cube.SetColor(Color4.Red);
            //Cube Manipulator;

        }

        //public void MoveJoint(int jointId, float value)
        //{
        //    //value = MathHelper.Clamp(value, -RobotJoints[jointId].MaximumMovement / 2, RobotJoints[jointId].MaximumMovement / 2);
        //    var joint = RobotJoints[jointId];
        //    // var x = joint.RotationCenter;
        //    //joint.UpdateCenter(value, joint.RotationCenter);

        //    // joint.MoveJoint_Angular(value, joint.RotationCenter, Axis.Y);
        //    //joint.UpdateCenter(value, RobotBase.Center);
        //    //var z = joint.RotationCenter;

        //    if (joint.JointType == RobotLimb.JointTypes.Revolute)
        //    {
        //        var x = joint.Cube.Model;
        //        joint.MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
        //        var z = joint.Cube.Model;
        //        //for (int i = jointId + 1; i < RobotJoints.Count; i++)
        //        for (int i = jointId + 1; i < 2; i++)
        //        {
        //            //RobotJoints[i].UpdateCenter(value, RobotJoints[i - 1].RotationCenter);
        //            //RobotJoints[i].UpdateCenter(value, RobotJoints[i - 1].RotationCenter);
        //            RobotJoints[i].MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
        //            //RobotJoints[i].UpdateCenter(value, RobotJoints[i - 1].RotationCenter);

        //            //Debug.WriteLine($"Distance between base and rotation center {i}: {Vector3.Distance(RobotJoints[0].RotationCenter, RobotJoints[i].RotationCenter)}");
        //        }
        //    }
        //}

        public void MoveJoint(int jointId, float value)
        {
            //value = MathHelper.Clamp(value, -RobotJoints[jointId].MaximumMovement / 2, RobotJoints[jointId].MaximumMovement / 2);
            var joint = RobotJoints[jointId];

            if (joint.JointType == RobotLimb.JointTypes.Revolute)
            {
                joint.MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
                for (int i = jointId + 1; i < 2; i++)
                {
                    RobotJoints[i].MoveJoint_Angular(value, joint.RotationCenter, joint.Axis);
                }
            }
        }

        public void MoveRobot()
        {
            //Matrix4 rotation = Matrix4.CreateTranslation(-17, -10, -17);
            Matrix4 rotation = Matrix4.Identity;
            Matrix4 J1, J2, J3, J4;
            J1 = J2 = J3 = J4 = Matrix4.Identity;
            //for (int i = 0; i < DenavitHartenbergTable.Count; i++)
            //{
            //    rotation *= DenavitHartenbergTable[i];
            //    //RobotJoints[i].Cube.SetTransformation(rotation * Matrix4.CreateTranslation(17, 10, 17));
            //    //RobotJoints[i].SetRotationCenter(rotation * Matrix4.CreateTranslation(17, 10, 17));
            //    RobotJoints[i].SetRotationCenter(rotation * Matrix4.CreateTranslation(52, 10, 17));
            //}

            J1 = DenavitHartenbergTable[0] * Matrix4.CreateTranslation(17, 10, 17);
            J2 = DenavitHartenbergTable[0] * DenavitHartenbergTable[1] * Matrix4.CreateTranslation(17, 10, 17);
            J3 = DenavitHartenbergTable[0] * DenavitHartenbergTable[1] * DenavitHartenbergTable[2] * Matrix4.CreateTranslation(17, 10, 17);

            //RobotJoints[0].SetRotationCenter(J1);
            List<Vector3> centers = new List<Vector3>();
            foreach(RobotLimb limb in RobotJoints)
            {
                centers.Add(limb.RotationCenter);
            }
            var z = centers;
        }

        public void UpdateJointValues(float th1, float th2, float d3, float th4)
        {
            DenavitHartenbergTable[0] = RobotJoints[0].CreateDHMatrix(90, 0f, 0f, 0f, new Vector3(17,10,17));
            DenavitHartenbergTable[1] = RobotJoints[1].CreateDHMatrix(th2, 0f, 35f, 0f, new Vector3(RobotJoints[0].DHMatrix.M41, RobotJoints[0].DHMatrix.M42, RobotJoints[0].DHMatrix.M43));
            DenavitHartenbergTable[2] = RobotJoints[2].CreateDHMatrix(0f, 0f, 30f, d3, new Vector3(RobotJoints[1].DHMatrix.M41, RobotJoints[1].DHMatrix.M42, RobotJoints[1].DHMatrix.M43));
            DenavitHartenbergTable[3] = RobotJoints[3].CreateDHMatrix(th4, 0f, 0f, 0f, new Vector3(RobotJoints[2].DHMatrix.M41, RobotJoints[2].DHMatrix.M42, RobotJoints[2].DHMatrix.M43));

            var x = DenavitHartenbergTable;
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
