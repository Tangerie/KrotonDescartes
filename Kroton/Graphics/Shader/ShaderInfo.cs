using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenTK.Graphics.OpenGL4;

namespace Kroton.Graphics.Shader
{
    public class ShaderInfo
    {
        public readonly int Handle;
        
        private Dictionary<string, int> _uniforms;
        private Dictionary<string, int> _attributes;

        public ReadOnlyDictionary<string, int> Uniforms
        {
            get
            {
                if (_uniforms == null)
                {
                    InitUniforms();
                }

                return new ReadOnlyDictionary<string, int>(_uniforms);
            }
        }
        
        public ReadOnlyDictionary<string, int> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    InitAttributes();
                }

                return new ReadOnlyDictionary<string, int>(_attributes);
            }
        }

        public ShaderInfo(int handle)
        {
            Handle = handle;
        }

        private void InitUniforms()
        {
            _uniforms = new Dictionary<string, int>();
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var num_uniforms);
            
            Console.WriteLine($"{num_uniforms} Uniforms in Program");
            
            for (int i = 0; i < num_uniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);

                _uniforms.Add(key, location);
                Console.WriteLine($"[Shader] {key}: {location}");
            }
        }

        private void InitAttributes()
        {
            _attributes = new Dictionary<string, int>();
            GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out var num_attribs);
            
            Console.WriteLine($"{num_attribs} Attributes in Program");
            
            for (int i = 0; i < num_attribs; i++)
            {
                var key = GL.GetActiveAttrib(Handle, i, out _, out _);
                var location = GL.GetAttribLocation(Handle, key);

                _attributes.Add(key, location);
                Console.WriteLine($"[Shader] {key}: {location}");
            }
        }
    }
}