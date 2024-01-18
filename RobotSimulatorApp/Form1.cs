using Microsoft.VisualBasic.Devices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace RobotSimulatorApp
{
    public partial class Form1 : Form
    {
        readonly private float AspectRatio;
        private Shader? shader;
        private Timer timer = null!;
        private float _angle = 0.0f;
        INativeInput? NativeInput;
        Rectangle GlControlBounds;
        bool FirstMove = true;

        Camera? camera;
        Cube cube;
        Grid grid;
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

        private static readonly Color4[] ColorData = new Color4[]
{
            Color4.Silver, Color4.Silver, Color4.Silver, Color4.Silver,
            Color4.Honeydew, Color4.Honeydew, Color4.Honeydew, Color4.Honeydew,
            Color4.Moccasin, Color4.Moccasin, Color4.Moccasin, Color4.Moccasin,
            Color4.IndianRed, Color4.IndianRed, Color4.IndianRed, Color4.IndianRed,
            Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed,
            Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen,
};

        public int VertexArrayObject { get; private set; }
        public int ElementBufferObject { get; private set; }
        public int PositionBuffer { get; private set; }

        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 position = new Vector3(0f, 0f, 8f);
        private int ColorBuffer;

        public Form1()
        {
            InitializeComponent();
            AspectRatio = Math.Max(glControl.ClientSize.Width, 1) / (float)Math.Max(glControl.ClientSize.Height, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //glControl.Paint += glControl_Paint;
            SetUpOpenGL();

            glControl.GotFocus += (sender, e) =>
                textBox1.AppendText("Focus in");
            glControl.LostFocus += (sender, e) =>
                textBox1.AppendText("Focus out");

            glControl.MouseDown += (sender, e) =>
            {
                glControl.Focus();
                textBox1.AppendText($"WinForms Mouse down: ({e.X},{e.Y})");
            };
            glControl.KeyDown += (sender, e) =>
                textBox1.AppendText($"WinForms Key down: {e.KeyCode}");
        }

        private void SetUpOpenGL()
        {
            //GL.Enable(EnableCap.DepthTest);

            cube = new(glControl, "cube1", new Vector3(-1f, -1f, -1f), 2f, 2f, 2f);

            timer = new Timer();
            timer.Tick += (sender, e) =>
            {
                Render();
                const float DELTA_TIME = 1 / 50f;
                _angle += 180f * DELTA_TIME;
            };
            timer.Interval = 50;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            timer.Start();

            glControl_Resize(glControl, EventArgs.Empty);

            //grid = new Grid();
            camera = new(new Vector3(0f, 0f, 5f), AspectRatio);

            //shader = new Shader(VertexShader, FragmentShader);
            //shader.Use();


            //VertexArrayObject = GL.GenVertexArray();
            //GL.BindVertexArray(VertexArrayObject);

            //ElementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, cube.IndexData.Length * sizeof(int), cube.IndexData, BufferUsageHint.StaticDraw);

            //PositionBuffer = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, 3 * cube.ReturnInternalVectors().Length * sizeof(float), cube.ReturnInternalVectors(), BufferUsageHint.StaticDraw);


            //GL.EnableVertexAttribArray(0); //enables vertex
            ////var vertexLocation = shader.GetAttribLocation("aPosition");
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //ColorBuffer = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

            //GL.EnableVertexAttribArray(1);
            //GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
        }

        private void Render()
        {

            glControl.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), MathHelper.DegreesToRadians(_angle));

            Matrix4 view = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 1, 64);

            //shader.Use();
            INativeInput input = glControl.EnableNativeInput();

            if (captureMouseCheckBox.Checked)
            {
                camera.Move(input);
            }

            cube.RenderCube(model, camera.View, projection);
            //grid.RenderGrid();

            //shader.SetMatrix4("model", model);
            //shader.SetMatrix4("view", camera.View);
            //shader.SetMatrix4("projection", projection);

            //GL.DrawElements(BeginMode.Triangles, cube.IndexData.Length, DrawElementsType.UnsignedInt, 0);

            //shader.Dispose();
            //glControl.SwapBuffers();
        }

        private void glControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();

            if (glControl.ClientSize.Height == 0)
            {
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);
            }
            Vector3 position = new Vector3(0f, 0f, 8f);

            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;
            int borderWidth = screenRectangle.Right - this.Right;

            GlControlBounds = new Rectangle(glControl.Bounds.X + Bounds.X - borderWidth,
                glControl.Bounds.Y + Bounds.Y + titleHeight,
                glControl.Bounds.Width,
                glControl.Bounds.Height);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (sender != null)
            {
                glControl.MakeCurrent();
                GL.ClearColor(Color4.MidnightBlue);
                GL.Clear(ClearBufferMask.ColorBufferBit);
                glControl.SwapBuffers();
            }
        }

        private void captureMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (captureMouseCheckBox.Checked)
            {
                glControl.Focus();
                //Cursor.Hide();
                Cursor.Clip = GlControlBounds;

                INativeInput input = glControl.EnableNativeInput();
                if (NativeInput == null)
                {
                    NativeInput = input;
                    NativeInput.KeyUp += (e) =>
                    {
                        if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape)
                        {
                            captureMouseCheckBox.Checked = false;
                        }
                    };
                }
            }

            if (!captureMouseCheckBox.Checked)
            {
                glControl.DisableNativeInput();
                glControl.Focus();
                //Cursor.Show();
                Cursor.Clip = Rectangle.Empty;
                front = new Vector3(0.0f, 0.0f, -1.0f);
                position.Z = 30f;
            }
        }
    }

}
