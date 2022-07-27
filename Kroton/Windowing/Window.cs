using System;
using System.Diagnostics;
using Kroton.Graphics.Shader;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Kroton.Windowing
{
    public class KrotonWindow : GameWindow
    {
        private WindowSettings _settings;
        
        private KrotonWindow(GameWindowSettings wSettings, NativeWindowSettings nSettings) : base(wSettings, nSettings) {}

        public static KrotonWindow Create(WindowSettings settings)
        {
            var w = new KrotonWindow(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Flags = ContextFlags.ForwardCompatible,
                Title = settings.Title,
                Size = settings.BaseResolution,

            });

            w._settings = settings;
            
            return w;
        }
        
        private int _vertexBufferObject;

        private int _vertexArrayObject;
        
        private ShaderProgram _shader;
        
        private int _elementBufferObject;
        
        private Stopwatch _timer;
        private int _frame;
        private double _lastMillis;
        

        public void SetShader(ShaderProgram s)
        {
            _shader = s;
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            _shader.Use();
            
            _timer = new Stopwatch();
            _timer.Start();

            _frame = 0;
            _lastMillis = _timer.Elapsed.TotalMilliseconds;
        }

        private void SetAttributes()
        {
            if (_shader.HasAttribute(ShaderAttribute.Position))
            {
                GL.VertexAttribPointer(_shader.GetAttributeLocation(ShaderAttribute.Position), 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
                //Enable var 0 in shader
                GL.EnableVertexAttribArray(_shader.GetAttributeLocation(ShaderAttribute.Position));
            }
            
            if (_shader.HasAttribute(ShaderAttribute.Normal))
            {
                GL.VertexAttribPointer(_shader.GetAttributeLocation(ShaderAttribute.Normal), 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
                //Enable var 0 in shader
                GL.EnableVertexAttribArray(_shader.GetAttributeLocation(ShaderAttribute.Normal));
            }
            
            if (_shader.HasAttribute(ShaderAttribute.Color))
            {
                GL.VertexAttribPointer(_shader.GetAttributeLocation(ShaderAttribute.Color), 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
                GL.EnableVertexAttribArray(_shader.GetAttributeLocation(ShaderAttribute.Color));
            }
        }

        private void SetUniforms()
        {
            if (_shader.HasUniform(ShaderUniform.Resolution))
            {
                GL.Uniform2(_shader.GetUniformLocation(ShaderUniform.Resolution), (float)(Size.X * _settings.PixelDensityMult), (float)(Size.Y * _settings.PixelDensityMult));
            }

            if (_shader.HasUniform(ShaderUniform.Time))
            {
                GL.Uniform1(_shader.GetUniformLocation(ShaderUniform.Time), _timer.Elapsed.TotalMilliseconds);
            }
            
            if (_shader.HasUniform(ShaderUniform.TimeDelta))
            {
                GL.Uniform1(_shader.GetUniformLocation(ShaderUniform.TimeDelta), _timer.Elapsed.TotalMilliseconds - _lastMillis);
            }
            
            if (_shader.HasUniform(ShaderUniform.Frame))
            {
                GL.Uniform1(_shader.GetUniformLocation(ShaderUniform.Frame), _frame);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            SetUniforms();
            
            _shader.Use();
            
            SwapBuffers();
            _frame++;
            _lastMillis = _timer.Elapsed.TotalMilliseconds;
        }

        // This function runs on every update frame.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }

            base.OnUpdateFrame(e);
        }
        
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            Console.WriteLine("Resized {0}", Size * _settings.PixelDensityMult);

            // When the window gets resized, we have to call GL.Viewport to resize OpenGL's viewport to match the new size.
            // If we don't, the NDC will no longer be correct.
            GL.Viewport(0, 0, Size.X * _settings.PixelDensityMult, Size.Y * _settings.PixelDensityMult);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            
            _shader.Delete();
            
            base.OnUnload();
        }

        public void SetFullscren(bool fullscreen)
        {
            WindowState = fullscreen ? WindowState.Fullscreen : WindowState.Normal;
        }
    }
}