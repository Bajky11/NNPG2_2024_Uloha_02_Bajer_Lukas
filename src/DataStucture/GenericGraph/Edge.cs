using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class Edge<Key, VertexData, EdgeData>
    {
        public Vertex<Key, VertexData, EdgeData> StartVertex { get; }
        public Vertex<Key, VertexData, EdgeData> EndVertex { get; }
        public EdgeData Data { get; }

        public Edge(Vertex<Key, VertexData, EdgeData> startVertex, Vertex<Key, VertexData, EdgeData> endVertex, EdgeData data)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Data = data;
        }
    }
}
