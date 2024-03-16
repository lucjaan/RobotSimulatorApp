using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot
{
    public abstract class Robot
    {
        protected string Name { get; set; }
        public RobotTypes RobotType { get; set; }
        //protected Dictionary<int, Cube> RobotJoints { get; set; }
        //protected Dictionary<int, Vector3> JointRotationPoints { get; set; }
        protected Robot() { }

        public abstract void CreateKinematicChain();
        //public abstract void CreateJoint();
        //public abstract void MoveJoint(int jointId);
        public void SaveToFile()
        {
            //TODO
        }

        public void LoadFromFile()
        {
            //TODO
        }

    }
}
