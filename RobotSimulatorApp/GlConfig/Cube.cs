using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
//using Matrix4Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RobotSimulatorApp.GlConfig
{
    public class Cube
    {
        public Vector3 Center { get; set; }
        public Vector3 FirstCenter { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public Matrix4 Model { get; set; }
        public Matrix4 Translation { get; set; }
        public Matrix4 Rotation { get; set; }
        public Matrix4 Transformation { get; set; }
        public Matrix4 PrevTransformation { get; set; }
        public Matrix4 BaseModel { get; set; }
        public Matrix4 Identity { get; set; }
        public Matrix4 Point { get; set; }
        public Matrix4 CenterPoint { get; set; }
        public Matrix4 Buffer1 { get; set; }
        public Matrix4 Buffer2 { get; set; }
        public float Angle { get; set; }
        private Trace Trace { get; set; }

        private readonly GLControl GlControl;
        private bool isTraceSet;
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }
        private int calls = 0;

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

        public Cube(GLControl glControl, Vector3 position, Vector3 size)
        {
            Position = position;
            GlControl = glControl;
            Size = size;
            //FirstCenter = Center = Matrix4.CreateTranslation(new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position);
            FirstCenter = Center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position;
            PrevTransformation = Transformation = Matrix4.Identity;
            Rotation = Translation = Model = Point = CenterPoint = Matrix4.Identity;
            Buffer1 = Buffer2 = Matrix4.Identity;

            BaseModel = Model = Matrix4.CreateTranslation(position);
            isTraceSet = false;
            Angle = 0f;
            CenterPoint = Matrix4.CreateTranslation(size.X / 2, size.Y / 2, size.Z / 2) * Matrix4.CreateTranslation(position);
            Buffer1 = CenterPoint;
            ////Create vertices responsible for generating a cube and add them for later use:
            Vertices.AddRange(CreateWall(size.X, size.Y, 0, Axis.Z));
            Vertices.AddRange(CreateWall(size.X, 0, size.Z, Axis.Y));
            Vertices.AddRange(CreateWall(0, size.Y, size.Z, Axis.X));
            Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.Z));
            Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.Y));
            Vertices.AddRange(CreateWall(size.X, size.Y, size.Z, Axis.X));
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
            //Debug.WriteLine($"============== Model * Transformation ======================");
            //Debug.WriteLine($"{Model * Transformation}");
            //Debug.WriteLine($"=================== Transformation ============================");
            //Debug.WriteLine($"{Transformation}");

            //shader.SetMatrix4("model", BaseModel * Transformation);
            Point = Buffer2 * Transformation;
            CenterPoint = Buffer1 * Transformation;
            //Debug.WriteLine(Helpers.GetPositionFromMatrix(CenterPoint));
            shader.SetMatrix4("model", Model * Transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void SetPoint(Vector3 point) {
           Buffer2 = Point = Matrix4.CreateTranslation(point) * Matrix4.CreateTranslation(Position);
        } 

        public void UpdateBaseModel()
        {
            //BaseModel = Model;
            //Model *= PrevTransformation;
            calls++;
            Debug.WriteLine($"Calls {calls}");
            var x = Transformation;
            //Model = BaseModel * PrevTransformation * Transformation;
            var z = BaseModel;
            Model = Model * Transformation;
            Buffer2 = Buffer2 * Transformation;
            Buffer1 = Buffer1 * Transformation;
            //Buffer1 = Buffer1 * Transformation;
            //Model = BaseModel * Transformation;
            //Debug.WriteLine(Transformation);
            //Debug.WriteLine("------------------");
            //if (Transformation != Matrix4.Identity)
            //    PrevTransformation = Transformation;
            //Debug.WriteLine(PrevTransformation);
            //Debug.WriteLine("------------------");

            PrevTransformation = Matrix4.Identity;
            Transformation = Matrix4.Identity;

            //Matrix4 test = Model * Matrix4.Invert(PrevTransformation);
            //Model = Model * Matrix4.Invert(PrevTransformation);
            ////Matrix4 test = Transformation * Matrix4.Invert(Transformation);
            ////PrevTransformation = Transformation;
            //PrevTransformation = Transformation;
        }

        public void UpdateBaseModel(Matrix4 transformation)
        {
            //Model = BaseModel * transformation;
            //Transformation = Matrix4.Identity;
            Transformation = transformation;
        }

        public void SetTransformation(Matrix4 hgm)
        {
            Transformation = hgm;
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

        public void RotateCenter(float angle, Vector3 center)
        {
            angle = MathHelper.DegreesToRadians(angle);
            float cenX = FirstCenter.X - center.X;
            float cenZ = FirstCenter.Z - center.Z;

            float x = cenX * (float)MathHelper.Cos(angle) + cenZ * (float)MathHelper.Sin(angle);
            float z = -(cenX * (float)MathHelper.Sin(angle)) + cenZ * (float)MathHelper.Cos(angle);

            Center = center + new Vector3(x, 0, z);
        }

        public void SetTrace(bool isSet) => isTraceSet = isSet;
        public void SetPosition(Vector3 position) => Model = Matrix4.CreateTranslation(position);
        public void TranslateCube(Vector3 translationVector)
        {
            Position += translationVector;
            Model *= Matrix4.CreateTranslation(translationVector);
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
            //=> new Vector2[] { new(-a / 2, -b / 2 ), new(a/2 , -b/2), new(a/2, b/2), new(-a/2, b/2) };
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
