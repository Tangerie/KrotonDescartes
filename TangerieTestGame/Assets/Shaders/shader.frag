#version 330 core

out vec4 outputColor;

uniform vec2 uResolution;

in vec4 vertexColor;

void main()
{
    vec2 uv = gl_FragCoord.xy / uResolution;
    
    outputColor = vec4(uv.xy, 0.0, 1.0);
}