using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class Vertex<Key, VertexData, EdgeData>
    {
        public Key _Key { get; }
        public VertexData Data { get; set; }
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

        public override string ToString()
        {
            return "Vertex " + _Key;
        }

        public string Serialize()
        {
            Tuple<Key, VertexData> obj = new Tuple<Key, VertexData>(_Key, Data);
            string json = JsonConvert.SerializeObject(obj);
            return "vertex" + "\t" + json;
        }

        public static Tuple<Key, VertexData> Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<Tuple<Key, VertexData>>(jsonString);
        }
    }
}
