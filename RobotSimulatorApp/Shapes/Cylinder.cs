using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Collections.Generic;
using System.Linq;

namespace RobotSimulatorApp.Shapes
{
    public class Cylinder : Shape
    {
        #region Fields
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Transformation { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }
        private int Sides { get; set; }

        private readonly GLControl GlControl;

        private ShapeArrays TopArrays;
        private ShapeArrays BottomArrays;
        private ShapeArrays SideArrays;
        private ShapeArrays BorderArrays;
        #endregion
        public Cylinder(GLControl glControl, Vector3 position, float radius, float height)
        {
            Position = Center = position;
            GlControl = glControl;
            Radius = radius;
            Height = height;
            Sides = 90;
            Transformation = Matrix4.Identity;
            Model = Matrix4.CreateTranslation(position);

            BottomArrays.Vertices = CreateRoundBase(position.Y);
            BottomArrays.IndexData = GenerateBaseIndices().ToArray();

            TopArrays.Vertices = CreateRoundBase(position.Y + height);
            TopArrays.IndexData = GenerateBaseIndices().ToArray();

            SideArrays.Vertices = BottomArrays.Vertices.Concat(TopArrays.Vertices).ToArray();
            SideArrays.IndexData = GenerateSideIndices().ToArray();

            SetColor(Color4.Green);

            CreateBorder();
        }

        public void RenderCylinder(Matrix4 view, Matrix4 projection, bool borderShown = false)
        {
            Render(Model, Transformation, view, projection, TopArrays);
            Render(Model, Transformation, view, projection, BottomArrays);
            Render(Model, Transformation, view, projection, SideArrays);
            if (borderShown)
            {
                RenderBorder(Model, Transformation, view, projection, BorderArrays);
            }
        }

        public override void UpdateBaseModel()
        {
            Model *= Transformation;
            Transformation = Matrix4.Identity;
        }

        public Vector3 GetCenterPoint() => Center = Helpers.GetPositionFromMatrix(Model);
        public override void SetColor(Color4 colorData)
        {
            List<Color4> top = [];
            List<Color4> bottom = [];
            List<Color4> sides = [];
            for (int i = 0; i < SideArrays.IndexData.Length; i++)
            {
                sides.Add(colorData);
            }

            for (int i = 0; i < TopArrays.IndexData.Length; i++)
            {
                top.Add(new Color4(
                       MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                       MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                       MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                       1));
            }

            for (int i = 0; i < BottomArrays.IndexData.Length; i++)
            {
                bottom.Add(new Color4(
                MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                1));
            }
            TopArrays.ColorsData = top.ToArray();
            BottomArrays.ColorsData = bottom.ToArray();
            SideArrays.ColorsData = sides.ToArray();
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
            List<int> result = new();
            int c = TopArrays.Vertices.Length;
            for (int i = 1; i < Sides; i++)
            {
                result.Add(i);
                result.Add(i + 1);
                result.Add(i + c);
                result.Add(i + c);
                result.Add(i + c + 1);
                result.Add(i + 1);
            }

            result.Add(1);
            result.Add(c - 1);
            result.Add(c + 1);
            result.Add(c + 1);
            result.Add(c + Sides);
            result.Add(c - 1);
            return result;
        }

        private ShapeArrays CreateBorder()
        {
            List<Vector3> bases = CreateBorderVertices(Position.Y).ToList();
            bases.AddRange(CreateBorderVertices(Position.Y + Height));
            BorderArrays.Vertices = bases.ToArray();

            List<int> indices = [];
            indices.AddRange(GenerateBorderBaseIndices());
            indices.AddRange(GenerateBorderBaseIndices(Sides));
            indices.AddRange(CreateSideIndices());
            BorderArrays.IndexData = indices.ToArray();

            SetBorderColor(Color4.Black);
            return BorderArrays;
        }

        private Vector3[] CreateBorderVertices(float level)
        {
            float angle = MathHelper.DegreesToRadians(360 / Sides);
            Vector3[] result = new Vector3[Sides];
            for (int i = 0; i < Sides; i++)
            {
                Vector2 point = new((float)MathHelper.Cos(angle * i) * Radius, (float)MathHelper.Sin(angle * i) * Radius);
                result[i] = new(point.X, level, point.Y);
            }
            return result;
        }

        private List<int> GenerateBorderBaseIndices(int step = 0)
        {
            List<int> result = [];
            for (int i = 0; i < Sides - 1; i++)
            {
                result.Add(i + step);
                result.Add(i + step +  1);
            }
            result.Add(step + Sides - 1);
            result.Add(step);
            return result;
        }

        private List<int> CreateSideIndices(int lines = 10) 
        {
            List<int> result = [];
            int step = Sides / lines;
            for (int i = 0; i < lines; i++)
            {
                result.Add(i * step);
                result.Add((i * step) + Sides);
            }
            return result;
        }

        public void SetBorderColor(Color4 colorData)
        {
            List<Color4> color = [];
            for (int i = 0; i < BorderArrays.IndexData.Length; i++)
            {
                color.Add(colorData);
            }
            BorderArrays.ColorsData = color.ToArray();
        }
    }
}
