using OpenTK.Mathematics;

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

        public static Vector3 GetPositionFromMatrix(Matrix4 matrix) => new(matrix.M41, matrix.M42, matrix.M43);
    }
}
