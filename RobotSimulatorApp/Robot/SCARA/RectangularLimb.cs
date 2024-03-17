using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotSimulatorApp.Robot.SCARA.RobotLimb2;
using System.Xml.Linq;
using OpenTK.Mathematics;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RectangularLimb : RobotLimb
    {

        Cube Model { get; set; }
        public RectangularLimb(Cube cube, string name, float maximumMovement, Vector3 rotationCenter)
        {
            Name = name;
            Model = cube;
            //Geometry;
            MaximumDistance = maximumMovement;
            RotationCenter = rotationCenter;
        }

        public override void Move(float angle, Vector3 centerOfRotation)
        {
            //angle = MathHelper.DegreesToRadians(angle);
            Model.Transformation = Helpers.CreateRotationYAroundPoint(MathHelper.DegreesToRadians(angle), centerOfRotation);
        }

        public override void SetColor(Color4 color)
        {
            Model.SetColor(color);
        }
    }
}
