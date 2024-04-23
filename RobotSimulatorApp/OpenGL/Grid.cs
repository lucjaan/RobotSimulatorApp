using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Collections.Generic;

namespace RobotSimulatorApp.GlConfig
{
    internal class Grid
    {
        private readonly GLControl GlControl;

        private readonly List<Vector3> GridVertices = new();
        private readonly List<int> GridIndexData = new();

        private List<Vector3> XYZVertices = new();
        private static readonly List<int> value = new() { 0, 1, 2, 3, 4, 5};
        private static readonly List<int> XYZIndexData = value;

        private int VertexBufferObject { get; set; }
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        private static readonly Color4[] XYZColors =
            new Color4[] { Color4.IndianRed, Color4.IndianRed, Color4.DeepSkyBlue, Color4.DeepSkyBlue, Color4.ForestGreen, Color4.ForestGreen };

        private static readonly string XyzVertexShader =
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
} ";

        private static readonly string XyzFragmentShader =
@"#version 330

in vec4 fColor;
out vec4 outputColor;

void main()
{
    outputColor = fColor;
}";

        private static readonly string GridVertexShader =
 @"#version 330 core

layout(location = 0) in vec3 aPosition;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;

} ";

        private static readonly string GridFragmentShader =
@"#version 330

out vec4 outputColor;

void main()
{
    outputColor = vec4(0.0, 1.0, 1.0, 1.0);
}";

        public Grid(GLControl gLControl)
        {
            GlControl = gLControl;
            CreateGrid();
            CreateXYZ();
        }

        public void RenderWorld(Matrix4 view, Matrix4 projection)
        {
            RenderGrid(view, projection);
            RenderXYZ(view, projection);
        }

        public void CreateGrid()
        {
            int bounds = 300;
            int size = 10;

            //Creating vertical lines
            for (int i = -bounds; i < bounds; i+=size) 
            {
                Vector3[] vertical = new Vector3[] { new(-bounds, 0f, i + size), new(bounds, 0f, i + size) };
                GridVertices.AddRange(vertical);
            }

            //Creating horizontal lines
            for (int i = -bounds; i < bounds; i += size)
            {
                Vector3[] vertical = new Vector3[] { new(i + size, 0f, -bounds), new(i + size, 0f, bounds) };
                GridVertices.AddRange(vertical);
            }

            for (int i = 0; i < GridVertices.Count; i++)
            {
                GridIndexData.Add(i);
            }
        }

        private void CreateXYZ()
        {
            float length = 300f;

            XYZVertices.Add(new Vector3(-length, 0f, 0f));
            XYZVertices.Add(new Vector3(length, 0f, 0f));
            XYZVertices.Add(new Vector3(0f, 0f, -length));
            XYZVertices.Add(new Vector3(0f, 0f, length));
            XYZVertices.Add(new Vector3(0f, -length, 0f));
            XYZVertices.Add(new Vector3(0f, length, 0f));
        }

        private void RenderXYZ(Matrix4 view, Matrix4 projection)
        {
            Shader shader = new(XyzVertexShader, XyzFragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * XYZVertices.Count * sizeof(float), XYZVertices.ToArray(), BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, XYZIndexData.Count * sizeof(int), XYZIndexData.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, XYZColors.Length * sizeof(float) * 4, XYZColors, BufferUsageHint.StaticDraw);

            int colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Lines, XYZVertices.Count, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        private void RenderGrid(Matrix4 view, Matrix4 projection)
        {
            Shader shader = new(GridVertexShader, GridFragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * GridVertices.Count * sizeof(float), GridVertices.ToArray(), BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, GridIndexData.Count * sizeof(int), GridIndexData.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0); //enables vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Lines, GridVertices.Count, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }
    }
}
