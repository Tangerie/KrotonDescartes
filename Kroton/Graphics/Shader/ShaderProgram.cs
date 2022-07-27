using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using All = OpenTK.Graphics.OpenGL4.All;
using GetProgramParameterName = OpenTK.Graphics.OpenGL4.GetProgramParameterName;
using GL = OpenTK.Graphics.OpenGL4.GL;
using ShaderParameter = OpenTK.Graphics.OpenGL4.ShaderParameter;
using ShaderType = OpenTK.Graphics.OpenGL4.ShaderType;


namespace Kroton.Graphics.Shader
{
    public class ShaderProgram
    {
        public readonly ShaderInfo Info;
        private readonly ShaderConfiguration _config;

        public ShaderProgram(ShaderConfiguration config)
        {
            _config = config;
            var vertexShader = CreateShader(config.VertexShaderPath, ShaderType.VertexShader);
            var fragmentShader = CreateShader(config.FragmentShaderPath, ShaderType.FragmentShader);

            var handle = GL.CreateProgram();
            
            
            
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);
            
            LinkProgram(handle);
            
            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            
            Info = new ShaderInfo(handle);
        }

        private static int CreateShader(string path, ShaderType type)
        {
            var source = File.ReadAllText(path);

            var shader = GL.CreateShader(type);
            
            GL.ShaderSource(shader, source);
            
            CompileShader(shader);

            return shader;
        }
        
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error compiling shader ({shader}): {infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error linking program ({program}): {infoLog}");
            }
        }

        public bool HasUniform(ShaderUniform u)
        {
            return HasUniform(_config.Uniforms.GetValueOrDefault(u, ""));
        }

        public bool HasUniform(string name)
        {
            return Info.Uniforms.ContainsKey(name);
        }

        public int GetUniformLocation(ShaderUniform u)
        {
            return GetUniformLocation(_config.Uniforms[u]);
        }

        public int GetUniformLocation(string name)
        {
            return Info.Uniforms[name];
        }
        
        public bool HasAttribute(ShaderAttribute a)
        {
            return HasAttribute(_config.Attributes.GetValueOrDefault(a, ""));
        }

        public bool HasAttribute(string name)
        {
            return Info.Attributes.ContainsKey(name);
        }

        public int GetAttributeLocation(ShaderAttribute a)
        {
            return GetAttributeLocation(_config.Attributes[a]);
        }

        public int GetAttributeLocation(string name)
        {
            return Info.Attributes[name];
        }


        public void Use()
        {
            GL.UseProgram(Info.Handle);
        }

        public void Delete()
        {
            GL.DeleteProgram(Info.Handle);
        }
    }
}