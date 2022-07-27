using System.Collections.Generic;

namespace Kroton.Models
{
    public class KrotonModel
    {
        public List<float> Vertices;
        public List<float> Normals;
        public List<float> TexCoords;
        public List<int> Indices;
        public List<float> Colors;

        public List<float> GetGLVertices()
        {
            var list = new List<float>();
            
            
            /* Vertex Array
            Px Py Pz Nx Ny Nz Cr Cg Cb
            */
            for (int i = 0; i < Vertices.Count; i += 3)
            {
                list.Add(Vertices[i]);
                list.Add(Vertices[i + 1]);
                list.Add(Vertices[i + 2]);

                if (Normals.Count > 0)
                {
                    list.Add(Normals[i]);
                    list.Add(Normals[i + 1]);
                    list.Add(Normals[i + 2]);
                }
                else
                {
                    list.Add(0);
                    list.Add(0);
                    list.Add(0);
                }

                if (Colors.Count > 0)
                {
                    list.Add(Colors[i]);
                    list.Add(Colors[i + 1]);
                    list.Add(Colors[i + 2]);
                }
                else
                {
                    list.Add(1);
                    list.Add(1);
                    list.Add(1);
                }
            }

            return list;
        }
    }
}