using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RobotSimulatorApp.GlConfig
{
    internal class Cube
    {
        public static string Name;
        private GLControl GlControl;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private int PositionBufferObject;
        private int ColorBufferObject;

        private List<Vector3> Vertices = [];
        public readonly int[] IndexData =
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

        private Shader Shader = new Shader(VertexShader, FragmentShader);

        public Cube(GLControl glControl, string name, Vector3 position, float length, float width, float height)
        {
            Name = name;
            GlControl = glControl;

            GlControl.MakeCurrent();

            //Create vertices responsible for generating a cube and add them for later use:
            Vertices.AddRange(CreateWall(position, length, width, 0, "z"));
            Vertices.AddRange(CreateWall(position, length, 0, height, "y"));
            Vertices.AddRange(CreateWall(position, 0, width, height, "x"));
            Vertices.AddRange(CreateWall(position, length, width, height, "z"));
            Vertices.AddRange(CreateWall(position, length, width, height, "y"));
            Vertices.AddRange(CreateWall(position, length, width, height, "x"));
            Shader Shader = new Shader(VertexShader, FragmentShader);

            Shader.Use();

            GL.Enable(EnableCap.DepthTest);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, BufferUsageHint.StaticDraw);

            PositionBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * ReturnInternalVectors().Length * sizeof(float), ReturnInternalVectors(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0); //enables vertex
            var vertexLocation = Shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
        }

        public Vector3[] ReturnInternalVectors()
        {
            Vector3[] result = new Vector3[Vertices.Count];

            for (int i = 0; i < Vertices.Count; i++)
            {
                result[i] = Vertices[i];
            }
            return result;
        }
        
        public void RenderCube(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            GlControl.MakeCurrent();

            Shader.SetMatrix4("model", model);
            Shader.SetMatrix4("view", view);
            Shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            GlControl.SwapBuffers();
        }

        private List<Vector3> CreateWall(Vector3 position, float x, float y, float z, string dimension)
        {
            List<Vector3> result = [];

            switch (dimension)
            {
                case "x":
                    foreach (Vector2 v in CreateWallRectangle(y,z))
                    {
                        result.Add(new Vector3(position.X + x, v.X + position.Y, v.Y + position.Z));
                    }
                    break;

                case "y":
                    foreach (Vector2 v in CreateWallRectangle(x, z))
                    {
                        result.Add(new Vector3(v.X + position.X, position.Y + y, v.Y + position.Z));
                    }
                    break;

                case "z":
                    foreach (Vector2 v in CreateWallRectangle(x, y))
                    {
                        result.Add(new Vector3(v.X + position.X, v.Y + position.Y, position.Z + z));
                    }
                    break;
            }
            return result;
        }

        private Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
