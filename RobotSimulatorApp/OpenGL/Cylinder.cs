using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System.Collections.Generic;
using System.Linq;

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
        private int Sides { get; set; }

        private readonly GLControl GlControl;
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

        //private readonly Color4[] ColorData = new Color4[36];
        private readonly List<Vector3> TopVertices = [];
        private readonly List<Vector3> BottomVertices = [];
        private readonly List<int> BaseIndexData = [];
        private readonly List<int> SidesIndexData = [];
        private readonly List<Vector3> SidesVertices = [];

        private readonly List<Color4> SideColorData = [];
        private readonly List<Color4> TopColorData = [];
        private readonly List<Color4> BottomColorData = [];


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

        public Cylinder(GLControl glControl, Vector3 position, float radius, float height)
        {
            Position = Center = position;
            GlControl = glControl;
            Radius = radius;
            Sides = 90;

            //Center = new Vector3(size.X / 2, size.Y / 2, size.Z / 2) + position;
            Transformation = Matrix4.Identity;
            
            Model = Matrix4.CreateTranslation(position);

            BottomVertices = CreateRoundBase(position.Z).ToList();
            TopVertices = CreateRoundBase(position.Z + height).ToList();

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

            Render(view, projection, BaseIndexData.ToArray(), TopVertices.ToArray(), TopColorData.ToArray());
            Render(view, projection, BaseIndexData.ToArray(), BottomVertices.ToArray(), BottomColorData.ToArray());
            Render(view, projection, SidesIndexData.ToArray(), SidesVertices.ToArray(), SideColorData.ToArray());
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

        public void UpdateBaseModel()
        {
            Model = Model * Transformation;
            Transformation = Matrix4.Identity;
        }

        public void SetColor(Color4 colorData)
        {
            if (TopColorData != null || BottomColorData != null || SideColorData != null)
            {
                TopColorData.Clear();
                BottomColorData.Clear();
                SideColorData.Clear();
            }

            for (int i = 0; i < SidesIndexData.Count; i++)
            {
                SideColorData.Add(colorData);
            }

            for (int i = 0; i < BaseIndexData.Count; i++)
            {
                //TopColorData.Add(new Color4(
                //       MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                //       MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                //       MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                //       1));

                if (i % 2 == 0)
                {
                    TopColorData.Add(new Color4(
                    MathHelper.Clamp(colorData.R + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.G + 0.05f, 0f, 1),
                    MathHelper.Clamp(colorData.B + 0.05f, 0f, 1),
                    1));
                }
                else
                {
                    TopColorData.Add(Color4.OrangeRed);
                }
            }

            for (int i = 0; i < BaseIndexData.Count; i++)
            {
                BottomColorData.Add(new Color4(
                MathHelper.Clamp(colorData.R - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.G - 0.05f, 0f, 1),
                MathHelper.Clamp(colorData.B - 0.05f, 0f, 1),
                1));
            }
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
