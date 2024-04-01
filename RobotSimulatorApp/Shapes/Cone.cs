using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System.Collections.Generic;
using System.Linq;

namespace RobotSimulatorApp.Shapes
{
    public class Cone : Shape
    {
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

        private readonly List<Vector3> BaseVertices = [];
        private readonly List<Vector3> SidesVertices = [];

        private readonly List<int> BaseIndexData = [];
        private readonly List<int> SidesIndexData = [];

        private readonly List<Color4> SideColorData = [];
        private readonly List<Color4> BaseColorData = [];

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

            BaseVertices = CreateRoundBase(position.Y).ToList();
            SidesVertices.Add(ApexPoint);
            SidesVertices.AddRange(BaseVertices);
            GenerateBaseIndices();
            GenerateSideIndices();

            Apex = ApexBuffer *= Matrix4.CreateTranslation(position);
        }

        public void RenderCone(Matrix4 view, Matrix4 projection)
        {
            Render(Model, Transformation, view, projection, BaseIndexData.ToArray(), BaseVertices.ToArray(), BaseColorData.ToArray());
            Render(Model, Transformation, view, projection, SidesIndexData.ToArray(), SidesVertices.ToArray(), SideColorData.ToArray());
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
            for (int i = 0; i < BaseIndexData.Count; i++)
            {
                BaseColorData.Add(new Color4(
                    MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                    1));
            }

            for (int i = 0; i < SidesIndexData.Count; i++)
            {
                SideColorData.Add(colorData);
            }
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

        private void GenerateBaseIndices()
        {
            for (int i = 1; i < Sides; i++)
            {
                BaseIndexData.Add(0);
                BaseIndexData.Add(i);
                BaseIndexData.Add(i + 1);
            }
            BaseIndexData.Add(0);
            BaseIndexData.Add(Sides);
            BaseIndexData.Add(1);
        }

        private void GenerateSideIndices()
        {
            for (int i = 2; i <= Sides; i++)
            {
                SidesIndexData.Add(0);
                SidesIndexData.Add(i);
                SidesIndexData.Add(i + 1);
            }
            SidesIndexData.Add(0);
            SidesIndexData.Add(2);
            SidesIndexData.Add(Sides + 1);
        }
    }
}
