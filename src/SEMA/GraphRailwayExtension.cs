using Newtonsoft.Json;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.SEMA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension
{
    internal class GraphRailwayExtension<Key, VertexData, EdgeData> : Graph<Key, VertexData, EdgeData>
    {
        public Data<Key> data;

        public GraphRailwayExtension(string filePath)
        {
            LoadFromSimplefile(filePath);
        }

        public List<List<Key>> FindAllPathsBetweenStartAndEndVertexes(Key[] startVertexes, Key[] endVertexes, List<List<Key>> crossVertexes)
        {
            List<List<Key>> allPaths = new List<List<Key>>();
            foreach (var startVertex in startVertexes)
            {
                foreach (var endVertex in endVertexes)
                {
                    var allPathsBetweenTwoVertexes = FindAllPathsBetweenTwoVertexes(startVertex, endVertex, crossVertexes);
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

        public List<List<Key>> FindAllPathsBetweenTwoVertexes(Key startVertexKey, Key endVertexKey, List<List<Key>> crossVertexes)
        {
            List<List<Key>> allPaths = new List<List<Key>>();
            HashSet<Key> visited = new HashSet<Key>();
            List<Key> currentPath = new List<Key>();
            FindAllPathsRecursive(GetAdjencyList()[startVertexKey], endVertexKey, visited, currentPath, allPaths, crossVertexes);
            return allPaths;
        }

        private void FindAllPathsRecursive(Vertex<Key, VertexData, EdgeData> currentVertex, Key endVertexKey, HashSet<Key> visited, List<Key> currentPath, List<List<Key>> allPaths, List<List<Key>> crossVertexes)
        {
            visited.Add(currentVertex._Key);
            currentPath.Add(currentVertex._Key);

            if (currentVertex._Key.Equals(endVertexKey))
            {
                //check for cross
                if (IsValidCrossPath(currentPath, crossVertexes))
                {
                    allPaths.Add(new List<Key>(currentPath));
                }
            }
            else
            {
                foreach (var neighbor in currentVertex.Neighbors)
                {
                    if (!visited.Contains(neighbor._Key))
                    {
                        FindAllPathsRecursive(neighbor, endVertexKey, visited, currentPath, allPaths, crossVertexes);
                    }
                }
            }

            visited.Remove(currentVertex._Key);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
        private bool IsValidCrossPath(List<Key> path, List<List<Key>> crossPaths)
        {
            foreach (List<Key> crossPath in crossPaths)
            {
                /*
                Console.WriteLine("\nRULE:");
                foreach (var v in crossPath)
                {
                    Console.Write(v + " -> ");
                }
                Console.WriteLine();

                Console.WriteLine("PATH:");
                foreach (var v in path)
                {
                    Console.Write(v + " -> ");
                }
                */



                for (int j = 0; j < path.Count; j++)
                {
                    if (path.Count == 2)
                    {
                        if (path[j].Equals(crossPath[1]) && path[j + 1].Equals(crossPath[2]))
                        {
                            return true;
                        }
                    }

                    if (path[j].Equals(crossPath[0]))
                    {
                        // zacatek krizovatky
                        //Console.WriteLine("Cross possible " + crossPath[0]);
                        if (path[j + 1].Equals(crossPath[1]))
                        {
                            //krizovatka
                            //Console.WriteLine("Cross begin " + crossPath[1]);
                            if (path[j + 2].Equals(crossPath[2]))
                            {
                                //validni krizovatka
                                // Console.WriteLine("Cross valid " + crossPath[2]);
                                return true;
                            }
                            else
                            {
                                //nevalidni krizovatka
                                //Console.WriteLine("Cross ivalid");
                                return false;
                            }

                        }
                    }
                    else
                    {
                        //Console.WriteLine("no match");
                    }

                }
                //Console.WriteLine();
                //Console.WriteLine("ANOTHER RULE");
            }
            //Console.Write("exited not found down");
            return true;
        }

        public void FindDisjunktMnoziny(List<List<Key>> paths)
        {
            var result = new List<List<List<Key>>>();
            FindDisjunktMnozinyRecursive(paths, result, new List<List<Key>>(), 0);

            // Zpracování výsledků podle potřeby, např. výpis
            Console.WriteLine("Disjunktní množiny cest:");
            foreach (var set in result)
            {/*
                Console.WriteLine("Množina:");
                foreach (var path in set)
                {
                    Console.Write("[ ");
                    foreach (var key in path)
                    {
                        Console.Write($"{key} ");
                    }
                    Console.Write("] ");
                }
                Console.WriteLine(); // Oddělení množin
                */
            }
                Console.WriteLine("Celkem disjunktnich cest: " + result.Count);
            }


            private void FindDisjunktMnozinyRecursive(List<List<Key>> paths, List<List<List<Key>>> result, List<List<Key>> current, int index)
            {
                if (current.Count > 1)
                {
                    result.Add(new List<List<Key>>(current));
                }
                for (int i = index; i < paths.Count; i++)
                {
                    var nextPath = paths[i];
                    if (IsDisjunktWithCurrentPaths(current, nextPath))
                    {
                        current.Add(nextPath);
                        FindDisjunktMnozinyRecursive(paths, result, current, i + 1);
                        current.RemoveAt(current.Count - 1);
                    }
                }
            }

            private bool IsDisjunktWithCurrentPaths(List<List<Key>> currentPaths, List<Key> newPath)
            {
                var newPathSet = new HashSet<Key>(newPath);
                foreach (var existingPath in currentPaths)
                {
                    var existingPathSet = new HashSet<Key>(existingPath);
                    existingPathSet.IntersectWith(newPathSet); // Ponechá v existingPathSet pouze prvky, které jsou také v newPathSet
                    if (existingPathSet.Count > 0)
                    {
                        return false; // Pokud najdeme nějaké společné prvky, cesty nejsou disjunktní
                    }
                }
                return true; // Žádné společné prvky nenalezeny, cesty jsou disjunktní
            }


            public void LoadFromSimplefile(string filePath)
            {
                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist.");
                    return;
                }

                // Read text from the file
                string jsonText = File.ReadAllText(filePath);

                // Deserialize JSON to dynamic object

                data = JsonConvert.DeserializeObject<Data<Key>>(jsonText);


                // Add vertexes to the graph
                foreach (Key vertex in data.Vertices)
                {
                    AddVertex(vertex, default(VertexData));
                }

                // Add edges to the graph
                foreach (var edge in data.Edges)
                {
                    AddEdge(edge[0], edge[1], default(EdgeData));
                }

                // Do something with crossPaths if needed
            }

        }
    }
