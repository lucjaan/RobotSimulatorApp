using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
//using Matrix4Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RobotSimulatorApp.GlConfig
{
    internal class Cube
    {
        public string Name { get; set; }
        public Vector3 Center { get; set; }
        private Vector3 Position { get; set; }
        private Matrix4 Model { get; set; }

        private readonly GLControl GlControl;
        
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        private List<Vector3> Vertices = [];
        private static readonly int[] IndexData =
        {
             0,  1,  2,  2,  3,  0,
             4,  5,  6,  6,  7,  4,
             8,  9, 10, 10, 11,  8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20,
        };

        private static readonly Color4[] ColorData = new Color4[]
        {
            Color4.Silver, Color4.Silver, Color4.Silver, Color4.Silver,
            Color4.Honeydew, Color4.Honeydew, Color4.Honeydew, Color4.Honeydew,
            Color4.Moccasin, Color4.Moccasin, Color4.Moccasin, Color4.Moccasin,
            Color4.IndianRed, Color4.IndianRed, Color4.IndianRed, Color4.IndianRed,
            Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed,
            Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen,
        };

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

        public Cube(GLControl glControl, string name, Vector3 position, float length, float height, float width)
        {
            Name = name;
            Position = position;
            GlControl = glControl;
            Center = new Vector3(length / 2, height / 2, width / 2) + position;
            Model = Matrix4.CreateTranslation(position);

            //Create vertices responsible for generating a cube and add them for later use:
            Vertices.AddRange(CreateWall(length, height, 0, Axis.Z));
            Vertices.AddRange(CreateWall(length, 0, width, Axis.Y));
            Vertices.AddRange(CreateWall(0, height, width, Axis.X));
            Vertices.AddRange(CreateWall(length, height, width, Axis.Z));
            Vertices.AddRange(CreateWall(length, height, width, Axis.Y));
            Vertices.AddRange(CreateWall(length, height, width, Axis.X));
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

            shader.SetMatrix4("model", Model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void RotateCube(float angle, Vector3 centerOfRotation, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    Model *= CreateRotationXAroundPoint(angle, centerOfRotation);
                    break;

                case Axis.Y:
                    Model *= CreateRotationYAroundPoint(angle, centerOfRotation);
                    break;

                case Axis.Z:
                    Model *= CreateRotationZAroundPoint(angle, centerOfRotation);
                    break;
            }
        }

        private Matrix4 CreateRotationXAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationX(angle) * Matrix4.CreateTranslation(-centerVector);
        private Matrix4 CreateRotationYAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-centerVector);
        private Matrix4 CreateRotationZAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(-centerVector);

        private List<Vector3> CreateWall(float x, float y, float z, Axis axis)
        {
            List<Vector3> result = [];

            switch (axis)
            {
                case Axis.X:
                    foreach (Vector2 v in CreateWallRectangle(y,z))
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

        private Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
