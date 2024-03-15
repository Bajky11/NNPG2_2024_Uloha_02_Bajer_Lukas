using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /*
                    //REMOVE
                    public List<List<Key>> FindAllPathsBetweenStartAndEndVertexes(Key[] startVertexes, Key[] endVertexes)
                    {
                        List<List<Key>> allPaths = new List<List<Key>>();
                        foreach (var startVertex in startVertexes)
                        {
                            foreach (var endVertex in endVertexes)
                            {
                                var allPathsBetweenTwoVertexes = FindAllPathsBetweenTwoVertexes(startVertex, endVertex);
                                foreach (var path in allPathsBetweenTwoVertexes)
                                {
                                    if (path.Count > 1)
                                    {
                                        allPaths.Add(path);
                                    }
                                }
                            }
                        }
                        return allPaths;
                    }


                    //REMOVE
                    public List<List<Key>> FindAllPathsBetweenTwoVertexes(Key startVertexKey, Key endVertexKey)
                    {
                        List<List<Key>> allPaths = new List<List<Key>>();
                        HashSet<Key> visited = new HashSet<Key>();
                        List<Key> currentPath = new List<Key>();
                        FindAllPathsRecursive(adjencyList[startVertexKey], endVertexKey, visited, currentPath, allPaths);
                        return allPaths;
                    }

                    //REMOVE
                    private void FindAllPathsRecursive(Vertex<Key, VertexData, EdgeData> currentVertex, Key endVertexKey, HashSet<Key> visited, List<Key> currentPath, List<List<Key>> allPaths)
                    {
                        visited.Add(currentVertex._Key);
                        currentPath.Add(currentVertex._Key);

                        if (currentVertex._Key.Equals(endVertexKey))
                        {
                            allPaths.Add(new List<Key>(currentPath));
                        }
                        else
                        {
                            foreach (var neighbor in currentVertex.Neighbors)
                            {
                                if (!visited.Contains(neighbor._Key))
                                {
                                    FindAllPathsRecursive(neighbor, endVertexKey, visited, currentPath, allPaths);
                                }
                            }
                        }

                        visited.Remove(currentVertex._Key);
                        currentPath.RemoveAt(currentPath.Count - 1);
                    }
                    */
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
