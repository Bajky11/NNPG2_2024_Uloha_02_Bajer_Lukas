using NNPG2_2024_Uloha_02_Bajer_Lukas.Properties;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public int GetfirstValidKey()
        {
            List<int> keys = new List<int>();

            foreach (var kvp in this)
            {
                keys.Add(kvp.Value._Key);
            }
            if(keys.Count() == 0)
            {
                return 1;
            }
            return keys.Max() + 1;

        }

        private void UpdateGraphCoordinates(int deltaX, int deltaY)
        {
            foreach (var obj in this)
            {
                var vertex = obj.Value.Data;

                int newX = vertex.GetX() + deltaX;
                int newY = vertex.GetY() + deltaY;
                vertex.SetRectXY(newX, newY);

                foreach (var edge in obj.Value.Edges)
                {
                    // update Edges
                }
            }
        }

        public void Save(string filePath, Map map)
        {
            string mapSettingsFileName = filePath.Split('.')[0] + "_mapSettings" + "." + filePath.Split('.')[1];
            File.WriteAllText(mapSettingsFileName, map.coordinates.X + "\t" + map.coordinates.Y);
            base.Save(filePath);
        }

        public new Point Load(string filePath)
        {
            string mapSettingsFileName = filePath.Split('.')[0] + "_mapSettings" + "." + filePath.Split('.')[1];
            string text = File.ReadAllText(mapSettingsFileName);
            string[] splitText = text.Split('\t');
            base.Load(filePath);
            return new Point(int.Parse(splitText[0]), int.Parse(splitText[1]));
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
