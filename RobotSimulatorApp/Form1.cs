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
        private Timer timer = null!;
        private float _angle = 0.0f;
        INativeInput? NativeInput;
        Rectangle GlControlBounds;

        Camera camera;
        Cube cube;
        Cube cube2;
        Grid grid;

        public int VertexArrayObject { get; private set; }
        public int ElementBufferObject { get; private set; }
        public int PositionBuffer { get; private set; }

        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 position = new Vector3(0f, 0f, 5f);
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
            GL.Enable(EnableCap.DepthTest);

            cube2 = new(glControl, "cube2", new Vector3(-2f, 0f, 0f), 5f, 7f, 2f);
            cube = new(glControl, "cube1", new Vector3(-1f, -1f, -1f), 2f, 2f, 2f);
            grid = new Grid(glControl);

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

            camera = new(new Vector3(0f, 0f, 5f), AspectRatio);
        }

        private void Render()
        {

            glControl.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), MathHelper.DegreesToRadians(_angle));

            //Matrix4 view = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 1, 100);

            INativeInput input = glControl.EnableNativeInput();

            if (captureMouseCheckBox.Checked)
            {
                camera.Move(input);
            }

            Matrix4 view = Matrix4.LookAt(position, position + front, Vector3.UnitY);

            //Matrix4 view = Matrix4.LookAt(0, 10, 1, 0, 1, 0, 0, 1,0 );
            //grid.RenderGrid(Matrix4.Identity, camera.View, projection);

            cube2.RenderCube(Matrix4.Identity, camera.View, projection);
            cube.RenderCube(model, camera.View, projection);
            grid.RenderGrid(Matrix4.Identity, view, projection);
            glControl.SwapBuffers();
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

        private void returnDefaultButton_Click(object sender, EventArgs e)
        {
            //positionX.Value = 0;
            //positionY.Value = 0;
            //positionZ.Value = 9;

            //frontX.Value = 0;
            //frontY.Value = 0;
            //frontZ.Value = -1; 
            
            positionX.Value = -7;
            positionY.Value = 35;
            positionZ.Value = -7;

            frontX.Value = -9;
            frontY.Value = -3;
            frontZ.Value = -1;
        }

        private void getCameraButton_Click(object sender, EventArgs e)
        {
            positionX.Value = (decimal)(camera != null ? camera.Position.X : 0);
            positionY.Value = (decimal)(camera != null ? camera.Position.Y : 0);
            positionZ.Value = (decimal)(camera != null ? camera.Position.Z : 0);

            frontX.Value = (decimal)(camera != null ? camera.Front.X : 0);
            frontY.Value = (decimal)(camera != null ? camera.Front.Y : 0);
            frontZ.Value = (decimal)(camera != null ? camera.Front.Z : 0);
        }

        private void setCameraButton_Click(object sender, EventArgs e)
        {
            position = new((float)positionX.Value, (float)positionY.Value, (float)positionZ.Value);
            front = new((float)frontX.Value, (float)frontY.Value, (float)frontZ.Value);
            Matrix4 view = Matrix4.LookAt(position, position + front, Vector3.UnitZ);
        }
    }

}
