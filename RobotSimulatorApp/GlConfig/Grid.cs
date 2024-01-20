using OpenTK;
using OpenTK.Graphics.OpenGL;
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
    internal class Grid
    {
        private GLControl GlControl;

        private List<Vector3> Vertices = [];
        private List<int> IndexData = [];

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        private static readonly string VertexShader =
   @"#version 330 core

layout(location = 0) in vec3 aPosition;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;

} ";

        private static readonly string FragmentShader =
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
        }

        public void CreateGrid()
        {
            int i = 0;

            int bounds = 100;
            int size = 5; 

            for (int x = -bounds; x <  bounds; x+=size)
            {
                for (int z = -bounds; z < bounds; z+=size)
                {
                    List<int> indices = 
                        [i, i + 1, i + (bounds/size),
                        i + (bounds / size), i + 1 + i + (bounds / size), i + 1];

                    IndexData.AddRange(indices);
                    i++;

                    for (int j = 0; j < 4; j++)
                    {
                        Vertices.Add(new Vector3(x, 0, z));
                    }
                }
            }
        }

        public void RenderGrid(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            Shader shader = new(VertexShader, FragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * Vertices.Count * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexData.Count * sizeof(int), IndexData.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0); //enables vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Lines, Vertices.Count, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }
    }
}
