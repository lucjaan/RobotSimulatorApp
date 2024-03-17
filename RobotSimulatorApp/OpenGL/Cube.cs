using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Collections.Generic;

namespace RobotSimulatorApp.GlConfig
{
    public class Cube
    {
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Transformation { get; set; }
        public Matrix4 Point { get; set; }
        public Matrix4 CenterPoint { get; set; }
        public Matrix4 Buffer1 { get; set; }
        public Matrix4 Buffer2 { get; set; }

        private readonly GLControl GlControl;
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        private readonly List<Vector3> Vertices = [];
        private static readonly int[] IndexData =
        {
             0,  1,  2,  2,  3,  0,
             4,  5,  6,  6,  7,  4,
             8,  9, 10, 10, 11,  8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20,
        };

        private Color4[] ColorData = new Color4[36];

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

        public Cube(GLControl glControl, Vector3 position, float sizeX, float sizeY, float sizeZ)
        {
            Position = position;
            GlControl = glControl;
            Size = new Vector3(sizeX, sizeY, sizeZ);
            Center = new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2) + position;
            Transformation = Point = Matrix4.Identity;
            Buffer1 = Buffer2 = Matrix4.Identity;

            Model = Matrix4.CreateTranslation(position);
            CenterPoint = Matrix4.CreateTranslation(sizeX / 2, sizeY / 2, sizeZ / 2) * Matrix4.CreateTranslation(position);
            Buffer1 = CenterPoint;
            ////Create vertices responsible for generating a cube and add them for later use:
            Vertices.AddRange(CreateWall(sizeX, sizeY, 0, Axis.Z));
            Vertices.AddRange(CreateWall(sizeX, 0, sizeZ, Axis.Y));
            Vertices.AddRange(CreateWall(0, sizeY, sizeZ, Axis.X));
            Vertices.AddRange(CreateWall(sizeX, sizeY, sizeZ, Axis.Z));
            Vertices.AddRange(CreateWall(sizeX, sizeY, sizeZ, Axis.Y));
            Vertices.AddRange(CreateWall(sizeX, sizeY, sizeZ, Axis.X));
        }

        public void RenderCube(Matrix4 view, Matrix4 projection)
        {
            Shader shader = new(VertexShader, FragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, BufferUsageHint.StaticDraw);

            PositionBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * Vertices.Count * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

            int colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

            Point = Buffer2 * Transformation;
            CenterPoint = Buffer1 * Transformation;
            shader.SetMatrix4("model", Model * Transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void SetPoint(Vector3 point) {
           Buffer2 = Point = Matrix4.CreateTranslation(point) * Matrix4.CreateTranslation(Position);
        }

        public void SetPosition(Vector3 position) => Model = Matrix4.CreateTranslation(position);

        public void UpdateBaseModel()
        {
            Model = Model * Transformation;
            Buffer2 *= Transformation;
            Buffer1 *= Buffer1 * Transformation;
            Transformation = Matrix4.Identity;
        }

        public void SetColor(Color4 colorData)
        {
            Color4[] color = new Color4[24];
            for (int i = 0; i < 4; i++)
            {
                //very rudimentary shadow simulation
                color[i + 16] = colorData;

                color[i] = color[i + 8] = color[i + 12] = color[i + 20] = new Color4(
                    MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                    1);

                color[i + 4] = new Color4(
                    MathHelper.Clamp(colorData.R - 0.1f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.1f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.1f, 0f, 1),
                    1);
            }

            ColorData = color;
        }

        private List<Vector3> CreateWall(float x, float y, float z, Axis axis)
        {
            List<Vector3> result = [];

            switch (axis)
            {
                case Axis.X:
                    foreach (Vector2 v in CreateWallRectangle(y, z))
                    {
                        result.Add(new Vector3(x, v.X, v.Y));
                    }
                    break;

                case Axis.Y:
                    foreach (Vector2 v in CreateWallRectangle(x, z))
                    {
                        result.Add(new Vector3(v.X, y, v.Y));
                    }
                    break;

                case Axis.Z:
                    foreach (Vector2 v in CreateWallRectangle(x, y))
                    {
                        result.Add(new Vector3(v.X, v.Y, z));
                    }
                    break;
            }
            return result;
        }

        private static Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
