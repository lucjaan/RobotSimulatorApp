using OpenTK.Mathematics;
using System.Collections.Generic;

namespace RobotSimulatorApp.Shapes
{
    public class Cube : Shape
    {
        #region Fields
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Matrix4 Model { get; set; }
        public float Length { get; set; }
        public Matrix4 Transformation { get; set; }
        public Matrix4 CenterPoint { get; set; }
        public Matrix4 RotationCenter { get; set; }
        public Matrix4 StartPoint { get; set; }
        private Matrix4 CenterBuffer { get; set; }
        private Matrix4 RotationBuffer { get; set; }
        private Matrix4 StartBuffer { get; set; }

        public ShapeArrays Arrays = new();
        public ShapeArrays BorderArrays = new();
        private readonly List<int> _indexData = new()
        {
             0,
            1,
            2,
            2,
            3,
            0,
            4,
            5,
            6,
            6,
            7,
            4,
            8,
            9,
            10,
            10,
            11,
            8,
            12,
            13,
            14,
            14,
            15,
            12,
            16,
            17,
            18,
            18,
            19,
            16,
            20,
            21,
            22,
            22,
            23,
            20
        };
        #endregion

        /// <summary>
        /// Creates Cube from center, where sizeX/Y/Z is total size in given axis
        /// </summary>
        public Cube(Vector3 position, float sizeX, float sizeY, float sizeZ)
        {
            Position = position;
            Center = new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2);
            Transformation = Matrix4.Identity;

            Model = Matrix4.CreateTranslation(position);
            CenterPoint = CenterBuffer = Matrix4.CreateTranslation(Center) * Matrix4.CreateTranslation(position);
            RotationCenter = RotationBuffer = Matrix4.CreateTranslation(new Vector3(0, sizeY, 0));
            Length = 0;
            Arrays.Vertices = CreateVertices(sizeX, sizeY, sizeZ).ToArray();
            Arrays.IndexData = _indexData.ToArray();
            CreateBorder(sizeX, sizeY, sizeZ);

            SetColor(Color4.DarkOrange);
        }

        /// <summary>
        /// Creates cube between point A and point B:(A.X + distanceToEndPoint, A.Y, A.Z), and where paddingX is distance between points A,B and
        /// borders of the model in X axis and sizeY/Z are sizes in Y/Z axis
        /// </summary>
        public Cube(Vector3 startPoint, float distanceToEndPoint, float paddingX, float sizeY, float sizeZ)
        {
            Position = new Vector3(startPoint.X + distanceToEndPoint / 2, startPoint.Y, startPoint.Z);
            float sizeX = distanceToEndPoint + 2 * paddingX;
            Center = new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2);
            Model = Matrix4.CreateTranslation(Position);

            StartPoint = StartBuffer = Matrix4.CreateTranslation(startPoint);
            RotationCenter = RotationBuffer = Matrix4.CreateTranslation(new Vector3(startPoint.X + distanceToEndPoint, startPoint.Y + sizeY, startPoint.Z));
            CenterPoint = CenterBuffer = Matrix4.CreateTranslation(Center) * Matrix4.CreateTranslation(Position);
            Transformation = Matrix4.Identity;
            Length = distanceToEndPoint;

            Arrays.Vertices = CreateVertices(sizeX, sizeY, sizeZ).ToArray();
            Arrays.IndexData = _indexData.ToArray();
            SetColor(Color4.DarkOrange);
            CreateBorder(sizeX, sizeY, sizeZ);
        }

        public void RenderCube(Matrix4 view, Matrix4 projection, bool borderShown = false)
        {
            Render(Model, Transformation, view, projection, Arrays);
            RotationCenter = RotationBuffer * Transformation;
            CenterPoint = CenterBuffer * Transformation;
            if (borderShown)
            {
                RenderBorder(Model, Transformation, view, projection, BorderArrays);
            }
        }

        public void SetRotationCenter(Vector3 point) => RotationBuffer = RotationCenter = Matrix4.CreateTranslation(point) * Matrix4.CreateTranslation(Position);
        public Vector3 GetRotationCenter() => Helpers.GetPositionFromMatrix(RotationBuffer * Transformation);
        public void SetPosition(Vector3 position) => Model = Matrix4.CreateTranslation(position);

        public override void UpdateBaseModel()
        {
            Model *= Transformation;
            RotationBuffer *= Transformation;
            CenterBuffer *= Transformation;
            StartBuffer *= Transformation;
            Transformation = Matrix4.Identity;
        }

        public override void SetColor(Color4 colorData)
        {
            Color4[] color = new Color4[24];

            for (int i = 0; i < 4; i++)
            {
                color[i + 16] = new Color4(
                    MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                    1);

                color[i] = color[i + 8] = color[i + 12] = color[i + 20] = colorData;

                color[i + 4] = new Color4(
                    MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                    1);
            }
            Arrays.ColorsData = color;
        }

        private List<Vector3> CreateVertices(float sizeX, float sizeY, float sizeZ)
        {
            List<Vector3> vertices = new List<Vector3>();
            float x = sizeX / 2;
            float y = sizeY / 2;
            float z = sizeZ / 2;
            vertices.AddRange(CreateWall(x, y, -z, Axis.Z));
            vertices.AddRange(CreateWall(x, -y, z, Axis.Y));
            vertices.AddRange(CreateWall(-x, y, z, Axis.X));
            vertices.AddRange(CreateWall(x, y, z, Axis.Z));
            vertices.AddRange(CreateWall(x, y, z, Axis.Y));
            vertices.AddRange(CreateWall(x, y, z, Axis.X));
            return vertices;
        }

        private List<Vector3> CreateWall(float x, float y, float z, Axis axis)
        {
            List<Vector3> result  = new();
            float absY = MathHelper.Abs(y);
            switch (axis)
            {
                case Axis.X:
                    foreach (Vector2 v in CreateWallRectangle(y, z))
                    {
                        result.Add(new Vector3(x, v.X + absY, v.Y));
                    }
                    break;

                case Axis.Y:
                    foreach (Vector2 v in CreateWallRectangle(x, z))
                    {
                        result.Add(new Vector3(v.X, y + absY, v.Y));
                    }
                    break;

                case Axis.Z:
                    foreach (Vector2 v in CreateWallRectangle(x, y))
                    {
                        result.Add(new Vector3(v.X, v.Y + absY, z));
                    }
                    break;
            }
            return result;
        }

        private static Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(-a, -b), new(a, -b), new(a, b), new(-a, b) };

        private ShapeArrays CreateBorder(float sizeX, float sizeY, float sizeZ)
        {
            BorderArrays.Vertices = CreateBorderVertices(sizeX, sizeY, sizeZ).ToArray();
            BorderArrays.IndexData = CreateBorderIndices().ToArray();
            SetBorderColor(Color4.Black);
            return BorderArrays;
        }

        private List<Vector3> CreateBorderVertices(float sizeX, float sizeY, float sizeZ)
        {
            List<Vector3> vert1  = new();
            List<Vector3> vert2  = new();

            foreach (Vector2 v in CreateWallRectangle(sizeX / 2, sizeZ / 2))
            {
                vert1.Add(new Vector3(v.X, 0, v.Y));
                vert2.Add(new Vector3(v.X, sizeY, v.Y));
            }

            vert1.AddRange(vert2);
            return vert1;
        }

        private List<int> CreateBorderIndices()
        {
            List<int> indices = new() { 0, 1, 0, 4, 1, 2, 1, 5, 2, 3, 2, 6, 3, 7, 0, 3, 4, 5, 5, 6, 6, 7, 4, 7 };
            return indices;
        }

        public void SetBorderColor(Color4 colorData)
        {
            List<Color4> color  = new();
            for (int i = 0; i < BorderArrays.IndexData.Length; i++)
            {
                color.Add(colorData);
            }
            BorderArrays.ColorsData = color.ToArray();
        }
    }
}
