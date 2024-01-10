using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;
using System.Windows.Forms;
using System;
using System.IO;

namespace RobotSimulatorApp
{
    public partial class Form1 : Form
    {
        int VertexBufferObject;
        int VertexArrayObject;
        private Shader? shader;
        private Timer timer = null!;

        float[] vertices = {
                 -0.5f, -0.5f, 0.0f, //Bottom-left vertex
                 0.5f, -0.5f, 0.0f, //Bottom-right vertex
                 0.0f,  0.5f, 0.0f  //Top vertex
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

            shader = new Shader();
            shader.Use();

            GL.EnableVertexAttribArray(0); //enables vertex
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        }

        public void Render()
        {
            glControl.MakeCurrent();

            GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            glControl.SwapBuffers();
        }

        public void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (sender != null) { 
            glControl.MakeCurrent();

            GL.ClearColor(Color4.MidnightBlue);
            //GL.ClearColor(new Color4(0, 44, 25, 255));

            GL.Clear(ClearBufferMask.ColorBufferBit);

            glControl.SwapBuffers();
            }
        }
    }
}
