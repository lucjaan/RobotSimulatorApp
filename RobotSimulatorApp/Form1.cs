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
        Cube marker;
        Grid grid;
        public SCARA_Robot scara { get; set; }
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

            ControlsForm controlsForm = new ControlsForm(scara);
            controlsForm.Show();

        }

        private void SetUpOpenGL()
        {
            GL.Enable(EnableCap.DepthTest);

            cube = new(glControl, new Vector3(-5f, -1f, -5f), new Vector3(2f, 22f, 2f));
            cube2 = new(glControl, new Vector3(-7f, 0f, 9f), new Vector3(6f, 8f, 10f));
            //cube2 = new(glControl, new Vector3(15f, 0f, 9f), new Vector3(6f, 8f, 10f));
            cube0 = new(glControl, new Vector3(0f, -3f, -7f), new Vector3(6f, 6f, 6f));
            marker = new(glControl, new Vector3(0f, 0, 0), new Vector3(1f, 30f, 1f));
            //cube0 = new(glControl, new Vector3(0f, -3f, 0f), new Vector3(6f, 6f, 6f));
            marker.SetColor(Color4.Red);
            cube0.SetColor(Color4.LimeGreen);
            cube2.SetColor(Color4.MediumVioletRed);
            cube.SetColor(Color4.Olive);

            grid = new Grid(glControl);
            scara = new SCARA_Robot(glControl, "scara");
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

            //cube.RotateCube(15f, cube.Center, Axis.Y);
            //cube2.RotateCube(15f, cube2.Center, Axis.Y);
            //cube0.RotateCube(15f, cube0.Center, Axis.Y);

            if (captureMouseCheckBox.Checked)
            {
                camera.Move(input);
            }

            cube2.RenderCube(camera.View, projection);
            cube.RenderCube(camera.View, projection);
            cube0.RenderCube(camera.View, projection);
            marker.RenderCube(camera.View, projection);

            //cube2.UpdateBaseModel();
            //cube.UpdateBaseModel();
            //cube2.UpdateBaseModel();

            scara.RenderRobot(camera.View, projection);
            grid.RenderWorld(camera.View, projection);
            glControl.SwapBuffers();
        }

        private void glControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();

            if (glControl.ClientSize.Height == 0)
            {
                glControl.ClientSize = new Size(glControl.ClientSize.Width, 1);
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

        private void OpenControlsButton_Click(object sender, EventArgs e)
        {
            ControlsForm controlsForm = new ControlsForm(scara);
            if (!controlsForm.Visible)
                controlsForm.Show();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float value = (float)trackBar1.Value;
            float value2 = (float)trackBar2.Value;

            Vector3 center = Vector3.Zero;
            if (numericUpDown1.Value == 1)
            {
                center = cube.Center;
                marker.SetPosition(center);

                cube.RotateCube(value, center, Axis.Y);
                cube2.RotateCube(value, center, Axis.Y);
                cube0.RotateCube(value, center, Axis.Y);
                cube2.RotateCenter(value, cube.Center);


                Matrix4 m = cube2.Model;
                Matrix4 t = cube2.Transformation;
                Matrix4 mv = cube2.Model * cube2.Transformation;
                Debug.WriteLine($"Model: {new Vector3(mv.M41, mv.M42, mv.M43)} ");
                Debug.WriteLine($"Center: {cube2.Center} ");
            }

            if (numericUpDown1.Value == 2)
            {
                center = cube2.Center;
                marker.SetPosition(center);

                //cube.RotateCube(value, center, Axis.Y);
                cube2.RotateCube(value + value2, center, Axis.Y);
                cube0.RotateCube(value, center, Axis.Y);

                Matrix4 mv = cube2.Model * cube2.Transformation;
                Debug.WriteLine($"Model: {new Vector3(mv.M41, mv.M42, mv.M43)} ");
                Debug.WriteLine($"Center: {cube2.Center} ");
            }

            if (numericUpDown1.Value == 3)
            {
                center = cube0.Center;
            }


            //Debug.WriteLine($"C: {center}, A:{value}");


            //cube.RotateCube(value, center, Axis.Y);
            //cube2.RotateCube(value, center, Axis.Y);
            ////cube2.RotateCube(value, new Vector3(cube2.Model.M41, cube2.Model.M42, cube2.Model.M43), Axis.Y);
            //cube0.RotateCube(value, center, Axis.Y);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            float angle = (float)trackBar1.Value;
            Matrix4 c = cube.Model * cube.Transformation;
            Matrix4 c2 = cube2.Model * cube2.Transformation;
            Matrix4 c0 = cube0.Model * cube0.Transformation;
            if (numericUpDown1.Value == 1)
            {
                cube.RotateCenter(angle, cube.Center);
                //cube.UpdateCenter();
            }

            if (numericUpDown1.Value == 2)
            {
                //cube2.RotateCenter(angle, cube.Center);
                //cube2.UpdateCenter();
                trackBar2.Value = trackBar1.Value;
                trackBar1.Value = 0;
                cube2.UpdateBaseModel();
                Matrix4 mv = cube2.Model * cube2.Transformation;
                Debug.WriteLine($"Model: {new Vector3(mv.M41, mv.M42, mv.M43)} ");
                Debug.WriteLine($"Center: {cube2.Center} ");
                

            }

            if (numericUpDown1.Value == 3)
            {
                cube0.RotateCenter(angle, cube2.Center);
                //cube0.UpdateCenter();

            }
            Debug.WriteLine($"------------------------");
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            float value1  = (float)trackBar1.Value;
            float value2  = (float)trackBar2.Value;

            Vector3 center = Vector3.Zero;
            if (numericUpDown1.Value == 3)
            {

                center = cube2.Center;
                marker.SetPosition(center);
                cube2.RotateCube(value2, center, Axis.Y);
                cube0.RotateCube(value2, center, Axis.Y);

                Matrix4 mv = cube2.Model * cube2.Transformation;
                Debug.WriteLine($"Model: {new Vector3(mv.M41, mv.M42, mv.M43)} ");
                Debug.WriteLine($"Center: {cube2.Center} ");
            }
        }
    }
}
