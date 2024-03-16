﻿using OpenTK.Graphics.OpenGL4;
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
        private List<int> SidesIndexData = [];
        private List<Vector3> SidesVertices = [];

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
            TopVertices = CreateRoundBase(50).ToList();
            BottomVertices = CreateRoundBase(10).ToList();
            SidesVertices.AddRange(BottomVertices);
            SidesVertices.AddRange(TopVertices);
            GenerateBaseIndices();
            GenerateSideIndices();

        }

        public void RenderCylinder(Matrix4 view, Matrix4 projection)
        {
            Color4[] color = new Color4[BaseIndexData.Count];
            for (int i  = 0; i < BaseIndexData.Count; i++)
            {
                color[i] = Color4.DeepSkyBlue;
            }

            Color4[] color2 = new Color4[SidesIndexData.Count];
            for (int i = 0; i < SidesIndexData.Count; i++)
            {
                color2[i] = Color4.ForestGreen;
            }

            Render(view, projection, BaseIndexData.ToArray(), TopVertices.ToArray(), ColorData);
            Render(view, projection, BaseIndexData.ToArray(), BottomVertices.ToArray(), color);
            Render(view, projection, SidesIndexData.ToArray(), SidesVertices.ToArray(), color2);
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

            shader.SetMatrix4("model", Model * Transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, indexData.Length, DrawElementsType.UnsignedInt, 0);
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
            BaseIndexData.Add(0);
            BaseIndexData.Add(Sides - 1);
            BaseIndexData.Add(1);
        }

        private void GenerateSideIndices()
        {
            int c = TopVertices.Count;
            for (int i = 1; i < Sides ; i++)
            {
                SidesIndexData.Add(i);
                SidesIndexData.Add(i + 1);
                SidesIndexData.Add(i + c);
                SidesIndexData.Add(i + c);
                SidesIndexData.Add(i + c + 1);
                SidesIndexData.Add(i + 1);
            }

            SidesIndexData.Add(1);
            SidesIndexData.Add(c - 1);
            SidesIndexData.Add(c + 1);
            SidesIndexData.Add(c + 1);
            SidesIndexData.Add(c + Sides);
            SidesIndexData.Add(c - 1);
        }
    }
}
