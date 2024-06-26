using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;
using RobotSimulatorApp.Robot.SCARA;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotSimulatorApp
{
    public partial class Form1 : Form
    {
        readonly private float AspectRatio;

        Camera camera;
        Grid grid;
        Trace trace;
        public SCARA_Robot Scara { get; set; }

        private Vector3 prevPosition;
        private float prevPitch;
        private float prevYaw;

        public Form1()
        {
            InitializeComponent();
            AspectRatio = Math.Max(glControl.ClientSize.Width, 1) / (float)Math.Max(glControl.ClientSize.Height, 1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            SetUpOpenGL();

            ControlsForm controlsForm = new ControlsForm(Scara);
            controlsForm.Show();
        }

        private void SetUpOpenGL()
        {
            grid = new Grid(glControl);
            Scara = new SCARA_Robot(glControl, "scara");
            trace = new Trace();
            Scara.RobotMoved += Form_RobotMoved;
            this.Resize += glControl_Resize;

            Timer timer = new();
            timer.Tick += (sender, e) =>
            {
                Render();
            };
            timer.Interval = 50;
            timer.Start();

            glControl_Resize(glControl, EventArgs.Empty);

            camera = new(new Vector3(86f, 97f, 174f), MathHelper.DegreesToRadians(-20f), MathHelper.DegreesToRadians(-115f), AspectRatio);
            UpdateTrackBars();
        }

        private void Render()
        {
            glControl.MakeCurrent();

            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 0.1f, 500f);

            Scara.RenderRobot(camera.View, projection);
            grid.RenderWorld(camera.View, projection);
            if (TraceCheckbox.Checked)
            {
                trace.RenderTrace(camera.View, projection);
            }

            glControl.SwapBuffers();
        }

        private void Form_RobotMoved(object? sender, Vector3 position)
        {
            if (TraceCheckbox.Checked)
            {
                trace.AddToTrace(position);
            }
        }

        private void glControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();

            if (glControl.ClientSize.Height == 0)
            {
                glControl.ClientSize = new Size(glControl.ClientSize.Width, 1);
            }

            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);
        }

        private void SendValuesToCamera(Vector3 position, float pitch, float yaw)
        {
            prevPosition = camera.Position;
            prevPitch = camera.Pitch;
            prevYaw = camera.Yaw;

            camera.Position = position;
            camera.Pitch = pitch;
            camera.Yaw = yaw;
            camera.UpdateVectors();
            UpdateTrackBars();
            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            if (camera == null)
            {
                PositionXTextBox.Text = string.Empty;
                PositionYTextBox.Text = string.Empty;
                PositionZTextBox.Text = string.Empty;
                PitchTextBox.Text = string.Empty;
                YawTextBox.Text = string.Empty;
            }
            else
            {
                PositionXTextBox.Text = PosXTrackBar.Value.ToString();
                PositionYTextBox.Text = PosYTrackBar.Value.ToString();
                PositionZTextBox.Text = PosZTrackBar.Value.ToString();
                PitchTextBox.Text = MathHelper.RadiansToDegrees(camera.Yaw).ToString();
                YawTextBox.Text = MathHelper.RadiansToDegrees(camera.Yaw).ToString();
            }
        }

        private void UpdateTrackBars()
        {
            PosXTrackBar.Value = (int)camera.Position.X;
            PosYTrackBar.Value = (int)camera.Position.Y;
            PosZTrackBar.Value = (int)camera.Position.Z;
            PitchTrackBar.Value = (int)MathHelper.RadiansToDegrees(camera.Pitch);
            YawTrackBar.Value = (int)MathHelper.RadiansToDegrees(camera.Yaw);
        }

        private void PitchTrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Pitch = MathHelper.DegreesToRadians((float)PitchTrackBar.Value);
            camera.UpdateVectors();
            UpdateTextBoxes();
        }

        private void YawTrackBar_Scroll(object sender, EventArgs e)
        {
            camera.Yaw = MathHelper.DegreesToRadians((float)YawTrackBar.Value);
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
            ControlsForm controlsForm = new(Scara);
            if (!controlsForm.Visible)
            {
                controlsForm.Show();
            }
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(86f, 97f, 174f), MathHelper.DegreesToRadians(-20f), MathHelper.DegreesToRadians(-115f));
        }

        private void TopButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(Scara.Center.X, Scara.Height * 2f, Scara.Center.Z), MathHelper.DegreesToRadians(-89f), -MathHelper.PiOver2);
        }

        private void NorthButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(Scara.Center.X, Scara.Height / 2, Scara.Center.Z - (Scara.Radius * 1.1f)), 0f, MathHelper.PiOver2);
        }

        private void SouthButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(Scara.Center.X, Scara.Height / 2, Scara.Center.Z + (Scara.Radius * 1.1f)), 0f, -MathHelper.PiOver2);
        }

        private void EastButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(Scara.Center.X - (Scara.Radius * 1.1f), Scara.Height / 2, Scara.Center.Z), 0f, 0f);
        }

        private void WestButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(new Vector3(Scara.Center.X + (Scara.Radius * 1.1f), Scara.Height / 2, Scara.Center.Z), 0f, MathHelper.Pi);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            SendValuesToCamera(prevPosition, prevPitch, prevYaw);
        }
    }
}
