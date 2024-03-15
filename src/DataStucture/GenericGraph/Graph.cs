using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class Graph<Key, VertexData, EdgeData> : IEnumerable<KeyValuePair<Key, Vertex<Key, VertexData, EdgeData>>>, IGenericGraph<Key, VertexData, EdgeData>
    {

        private Dictionary<Key, Vertex<Key, VertexData, EdgeData>> adjencyList { get; }

        public Graph()
        {
            adjencyList = new Dictionary<Key, Vertex<Key, VertexData, EdgeData>>();
        }

        public Dictionary<Key, Vertex<Key, VertexData, EdgeData>> GetAdjencyList()
        {
            return adjencyList;
        }

        public void AddVertex(Key key, VertexData data)
        {
            if (adjencyList.ContainsKey(key))
                throw new InvalidOperationException("Vertex already exists.");
            adjencyList[key] = new Vertex<Key, VertexData, EdgeData>(key, data);
        }

        public void RemoveVertex(Key key)
        {
            if (!adjencyList.ContainsKey(key))
                throw new InvalidOperationException("Vertex does not exist.");

            var vertexToRemove = adjencyList[key];
            adjencyList.Remove(key);

            // Remove this vertex from the neighbors of other vertices and remove the corresponding edges
            foreach (var vertex in adjencyList.Values)
            {
                if (vertex.Neighbors.Contains(vertexToRemove))
                {
                    vertex.RemoveEdge(vertexToRemove);
                }
            }
        }

        public void AddEdge(Key startKey, Key endKey, EdgeData data)
        {
            if (!adjencyList.ContainsKey(startKey) || !adjencyList.ContainsKey(endKey))
                throw new InvalidOperationException("One or both vertices do not exist.");

            var startVertex = adjencyList[startKey];
            var endVertex = adjencyList[endKey];
            startVertex.AddEdge(endVertex, data);
        }

        public void RemoveEdge(Key startKey, Key endKey)
        {
            if (!adjencyList.ContainsKey(startKey) || !adjencyList.ContainsKey(endKey))
                throw new InvalidOperationException("One or both vertices do not exist.");

            adjencyList[startKey].RemoveEdge(adjencyList[endKey]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var vertex in adjencyList.Values)
            {
                sb.AppendLine($"Vertex {vertex._Key} ({vertex.Data}): ");
                foreach (var edge in vertex.Edges)
                {
                    sb.Append($"{edge.EndVertex._Key} ({edge.Data}) ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<Key, Vertex<Key, VertexData, EdgeData>>> GetEnumerator()
        {
            return adjencyList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
