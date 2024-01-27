using Microsoft.VisualBasic.Devices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using RobotSimulatorApp.GlConfig;
using RobotSimulatorApp.Robot.SCARA;
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
        Cube cube0;
        Grid grid;
        SCARA_Robot scara;
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
        }

        private void SetUpOpenGL()
        {
            GL.Enable(EnableCap.DepthTest);

            //cube = new(glControl, new Vector3(-5f, -1f, -5f), new Vector3(2f, 22f, 2f));
            cube2 = new(glControl, new Vector3(15f, 0f, 9f), new Vector3(6f, 8f, 10f));

            //cube0 = new(glControl, new Vector3(0f, -3f, 0f), new Vector3(6f, 6f, 6f));
           //cube0.SetColor(Color4.LimeGreen);

            grid = new Grid(glControl);
            scara = new SCARA_Robot(glControl, "tomek");
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
            camera.SetView(new Vector3(69f, 102f, 174f), new Vector3(-35f, -77f, -155f));
            UpdateTrackBars();
        }

        private void Render()
        {

            glControl.MakeCurrent();
            INativeInput input = glControl.EnableNativeInput();

            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 0.1f, 500f);
            //var x = MathHelper.Clamp(_angle, 0, 360);

            //cube.RotateCube(90f, cube.Center, Axis.Y);
            cube2.RotateCube(15f, cube2.Center, Axis.Y);
            //cube0.RotateCube(15f, cube0.Center, Axis.Y);

            if (captureMouseCheckBox.Checked)
            {
                camera.Move(input);
            }

            cube2.RenderCube(camera.View, projection);
            //cube.RenderCube(camera.View, projection);
            //cube0.RenderCube(camera.View, projection);
            //scara.RenderRobot(camera.View, projection);
            grid.RenderWorld(camera.View, projection);
            glControl.SwapBuffers();
        }

        private void glControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();

            if (glControl.ClientSize.Height == 0)
            {
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);
            }

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

            //positionX.Value = -7;
            //positionY.Value = 35;
            //positionZ.Value = -7;

            //frontX.Value = -9;
            //frontY.Value = -3;
            //frontZ.Value = -1;

            //positionX.Value = 5;
            //positionY.Value = 38;
            //positionZ.Value = -19;

            //frontX.Value = -3;
            //frontY.Value = -72;
            //frontZ.Value = -7;
        }

        private void getCameraButton_Click(object sender, EventArgs e)
        {
            //positionX.Value = (decimal)(camera != null ? camera.Position.X : 0);
            //positionY.Value = (decimal)(camera != null ? camera.Position.Y : 0);
            //positionZ.Value = (decimal)(camera != null ? camera.Position.Z : 0);

            //frontX.Value = (decimal)(camera != null ? camera.Front.X : 0);
            //frontY.Value = (decimal)(camera != null ? camera.Front.Y : 0);
            //frontZ.Value = (decimal)(camera != null ? camera.Front.Z : 0);
        }

        private void setCameraButton_Click(object sender, EventArgs e)
        {
            //position = new((float)positionX.Value, (float)positionY.Value, (float)positionZ.Value);
            //front = new((float)frontX.Value, (float)frontY.Value, (float)frontZ.Value);

            //camera.Position = new((float)positionX.Value, (float)positionY.Value, (float)positionZ.Value);
            //camera.Front = new((float)frontX.Value, (float)frontY.Value, (float)frontZ.Value);

            camera.UpdateVectors();
        }

        private void UpdateTextBoxes()
        {
            if (camera == null)
            {
                positionTextBox.Text = string.Empty;
                frontTextBox.Text = string.Empty;
            }
            else
            {
                positionTextBox.Text = $"{camera.Position}";
                frontTextBox.Text = $"{FrontXtrackBar.Value}, {FrontYtrackBar.Value}, {FrontZtrackBar.Value}";
                //frontTextBox.Text = $"{camera.Front}";
            }
        }

        private void UpdateTrackBars()
        {
            FrontXtrackBar.Value = (int)(MathHelper.RadiansToDegrees(camera.Front.X) * MathHelper.Pi);
            FrontYtrackBar.Value = (int)(MathHelper.RadiansToDegrees(camera.Front.Y) * MathHelper.Pi); ;
            FrontZtrackBar.Value = (int)(MathHelper.RadiansToDegrees(camera.Front.Z) * MathHelper.Pi); ;
            PosXTrackBar.Value = (int)camera.Position.X;
            PosYTrackBar.Value = (int)camera.Position.X;
            PosZTrackBar.Value = (int)camera.Position.X;
        }

        private void FrontXtrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Front.X = MathHelper.DegreesToRadians((float)FrontXtrackBar.Value) / MathHelper.Pi;
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void FrontYtrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Front.Y = MathHelper.DegreesToRadians((float)FrontYtrackBar.Value) / MathHelper.Pi;
            camera.UpdateVectors();
            UpdateTextBoxes();

        }

        private void FrontZtrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Front.Z = MathHelper.DegreesToRadians((float)FrontZtrackBar.Value) / MathHelper.Pi;
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void PosXTrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Position.X = PosXTrackBar.Value;
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void PosYTrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Position.Y = PosYTrackBar.Value;
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void PosZTrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Position.Z = PosZTrackBar.Value;
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void jointTrackBar_Scroll(object sender, EventArgs e)
        {
            scara.RobotBase.RotateCube((float)jointTrackBar.Value/1000, scara.RobotBase.Center, Axis.Y);
            var x = (float)jointTrackBar.Value / 1000;
            var z = scara.RobotBase.Center;
        }
    }
}
