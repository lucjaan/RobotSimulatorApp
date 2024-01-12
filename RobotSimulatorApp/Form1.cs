using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RobotSimulatorApp
{
    public partial class Form1 : Form
    {
        int VertexBufferObject;
        int ElementBufferObject;
        int VertexArrayObject;
        private Shader? shader;
        private Timer timer = null!;

        float[] vertices = {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };


        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //glControl.Paint += glControl_Paint;
            SetUpOpenGL();
        }

        public void SetUpOpenGL()
        {
            //glControl.Paint += glControl_Paint;

            GL.Enable(EnableCap.DepthTest);

            timer = new Timer();
            timer.Tick += (sender, e) =>
            {
                //const float DELTA_TIME = 1 / 50f;
                //_angle += 180f * DELTA_TIME;
                Render();
            };
            timer.Interval = 100;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            timer.Start();

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader();
            shader.Use();

            GL.EnableVertexAttribArray(0); //enables vertex
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        }

        public void Render()
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //GL.ClearColor(Color4.MidnightBlue);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Vector3 vector3;
            Debug.WriteLine("TODO");

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            glControl.SwapBuffers();
        }

        public void CreateCube(string name, int baseA, int baseB, int height, int location)
        {
            Debug.WriteLine("TODO");
            //dasdasda
        }

        public void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (sender != null)
            {
                glControl.MakeCurrent();

                GL.ClearColor(Color4.MidnightBlue);
                //GL.ClearColor(new Color4(0, 44, 25, 255));

                GL.Clear(ClearBufferMask.ColorBufferBit);

                glControl.SwapBuffers();
            }
        }
    }
}
