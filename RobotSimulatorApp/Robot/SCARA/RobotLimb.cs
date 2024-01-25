using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
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

        public Vector3 MovementPoint { get; set;}       
        public string Name { get; set;}
        public Cube Cube { get; set; }
        /// <summary>
        /// In linear joints it's the distance in one of the axes, in revolute joints it's maximum angle from starting postition
        /// </summary>
        public float MaximumMovement { get; set;} 
        private JointTypes JointType { get; set;}
        public RobotLimb(Cube cube, string name, float maximumMovement, JointTypes type, Vector3 movementPoint)
        {
            Name = name;
            Cube = cube;
            MaximumMovement = maximumMovement;
            JointType = type;
            MovementPoint = movementPoint;
        }

        
    //Relative to the model
    //float X;
    //float Y;
    //float Z;



    }
}
