﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas
{
    internal static class Program
    {
        static int[] startVertices = { 23, 21, 22, 24, 30, 29 };
        static int[] endVertices = { 30, 29, 27, 28 };

        static int[] vertices = { 23, 12, 14, 30, 17, 29, 18, 27, 21, 22, 15, 16, 19, 28, 24, 13 };
        static int[,] edges = new int[,]{
    { 23, 12 },
    { 12, 14 },
    { 14, 30 },
    { 30, 17 },
    { 17, 29 },
    { 29, 18 },
    { 18, 27 },
    { 18, 19 },
    { 21, 14 },
    { 22, 15 },
    { 15, 16 },
    { 16, 17 },
    { 16, 19 },
    { 19, 28 },
    { 24, 13 },
    { 13, 15 }
};

        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            // Create an instance of GraphExtension
            GraphRailwayExtension<int, string, string> graph = new GraphRailwayExtension<int, string, string>();

            foreach(int vertex in vertices)
            {
                graph.AddVertex(vertex, "V");
            }

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph.AddEdge(edges[i, 0], edges[i, 1], "E");
            }

            var allPaths = graph.FindAllPathsBetweenStartAndEndVertexes(startVertices, endVertices);

            Console.WriteLine(graph.ToString());
            Console.WriteLine();

            // Print all paths
            foreach (var path in allPaths)
            {   
                foreach (var vertexKey in path)
                {
                    Console.Write(vertexKey + " -> ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            var paths = graph.FindAllPathsBetweenTwoVertexes(16, 19);
            foreach (var path in paths)
            {
                foreach (var vertexKey in path)
                {
                    Console.Write(vertexKey + " -> ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Paths found: " + allPaths.Count);

            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
