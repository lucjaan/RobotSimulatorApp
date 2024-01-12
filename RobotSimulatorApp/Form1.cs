using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RobotSimulatorApp.GlConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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

        private static readonly int[] IndexData = new int[]
        {
             0,  1,  2,  2,  3,  0,
             4,  5,  6,  6,  7,  4,
             8,  9, 10, 10, 11,  8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20,
        };

        //uint[] indices = {  // note that we start from 0!
        //    0, 1, 2,   // first triangle
        //   2, 1, 3    // second triangle
        //};

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

            Cube cube = new Cube("cube1", new Vector3(-1f, -1f, -1f), 2f, 2f, 2f);
            Vector3[] verticesC = cube.ReturnInternalVectors();

            timer = new Timer();
            timer.Tick += (sender, e) =>
            {
                Render();
            };
            timer.Interval = 100;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            timer.Start();

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesC.Length * sizeof(float), verticesC, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //Matrix4 model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));
            //creating a scene behind on z so we can see everything as origin point is 0,0,0
            //Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //creating perspective
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0f, 1f, 0.0f), MathHelper.DegreesToRadians(70f));
            Matrix4 view = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            float aspect_ratio = Math.Max(glControl.ClientSize.Width, 1) / (float)Math.Max(glControl.ClientSize.Height, 1);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);

            shader = new Shader();
            shader.Use();
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            //GL.EnableVertexAttribArray(0); //enables vertex
            //var vertexLocation = shader.GetAttribLocation("aPosition");
            //GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexLocation); //enables vertex
        }

        public void Render()
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color4.MidnightBlue);

            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.DrawElements(BeginMode.Triangles, IndexData.Length, DrawElementsType.UnsignedInt, 0);



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
