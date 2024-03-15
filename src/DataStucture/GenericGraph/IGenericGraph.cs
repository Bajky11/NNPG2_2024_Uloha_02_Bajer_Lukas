using System.Collections.Generic;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal interface IGenericGraph<Key, VertexData, EdgeData>
    {
        void AddEdge(Key startKey, Key endKey, EdgeData data);
        void AddVertex(Key key, VertexData data);
        Dictionary<Key, Vertex<Key, VertexData, EdgeData>> GetAdjencyList();
        IEnumerator<KeyValuePair<Key, Vertex<Key, VertexData, EdgeData>>> GetEnumerator();
        void RemoveEdge(Key startKey, Key endKey);
        void RemoveVertex(Key key);
        string ToString();
    }
}