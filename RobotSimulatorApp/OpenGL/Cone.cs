using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System.Collections.Generic;
using System.Linq;

namespace RobotSimulatorApp.OpenGL
{
    public class Cone
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
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        private readonly List<Vector3> BaseVertices = [];
        private readonly List<Vector3> SidesVertices = [];

        private readonly List<int> BaseIndexData = [];
        private readonly List<int> SidesIndexData = [];

        private readonly List<Color4> SideColorData = [];
        private readonly List<Color4> BaseColorData = [];

        public static readonly string VertexShader =
   @"#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 aColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
out vec4 fColor;

void main(void)
{

    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    fColor = aColor;

}";

        public static readonly string FragmentShader =
           @"#version 330 core
in vec4 fColor;

out vec4 oColor;

void main()
{
    oColor = fColor;
}";

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
            Render(view, projection, BaseIndexData.ToArray(), BaseVertices.ToArray(), BaseColorData.ToArray());
            Render(view, projection, SidesIndexData.ToArray(), SidesVertices.ToArray(), SideColorData.ToArray());
        }

        public void Render(Matrix4 view, Matrix4 projection, int[] indexData, Vector3[] vertices, Color4[] colorData)
        {
            Shader shader = new(VertexShader, FragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexData.Length * sizeof(int), indexData, BufferUsageHint.StaticDraw);

            PositionBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, colorData.Length * sizeof(float) * 4, colorData, BufferUsageHint.StaticDraw);

            int colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

            Apex = ApexBuffer * Transformation;
            shader.SetMatrix4("model", Model * Transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, indexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void UpdateBaseModel()
        {
            Model *= Transformation;
            ApexBuffer *= Transformation;
            Transformation = Matrix4.Identity;
        }

        public Vector3 GetApexPosition() => ApexPoint = Helpers.GetPositionFromMatrix(Apex);
        public Vector3 GetCenterPoint() => Center = Helpers.GetPositionFromMatrix(Model);

        public void SetColor(Color4 colorData)
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
                result[i + 1] = (new(point.X, level, point.Y));
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
