using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class Edge<Key, VertexData, EdgeData>
    {
        public Vertex<Key, VertexData, EdgeData> StartVertex { get; set; }
        public Vertex<Key, VertexData, EdgeData> EndVertex { get; set; }
        public EdgeData Data { get; }

        public Edge(Vertex<Key, VertexData, EdgeData> startVertex, Vertex<Key, VertexData, EdgeData> endVertex, EdgeData data)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Data = data;
        }

        public override string ToString()
        {
            return "Edge from " + StartVertex._Key + " to " + EndVertex._Key;
        }
    }
}
