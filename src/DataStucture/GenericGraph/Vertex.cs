using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class Vertex<Key, VertexData, EdgeData>
    {
        public Key _Key { get; }
        public VertexData Data { get; }
        public List<Edge<Key, VertexData, EdgeData>> Edges { get; }
        public List<Vertex<Key, VertexData, EdgeData>> Neighbors { get; }

        public Vertex(Key key, VertexData data)
        {
            Edges = new List<Edge<Key, VertexData, EdgeData>>();
            Neighbors = new List<Vertex<Key, VertexData, EdgeData>>();
            this._Key = key;
            Data = data;
        }

        public void AddEdge(Vertex<Key, VertexData, EdgeData> toVertex, EdgeData edgeData)
        {
            Edges.Add(new Edge<Key, VertexData, EdgeData>(this, toVertex, edgeData));
            if (!Neighbors.Contains(toVertex))
            {
                Neighbors.Add(toVertex);
            }
        }

        public void RemoveEdge(Vertex<Key, VertexData, EdgeData> toVertex)
        {
            Edges.RemoveAll(edge => edge.EndVertex.Equals(toVertex));
            Neighbors.Remove(toVertex);
        }
    }
}
