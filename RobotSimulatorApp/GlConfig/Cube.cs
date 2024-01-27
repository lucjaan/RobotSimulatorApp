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
    internal class Cube
    {
        public Vector3 Center { get; set; }
        public Vector3 Position { get; set; }
        private Matrix4 Model { get; set; }
        private Trace Trace { get; set; }

        private readonly GLControl GlControl;
        private bool isTraceSet;
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

        //private Color4[] ColorData =
        //[
        //    Color4.Silver, Color4.Silver, Color4.Silver, Color4.Silver,
        //    Color4.Honeydew, Color4.Honeydew, Color4.Honeydew, Color4.Honeydew,
        //    Color4.Moccasin, Color4.Moccasin, Color4.Moccasin, Color4.Moccasin,
        //    Color4.IndianRed, Color4.IndianRed, Color4.IndianRed, Color4.IndianRed,
        //    Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed,
        //    Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen,
        //];

        private Color4[] ColorData =
        [
            Color4.DarkRed,
            Color4.DarkRed,
            Color4.DarkRed,
            Color4.DarkRed,
            Color4.WhiteSmoke,
            Color4.WhiteSmoke,
            Color4.WhiteSmoke,
            Color4.WhiteSmoke,
            Color4.Yellow,
            Color4.Yellow,
            Color4.Yellow,
            Color4.Yellow,
            Color4.Orange,
            Color4.Orange,
            Color4.Orange,
            Color4.Orange,
            Color4.Black,
            Color4.Black,
            Color4.Black,
            Color4.Black,
            Color4.ForestGreen,
            Color4.ForestGreen,
            Color4.ForestGreen,
            Color4.ForestGreen,
        ];

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
            Center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position;
            //Center = new Vector3(15, 2, 11);// + position;
            Model = Matrix4.CreateTranslation(position);

            //Center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position;

            isTraceSet = false;
            //Create vertices responsible for generating a cube and add them for later use:
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

            shader.SetMatrix4("model", Model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void RotateCube(float angle, Vector3 centerOfRotation, Axis axis)
        {
            angle = MathHelper.DegreesToRadians(angle);
            switch (axis)
            {
                case Axis.X:
                    Model *= CreateRotationXAroundPoint(angle, centerOfRotation);
                    break;

                case Axis.Y:
                    var dd = Model;
                    Model *= CreateRotationYAroundPoint(angle, centerOfRotation);
                    var z = Model;
                    break;

                case Axis.Z:
                    Model *= CreateRotationZAroundPoint(angle, centerOfRotation);
                    break;
            }
        }

        public void SetColor(Color4 colorData)
        {
            Color4[] color = new Color4[24];
            for (int i = 0; i < 4; i++)
            {
                //very rudimentary shadow simulation
                color[i + 16] = colorData;

                color[i] = color[i + 8] = color[i + 12] = color[i + 20] = new Color4
                    (MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.05f, 0f, 1), 1);

                color[i + 4] = new Color4
                    (MathHelper.Clamp(colorData.R - 0.1f, 0f, 1),
                    MathHelper.Clamp(colorData.G - 0.1f, 0f, 1),
                    MathHelper.Clamp(colorData.B - 0.1f, 0f, 1), 1);
            }

            ColorData = color;
        }

        public void SetTrace(bool isSet) => isTraceSet = isSet;

        private Matrix4 CreateRotationXAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationX(angle) * Matrix4.CreateTranslation(-centerVector);
        //private static Matrix4 CreateRotationYAroundPoint(float angle, Vector3 centerVector)
        //    => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-centerVector);

        private Matrix4 CreateRotationYAroundPoint(float angle, Vector3 centerVector)
        {
            //angle = MathHelper.DegreesToRadians(5f);
            //Matrix4 rotationBase = Matrix4.CreateRotationY(angle);
            //Matrix4 rotation = Matrix4.CreateRotationY(angle);
            //bool areEqual = Matrix4.Equals(rotationBase, rotation);

            ////rotation.M41 = centerVector.X;
            ////rotation.M43 = centerVector.Z;

            //rotation.M41 = 5;
            //rotation.M43 = 0;
            ////rotation = Matrix4.CreateTranslation(new ) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-centerVector);
            ////rotation.M41 = 0;
            ////rotation.M43 = 0;
            //areEqual = Matrix4.Equals(rotationBase, rotation);

            //return rotation;
            //centerVector = Vector3.Normalize(new(centerVector.X, 0, centerVector.Z));
            //centerVector = new(centerVector.X, 0, centerVector.Z);
            //centerVector = new(1, 15, 1);
            //centerVector = Position;

            //return Matrix4.CreateFromAxisAngle(centerVector, angle);
            return Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-centerVector);
        }
        //=> Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(-centerVector); 

        private Matrix4 CreateRotationZAroundPoint(float angle, Vector3 centerVector)
            => Matrix4.CreateTranslation(centerVector) * Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(-centerVector);

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

        private Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
