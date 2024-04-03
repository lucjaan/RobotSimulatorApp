using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;

namespace RobotSimulatorApp.Shapes
{
    public abstract class Shape
    {
        private int VertexArrayObject { get; set; }
        private int ElementBufferObject { get; set; }
        private int PositionBufferObject { get; set; }
        private int ColorBufferObject { get; set; }

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

        public void Render(Matrix4 model, Matrix4 transformation, Matrix4 view, Matrix4 projection, ShapeArrays arrays)
        {
            Shader shader = new(VertexShader, FragmentShader);
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, arrays.IndexData.Length * sizeof(int), arrays.IndexData, BufferUsageHint.StaticDraw);

            PositionBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * arrays.Vertices.Length * sizeof(float), arrays.Vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, arrays.ColorsData.Length * sizeof(float) * 4, arrays.ColorsData, BufferUsageHint.StaticDraw);

            int colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

            shader.SetMatrix4("model", model * transformation);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(BeginMode.Triangles, arrays.IndexData.Length, DrawElementsType.UnsignedInt, 0);
            shader.Dispose();
        }

        public void RenderBorder(Matrix4 model, Matrix4 transformation, Matrix4 view, Matrix4 projection, ShapeArrays arrays)
        {

        }

        public abstract void UpdateBaseModel();
        public abstract void SetColor(Color4 colorData);
    }
}
