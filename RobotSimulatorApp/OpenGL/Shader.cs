using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.IO;

namespace RobotSimulatorApp.GlConfig
{
    public class Shader
    {
        public int Handle { get; set; }

        public Shader(string vertexShaderString, string fragmentShaderString)
        { 
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderString);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderString);

            CompileShader(vertexShader);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);

            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetInt(string name, int value)
        {
            GL.UseProgram(Handle);

            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            GL.UseProgram(Handle);

            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            //TODO
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}