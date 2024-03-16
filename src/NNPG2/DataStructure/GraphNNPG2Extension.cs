using NNPG2_2024_Uloha_02_Bajer_Lukas.Properties;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class GraphNNPG2Extension : GraphRailwayExtension<int, VertexData, EdgeData>
    {

        public void AddVertex(VertexNNPG2 vertex)
        {
            base.AddVertex(vertex._Key, vertex.Data);
        }

        public void UpdateCoordinates(int deltaX, int deltaY)
        {
            UpdateGraphCoordinates(deltaX, deltaY);
        }

        private void UpdateGraphCoordinates(int deltaX, int deltaY)
        {
            foreach (var obj in this)
            {
                var vertex = obj.Value.Data;

                int newX = vertex.Rectangle.X + deltaX;
                int newY = vertex.Rectangle.Y + deltaY;
                vertex.SetRectXY(newX, newY);

                foreach (var edge in obj.Value.Edges)
                {
                    // update Edges
                }
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var kvp in this)
            {
                //Console.WriteLine($"Vertex Key: {kvp.Key}, Vertex Data: {kvp.Value.Data}");
                kvp.Value.Data.Draw(g);

                foreach (var edge in kvp.Value.Edges)
                {
                    //Console.WriteLine($"  Edge to {edge.EndVertex._Key}, Edge Data: {edge.Data}");
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
