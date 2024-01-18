using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RobotSimulatorApp.GlConfig
{
    internal class Grid
    {
        public List<Vector2> Vertices = [];
        public int[] IndexData = [];

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        private static readonly string VertexShader =
   @"#version 330 core

layout(location = 0) in vec2 aPosition;

void main(void)
{
    gl_Position = vec4(aPosition, 0.0, 1.0);

} ";

        private static readonly string FragmentShader =
@"#version 330 core
in vec4 fColor;

out vec4 oColor;

void main()
{
    oColor = fColor;
}";
        private Shader Shader = new Shader(VertexShader, FragmentShader);

        public Grid()
        {
            CreateGrid();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Vertices.Count * sizeof(int), IndexData, BufferUsageHint.StaticDraw);

            Shader.Use();

            GL.EnableVertexAttribArray(0); //enables vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void CreateGrid()
        {
            int i = 0;
            for (int x = 0; x < 100; x+=5) 
            { 
                for (int y = 0; y < 100; y+=5)
                {
                    Vertices.Add(new Vector2(x, y));
                    var z = IndexData;
                    IndexData.Append(i++);
                }
            }
        }

        public void RenderGrid()
        {
            GL.DrawElements(BeginMode.Lines, IndexData.Length, DrawElementsType.UnsignedInt, 0);
            Shader.Dispose();

        }
    }
}
