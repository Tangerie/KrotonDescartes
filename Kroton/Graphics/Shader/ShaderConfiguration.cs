using System.Collections.Generic;

/*
==== Standard Uniforms ====
uniform vec3 iResolution;
uniform float iTime;
uniform float iTimeDelta;
uniform float iFrame;
uniform vec4 iMouse;
*/

namespace Kroton.Graphics.Shader
{
    public enum ShaderUniform
    {
        Resolution,
        Time,
        TimeDelta,
        Frame,
        Mouse
    }

    public enum ShaderAttribute
    {
        Position,
        Color,
        Normal
    }
    
    public struct ShaderConfiguration
    {
        public Dictionary<ShaderUniform, string> Uniforms;
        public Dictionary<ShaderAttribute, string> Attributes;
        public string VertexShaderPath;
        public string FragmentShaderPath;
    }
}