using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.Robot.SCARA
{
    public abstract class RobotLimb
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 RotationCenter { get; set; }
        public float Distance { get; set; }
        public float MaximumDistance { get; set; }
        public abstract void Move(float angle, Vector3 centerOfRotation);
        public abstract void SetColor(Color4 color);
    }
}
