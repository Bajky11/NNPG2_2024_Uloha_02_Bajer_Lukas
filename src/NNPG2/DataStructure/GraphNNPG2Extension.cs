using NNPG2_2024_Uloha_02_Bajer_Lukas.Properties;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.DataStucture.GenericGraphExtension;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class GraphNNPG2Extension : GraphRailwayExtension<int, VertexData, EdgeData>
    {

        public List<List<int>> allPaths = new List<List<int>>();
        public List<int> selectedPath = new List<int>();

        public void AddVertex(VertexNNPG2 vertex)
        {
            vertex.Data.SetName(vertex._Key.ToString());
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
            if (keys.Count() == 0)
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
            Pen pen = new Pen(Color.Black, 3);
            int counter = 0;

            foreach (var kvp in this)
            {
                var vertexData = kvp.Value.Data;

                foreach (var edge in kvp.Value.Edges)
                {
                    var startVertex = edge.StartVertex;
                    var endVertex = edge.EndVertex;
                    var startVertexData = startVertex.Data;
                    var endVertexData = endVertex.Data;
                    int halfVertexSize = edge.EndVertex.Data.GetWidth() / 2;
                    int directionPointSize = 10;

                    Point start = new Point(startVertexData.GetCenterX(), startVertexData.GetCenterY());
                    Point end = new Point(endVertexData.GetCenterX(), endVertexData.GetCenterY());
                    Point directionPoint = CalculateRectangleStartPoint(start, end, halfVertexSize + directionPointSize / 2);
                    Point centerDirectionPoint = new Point(directionPoint.X - directionPointSize / 2, directionPoint.Y - directionPointSize / 2);

                    if (selectedPath.Count > 0 && AreAdjacent(startVertex._Key, endVertex._Key))
                    {
                        pen = new Pen(Color.Blue, 5);
                    }
                    else
                    {
                        pen = new Pen(Color.Black, 3);
                    }

                    g.DrawLine(pen, start, directionPoint);


                    g.FillEllipse(Brushes.Red, centerDirectionPoint.X, centerDirectionPoint.Y, directionPointSize, directionPointSize);
                }
                
                
                vertexData.Draw(g);
                counter++;
            }
        }

        public bool AreAdjacent( int start, int end)
        {
            int startIndex = this.selectedPath.IndexOf(start);
            int endIndex = this.selectedPath.IndexOf(end);

            if (startIndex == -1 || endIndex == -1)
                return false;

            return Math.Abs(startIndex - endIndex) == 1;
        }



        private Point CalculateRectangleStartPoint(Point start, Point end, int distance)
        {
            float lineLength = (float)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));

            if (lineLength == 0) return end;

            float ratio = distance / lineLength;

            int directionPointX = (int)(end.X + ratio * (start.X - end.X));
            int directionPointY = (int)(end.Y + ratio * (start.Y - end.Y));

            return new Point(directionPointX, directionPointY);
        }
    }
}
