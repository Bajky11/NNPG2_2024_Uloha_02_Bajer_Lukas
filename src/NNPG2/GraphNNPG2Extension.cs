using NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class GraphNNPG2Extension : GraphRailwayExtension<int, VertexData, EdgeData>
    {
        private int VertexKeysCounter;

        public GraphNNPG2Extension()
        {
            VertexKeysCounter = 0;
        }

        public void AddVertex(VertexData data)
        {
            base.AddVertex(VertexKeysCounter++, data);
        }


        public void Draw(Graphics g)
        {
            foreach (var kvp in this)
            {
                Console.WriteLine($"Vertex Key: {kvp.Key}, Vertex Data: {kvp.Value.Data}");
                kvp.Value.Data.Draw(g);

                foreach (var edge in kvp.Value.Edges)
                {
                    Console.WriteLine($"  Edge to {edge.EndVertex._Key}, Edge Data: {edge.Data}");
                    g.DrawLine(
                   new Pen(Color.Black, 3),
                   new Point(edge.StartVertex.Data.GetCenterX(), edge.StartVertex.Data.GetCenterY()),
                   new Point(edge.EndVertex.Data.GetCenterX(), edge.EndVertex.Data.GetCenterY())
                   );
                }
            }
        }
    }
}
