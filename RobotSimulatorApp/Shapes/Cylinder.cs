using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Collections.Generic;
using System.Linq;

namespace RobotSimulatorApp.Shapes
{
    public class Cylinder : Shape
    {
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Transformation { get; set; }
        public float Radius { get; set; }
        private int Sides { get; set; }

        private readonly GLControl GlControl;

        private readonly List<Vector3> TopVertices = [];
        private readonly List<Vector3> BottomVertices = [];
        private readonly List<int> BaseIndexData = [];
        private readonly List<int> SidesIndexData = [];
        private readonly List<Vector3> SidesVertices = [];

        private readonly List<Color4> SideColorData = [];
        private readonly List<Color4> TopColorData = [];
        private readonly List<Color4> BottomColorData = [];

        public Cylinder(GLControl glControl, Vector3 position, float radius, float height)
        {
            Position = Center = position;
            GlControl = glControl;
            Radius = radius;
            Sides = 90;
            Transformation = Matrix4.Identity;
            Model = Matrix4.CreateTranslation(position);

            BottomVertices = CreateRoundBase(position.Y).ToList();
            TopVertices = CreateRoundBase(position.Y + height).ToList();

            SidesVertices.AddRange(BottomVertices);
            SidesVertices.AddRange(TopVertices);
            GenerateBaseIndices();
            GenerateSideIndices();

            SetColor(Color4.Green);
        }

        public void RenderCylinder(Matrix4 view, Matrix4 projection)
        {
            Render(Model, Transformation, view, projection, BaseIndexData.ToArray(), TopVertices.ToArray(), TopColorData.ToArray());
            Render(Model, Transformation, view, projection, BaseIndexData.ToArray(), BottomVertices.ToArray(), BottomColorData.ToArray());
            Render(Model, Transformation, view, projection, SidesIndexData.ToArray(), SidesVertices.ToArray(), SideColorData.ToArray());
        }

        public override void UpdateBaseModel()
        {
            Model *= Transformation;
            Transformation = Matrix4.Identity;
        }

        public Vector3 GetCenterPoint() => Center = Helpers.GetPositionFromMatrix(Model);
        public override void SetColor(Color4 colorData)
        {
            if (TopColorData != null || BottomColorData != null || SideColorData != null)
            {
                TopColorData.Clear();
                BottomColorData.Clear();
                SideColorData.Clear();
            }

            for (int i = 0; i < SidesIndexData.Count; i++)
            {
                SideColorData.Add(colorData);
            }

            for (int i = 0; i < BaseIndexData.Count; i++)
            {
                //TopColorData.Add(new Color4(
                //       MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                //       MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                //       MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                //       1));

                if (i % 2 == 0)
                {
                    TopColorData.Add(new Color4(
                    MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                    1));
                }
                else
                {
                    TopColorData.Add(Color4.OrangeRed);
                }
            }

            for (int i = 0; i < BaseIndexData.Count; i++)
            {
                BottomColorData.Add(new Color4(
                MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                1));
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
            int c = TopVertices.Count;
            for (int i = 1; i < Sides; i++)
            {
                SidesIndexData.Add(i);
                SidesIndexData.Add(i + 1);
                SidesIndexData.Add(i + c);
                SidesIndexData.Add(i + c);
                SidesIndexData.Add(i + c + 1);
                SidesIndexData.Add(i + 1);
            }

            SidesIndexData.Add(1);
            SidesIndexData.Add(c - 1);
            SidesIndexData.Add(c + 1);
            SidesIndexData.Add(c + 1);
            SidesIndexData.Add(c + Sides);
            SidesIndexData.Add(c - 1);
        }
    }
}
