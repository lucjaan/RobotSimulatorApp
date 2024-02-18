using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp
{
    public class Helpers
    {
        public static Matrix4 CreateRotationXAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(-centerVector) * Matrix4.CreateRotationX(angle) * Matrix4.CreateTranslation(centerVector);

        public static Matrix4 CreateRotationYAroundPoint(float angle, Vector3 centerVector)
             => Matrix4.CreateTranslation(-centerVector) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(centerVector);

        public static Matrix4 CreateRotationZAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(-centerVector) * Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(centerVector);
    }
}
