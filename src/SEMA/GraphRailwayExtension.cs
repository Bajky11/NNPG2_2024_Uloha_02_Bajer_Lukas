using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension
{
    internal class GraphRailwayExtension<Key, VertexData, EdgeData> : Graph<Key, VertexData, EdgeData>
    {

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

        public List<List<Key>> FindAllPathsBetweenTwoVertexes(Key startVertexKey, Key endVertexKey)
        {
            List<List<Key>> allPaths = new List<List<Key>>();
            HashSet<Key> visited = new HashSet<Key>();
            List<Key> currentPath = new List<Key>();
            FindAllPathsRecursive(GetAdjencyList()[startVertexKey], endVertexKey, visited, currentPath, allPaths);
            return allPaths;
        }

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
    }
}
