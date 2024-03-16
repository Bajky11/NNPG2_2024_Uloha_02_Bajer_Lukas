using Newtonsoft.Json;
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

        public string Serialize()
        {
            Tuple<Key, Key> vertexKeys = new Tuple<Key, Key>(StartVertex._Key, EndVertex._Key);
            Tuple<Tuple<Key, Key>, EdgeData> obj = new Tuple<Tuple<Key, Key>, EdgeData>(vertexKeys, Data);
            string json = JsonConvert.SerializeObject(obj);
            return "edge" + "\t" + json;
        }

        public static Tuple<Tuple<Key, Key>, EdgeData> Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<Tuple<Tuple<Key, Key>, EdgeData>>(jsonString);
        }
    }
}
