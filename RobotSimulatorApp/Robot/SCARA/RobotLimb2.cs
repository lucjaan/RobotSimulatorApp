using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;

namespace RobotSimulatorApp.Robot.SCARA
{
    public class RobotLimb2
    {
        public enum JointTypes
        {
            Linear,
            Revolute
        }

        public Vector3 RotationCenter { get; set; }
        public Matrix4 DHMatrix { get; set; }
        /// <summary>
        /// Distance between previous and current joint center, used to keep the model rigid after rotation.
        /// </summary>
        public static Vector3 Delta { get; set; }
        public string Name { get; set; }
        public Cube Cube { get; set; }
        public Geometry Geometry { get; set; }
        public Axis Axis { get; set; }
        /// <summary>
        /// In linear joints it's the distance in one of the axes, in revolute joints it's maximum angle from starting postition
        /// </summary>
        public float MaximumDistance { get; set; }
        public float Distance { get ; set; }
        public JointTypes JointType { get; set; }
        public RobotLimb2(Cube cube, string name, float maximumMovement, JointTypes type, Vector3 rotationCenter)
        {
            Name = name;
            Cube = cube;
            //Geometry;
            MaximumDistance = maximumMovement;
            JointType = type;
            RotationCenter = rotationCenter;
            Geometry = Geometry.Cube;
            
            if (type == JointTypes.Revolute)
            {
                Axis = Axis.Y;
            }   
        }

        /// <summary>
        /// When we move one revolute joint, all joints higher on kinematic chain get rotated in relation to it's axis
        /// </summary>
        public void MoveJoint_Angular(float angle, Vector3 centerOfRotation, Axis axis)
        {
            angle = MathHelper.DegreesToRadians(angle);
            Cube.Transformation = Helpers.CreateRotationYAroundPoint(angle, centerOfRotation);
        }

        /// <summary>
        /// When we move one linear joint, all joints higher on kinematic chain get transposed through the same values
        /// </summary>
        public void MoveJoint_Linear(Vector3 translationVector)
        {
            //TODO
        }

        public void SetColor(Color4 color)
        {
            switch (Geometry)
            {
                case Geometry.Cube:
                    Cube.SetColor(color);
                    break;
            }
        }
        public Matrix4 CreateDHMatrix(float theta, float alpha, float a, float d, Vector3 prevCenter)
        {

            Matrix4 result = Matrix4.CreateTranslation(new Vector3(0, 0, d)) * Matrix4.CreateRotationY(theta) * Matrix4.CreateTranslation(new Vector3(a, 0, 0)) * Matrix4.CreateRotationX(alpha);
            DHMatrix = result;
            return result;
        }
    }
}
