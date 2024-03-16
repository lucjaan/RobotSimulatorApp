using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.OpenGL
{
    public class Cylinder
    {
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Transformation { get; set; }
        public Matrix4 CenterPoint { get; set; }
        public float Radius { get; set; }

        private readonly GLControl GlControl;
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        private Color4[] ColorData = new Color4[36];
        private List<Vector3> TopVertices = [];
        private List<Vector3> BottomVertices = [];
        private List<int> BaseIndexData = [];

        private int Sides { get; set; }

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

        public Cylinder(GLControl glControl, Vector3 position, float radius)
        {
            Position = position;
            GlControl = glControl;
            Radius = radius;
            //Center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position;
            Transformation  = Matrix4.Identity;
            
            Model = Matrix4.CreateTranslation(position);

            Sides = 90;
            BottomVertices = CreateRoundBase(50).ToList();
            GenerateBaseIndices();
            //CenterPoint = Matrix4.CreateTranslation(size.X / 2, size.Y / 2, size.Z / 2) * Matrix4.CreateTranslation(position);

            ////Create vertices responsible for generating a cube and add them for later use:
            //Vertices.AddRange(CreateWall(size.X, size.Y, 0, Axis.Z));
            //Vertices.AddRange(CreateWall(size.X, 0, size.Z, Axis.Y));
            //Vertices.AddRange(CreateWall(0, size.Y, size.Z, Axis.X));
            //Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.Z));
            //Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.Y));
            //Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.X));
        }

        //public void RenderCylinder(Matrix4 view, Matrix4 projection)
        //{
        //    Shader shader = new(VertexShader, FragmentShader);
        //    shader.Use();

        //    VertexArrayObject = GL.GenVertexArray();
        //    GL.BindVertexArray(VertexArrayObject);

        //    ElementBufferObject = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, BufferUsageHint.StaticDraw);

        //    PositionBufferObject = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
        //    GL.BufferData(BufferTarget.ArrayBuffer, 3 * Vertices.Count * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);

        //    int vertexLocation = shader.GetAttribLocation("aPosition");
        //    GL.EnableVertexAttribArray(vertexLocation);
        //    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        //    ColorBufferObject = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
        //    GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

        //    int colorLocation = shader.GetAttribLocation("aColor");
        //    GL.EnableVertexAttribArray(colorLocation);
        //    GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

        //    shader.SetMatrix4("model", Model * Transformation);
        //    shader.SetMatrix4("view", view);
        //    shader.SetMatrix4("projection", projection);

        //    GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
        //    shader.Dispose();
        //}

        public void RenderBase(Matrix4 view, Matrix4 projection)
        {
            Shader shader = new(VertexShader, FragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, BaseIndexData.Count * sizeof(int), BaseIndexData.ToArray(), BufferUsageHint.StaticDraw);

            PositionBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * BottomVertices.Count * sizeof(float), BottomVertices.ToArray(), BufferUsageHint.StaticDraw);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

            int colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

            shader.SetMatrix4("model", Model * Transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, BaseIndexData.Count, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
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
            //BaseIndexData.Add(0);
            BaseIndexData.Add(0);
            BaseIndexData.Add(Sides - 1);
            BaseIndexData.Add(1);
        }
    }
}
