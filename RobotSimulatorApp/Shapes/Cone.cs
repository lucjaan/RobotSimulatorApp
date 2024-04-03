using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Collections.Generic;

namespace RobotSimulatorApp.Shapes
{
    public class Cone : Shape
    {
        #region Fields
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 ApexPoint { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Transformation { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }
        private int Sides { get; set; }
        private Matrix4 Apex { get; set; }
        private Matrix4 ApexBuffer { get; set; }

        private readonly GLControl GlControl;
        private ShapeArrays BaseArrays = new();
        private ShapeArrays SideArrays = new();
        #endregion

        public Cone(GLControl glControl, Vector3 position, float radius, float height)
        {
            Position = Center = position;
            GlControl = glControl;
            Radius = radius;
            Height = height;
            Sides = 90;
            Transformation = Matrix4.Identity;
            Model = Matrix4.CreateTranslation(position);

            Apex = ApexBuffer = Matrix4.CreateTranslation(new Vector3(0, height, 0));
            ApexPoint = Helpers.GetPositionFromMatrix(Apex);
            Apex = ApexBuffer *= Matrix4.CreateTranslation(position);

            BaseArrays.IndexData = GenerateBaseIndices().ToArray();
            BaseArrays.Vertices = CreateRoundBase(position.Y);

            List<Vector3> sides = [ApexPoint, .. BaseArrays.Vertices];
            SideArrays.IndexData = GenerateSideIndices().ToArray();
            SideArrays.Vertices = sides.ToArray();

        }

        public void RenderCone(Matrix4 view, Matrix4 projection)
        {
            Render(Model, Transformation, view, projection, BaseArrays);
            Render(Model, Transformation, view, projection, SideArrays);
            Apex = ApexBuffer * Transformation;
        }

        public override void UpdateBaseModel()
        {
            Model *= Transformation;
            ApexBuffer *= Transformation;
            Transformation = Matrix4.Identity;
        }

        public Vector3 GetApexPosition() => ApexPoint = Helpers.GetPositionFromMatrix(Apex);
        public Vector3 GetCenterPoint() => Center = Helpers.GetPositionFromMatrix(Model);

        public override void SetColor(Color4 colorData)
        {
            List <Color4> bases = [];
            List <Color4> sides = [];
            for (int i = 0; i < BaseArrays.IndexData.Length; i++)
            {
                bases.Add(new Color4(
                    MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                    1));
            }

            for (int i = 0; i < SideArrays.IndexData.Length; i++)
            {
                sides.Add(colorData);
            }

            BaseArrays.ColorsData = bases.ToArray();
            SideArrays.ColorsData = bases.ToArray();
        }

        private Vector3[] CreateRoundBase(float level)
        {
            float angle = MathHelper.DegreesToRadians(360 / Sides);
            Vector3[] result = new Vector3[Sides + 1];
            result[0] = new(0, level, 0);
            for (int i = 0; i < Sides; i++)
            {
                Vector2 point = new((float)MathHelper.Cos(angle * i) * Radius, (float)MathHelper.Sin(angle * i) * Radius);
                result[i + 1] = new(point.X, level, point.Y);
            }
            return result;
        }

        private List<int> GenerateBaseIndices()
        {
            List<int> result = [];
            for (int i = 1; i < Sides; i++)
            {
                result.Add(0);
                result.Add(i);
                result.Add(i + 1);
            }
            result.Add(0);
            result.Add(Sides);
            result.Add(1);
            return result;
        }

        private List<int> GenerateSideIndices()
        {
            List<int> result = [];
            for (int i = 2; i <= Sides; i++)
            {
                result.Add(0);
                result.Add(i);
                result.Add(i + 1);
            }
            result.Add(0);
            result.Add(2);
            result.Add(Sides + 1);
            return result;
        }
    }
}
