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
        private int ElementBufferObject;
        private int VertexArrayObject;
        private int PositionBuffer;
        private int ColorBuffer;
        readonly private float AspectRatio;
        private Shader? shader;
        private Timer timer = null!;
        private float _angle = 0.0f;
        INativeInput? NativeInput;
        Rectangle GlControlBounds;
        bool FirstMove = true;
        Camera? camera;
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 position = new Vector3(15f, 5.0f, 30.0f);


        private static readonly int[] IndexData =
        {
             0,  1,  2,  2,  3,  0,
             4,  5,  6,  6,  7,  4,
             8,  9, 10, 10, 11,  8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20,
        };

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

        private static readonly Color4[] ColorData = new Color4[]
{
            Color4.Silver, Color4.Silver, Color4.Silver, Color4.Silver,
            Color4.Honeydew, Color4.Honeydew, Color4.Honeydew, Color4.Honeydew,
            Color4.Moccasin, Color4.Moccasin, Color4.Moccasin, Color4.Moccasin,
            Color4.IndianRed, Color4.IndianRed, Color4.IndianRed, Color4.IndianRed,
            Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed, Color4.PaleVioletRed,
            Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen, Color4.ForestGreen,
};

        private void SetUpOpenGL()
        {
            GL.Enable(EnableCap.DepthTest);

            Cube cube = new ("cube1", new Vector3(-1f, -1f, -1f), 2f, 2f, 2f);

            timer = new Timer();
            timer.Tick += (sender, e) =>
            {
                Render();
                const float DELTA_TIME = 1 / 50f;
                _angle += 180f * DELTA_TIME;
            };
            timer.Interval = 100;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            timer.Start();

            glControl_Resize(glControl, EventArgs.Empty);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, BufferUsageHint.StaticDraw);

            PositionBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * cube.ReturnInternalVectors().Length * sizeof(float), cube.ReturnInternalVectors(), BufferUsageHint.StaticDraw);

            shader = new Shader();
            shader.Use();

            GL.EnableVertexAttribArray(0); //enables vertex
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            ColorBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
        }

        private void Render()
        {

            glControl.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            position.Z = position.Z > 0 ? position.Z : 0;
            position.Z = position.Z < 40 ? position.Z : 40;


            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), MathHelper.DegreesToRadians(_angle));

            Matrix4 view = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 1, 64);

            Matrix4 cameraView = Matrix4.LookAt(position, position + front, Vector3.UnitY);



            shader = new Shader();
            shader.Use();

            camera = new(Vector3.UnitZ * 3, AspectRatio);

            if (captureMouseCheckBox.Checked)
            {
                MoveCamera();
            }

            //var x = camera.GetViewMatrix();
           // var y = camera.GetProjectionMatrix();

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", cameraView);
            //shader.SetMatrix4("view", camera.GetViewMatrix());
            //shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
            //shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);

            glControl.SwapBuffers();
        }

        private void MoveCamera()
        {
            float Yaw = 0;
            float Pitch = 0;
            float Sensitivity = 0.1f;
            INativeInput input = glControl.EnableNativeInput();

            NativeInput.MouseMove += (e) =>
            {
                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Yaw += e.DeltaX * Sensitivity;
                Pitch -= e.DeltaY * Sensitivity; // Reversed since y-coordinates range from bottom to top
                front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
                front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
                front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
                front = Vector3.Normalize(front);
                //Debug.Write($"{camera.Yaw}, {camera.Pitch} \n");
            };

            NativeInput.MouseWheel += (e) =>
            {
                if(e.OffsetY == -1 && position.Z > 0)
                {
                    position.Z -= 0.1f;
                }

                if (e.OffsetY == 1 && position.Z < 40)
                {
                    position.Z += 0.1f;
                }

                Debug.Write(position.Z + "\n");
            };

            NativeInput.KeyUp += (e) =>
            {
                if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Home)
                {
                    front = new Vector3(0.0f, 0.0f, -1.0f);
                }
            };



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
                Cursor.Hide();
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
                Cursor.Show();
                Cursor.Clip = Rectangle.Empty;
                front = new Vector3(0.0f, 0.0f, -1.0f);
                position.Z = 30f;
            }
        }
    }
    
}
