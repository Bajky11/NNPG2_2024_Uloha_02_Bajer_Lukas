using NNPG2_2024_Uloha_02_Bajer_Lukas;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace NNPG2_2024_Uloha_02_Bajer_Lukas
{
    public partial class Form1 : Form
    {
        private GraphNNPG2Extension graph = new GraphNNPG2Extension();
        private List<EdgeData> edges = new List<EdgeData>();
        private bool isDragging = false;
        private Point lastMousePosition;
        private Point mousePosition;
        private VertexNNPG2 hoveredObject = null;
        private VertexNNPG2 magneticObject = null;
        private VertexNNPG2 newObject = null;
        private EdgeNNPG2 edge = null;
        private string mode = "view";
        private Map _Map;
        private int StartPathKey = -1;
        private int EndPathKey = -1;
        private int keyCounter;

        private int edgeDeleteFirst = -1;
        private int edgeDeleteSecond = -1;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.panel1.MouseMove += OnMouseMove;
            this.panel1.Paint += OnPaint;
            this.panel1.MouseDown += OnMouseDown;
            this.panel1.MouseUp += OnMouseUp;
            this.panel1.KeyPress += OnKeyPress;


            // Set DoubleBuffering true on panel1
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
             | BindingFlags.Instance | BindingFlags.NonPublic, null,
             panel1, new object[] { true });

            panel1.Focus();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape: Reset(); break;
                case 'a': AddNewObject(); break;
                case 's': Save(); break;
                case 'd': DeleteObject(); break;
                case 'e': DeleteEdge(); break;
            }
        }

        private void Save()
        {
            graph.Save("NNPG2.txt", _Map);
            MessageBox.Show("Saved");
        }

        private void DeleteEdge()
        {
            SetMode("edgeDel");
        }

        private void SetComboboxes()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            foreach (var kvp in graph)
            {
                var vertex = kvp.Value;
                comboBox1.Items.Add(vertex._Key);
                comboBox2.Items.Add(vertex._Key);
            }
        }

        private void UpdateAllPaths()
        {
            bool pathIsComplete = StartPathKey != -1 && EndPathKey != -1;
            if (pathIsComplete)
            {
                Console.WriteLine("path is complete");
                List<List<int>> allPaths = graph.FindAllPathsBetweenTwoVertexes(StartPathKey, EndPathKey);
                if (allPaths.Count > 0)
                {
                    graph.allPaths = allPaths;
                    DisplayAllPaths();

                    foreach (List<int> path in allPaths)
                    {
                        foreach (int vertexKey in path)
                        {
                            Console.Write(vertexKey + " -> ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    flowLayoutPanel1.Controls.Clear();
                    graph.allPaths.Clear();
                    graph.selectedPath.Clear();
                }


            }
        }

        private void DisplayAllPaths()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (List<int> path in graph.allPaths)
            {
                System.Windows.Forms.Button pathButton = new System.Windows.Forms.Button();
                StringBuilder pathText = new StringBuilder();

                for (int i = 0; i < path.Count; i++)
                {
                    pathText.Append(path[i]);
                    if (i < path.Count - 1) pathText.Append(" -> ");
                }

                pathButton.Text = pathText.ToString();
                pathButton.Tag = path;
                pathButton.Click += PathButton_Click;
                pathButton.BackColor = Color.White;
                pathButton.Width = flowLayoutPanel1.Width - 5;
                flowLayoutPanel1.Controls.Add(pathButton);
            }
        }

        private void PathButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button clickedButton = sender as System.Windows.Forms.Button;
            if (clickedButton != null && clickedButton.Tag is List<int> path)
            {
                graph.selectedPath = new List<int>(path);
                Console.WriteLine("Selected Path: " + string.Join(" -> ", graph.selectedPath));
            }
            PanelInvalidate();
        }


        private void PanelInvalidate()
        {
            this.panel1.Invalidate();
        }

        private void DeleteObject()
        {
            if (hoveredObject != null)
            {
                graph.RemoveVertex(hoveredObject._Key);
                hoveredObject = null;
                PanelInvalidate();
                SetComboboxes();
            }
        }

        private void AddNewObject()
        {
            SetMode("add");
            newObject = new VertexNNPG2(keyCounter++, new VertexData("new", mousePosition.X, mousePosition.Y));
        }

        private void SetMode(string mode)
        {
            this.mode = mode;
        }

        private void Reset()
        {
            ResetEdgeCreation();
            ResetAddNewObject();
        }

        private void ResetEdgeCreation()
        {
            edge = null;
            PanelInvalidate();
        }

        private void ResetAddNewObject()
        {
            mode = "view";
            newObject = null;
            PanelInvalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lastMousePosition = Point.Empty;
            Point mapInitialCoordinates = graph.Load("NNPG2.txt");
            _Map = new Map("czechrepublic.png", mapInitialCoordinates.X, mapInitialCoordinates.Y);
            keyCounter = graph.GetfirstValidKey();
            SetComboboxes();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            switch (e.Button)
            {
                case MouseButtons.Right: OnMouseUpButtonRight(e); break;
                case MouseButtons.Left: OnMouseUpButtonLeft(e); break;
            }

            panel1.Focus();
        }

        private void OnMouseUpButtonRight(MouseEventArgs e)
        {
            if (hoveredObject != null)
            {
                if (edge == null)
                {
                    // edge is null or start vertex is different - set edge and start vertex
                    edge = new EdgeNNPG2(new VertexNNPG2(hoveredObject._Key, hoveredObject.Data), null, new EdgeData(""));
                }
            }

            if (magneticObject != null)
            {
                if (edge != null && magneticObject._Key != edge.StartVertex._Key)
                {
                    if (edge.StartVertex != null)
                    {
                        edge.EndVertex = new VertexNNPG2(magneticObject._Key, magneticObject.Data);
                    }
                }
            }

            if (edge != null && edge.IsComplete())
            {
                graph.AddEdge(edge.StartVertex._Key, edge.EndVertex._Key, new EdgeData("edge from" + edge.StartVertex._Key + " to " + edge.EndVertex._Key));
                this.edge = null;
                PanelInvalidate();
            }
        }

        private void OnMouseUpButtonLeft(MouseEventArgs e)
        {
            if (mode == "add")
            {
                graph.AddVertex(newObject);
                newObject = null;
                SetMode("view");
                SetComboboxes();
            }

            if (mode == "edgeDel")
            {
                Console.WriteLine("edgeDel");
                Console.WriteLine(edgeDeleteFirst);
                Console.WriteLine(edgeDeleteSecond);
                if (edgeDeleteFirst == -1)
                {
                    Console.WriteLine("Set first");
                    edgeDeleteFirst = hoveredObject._Key;
                }
                else if (edgeDeleteSecond == -1)
                {
                    if (hoveredObject._Key != edgeDeleteFirst)
                    {
                        Console.WriteLine("Set second");
                        edgeDeleteSecond = hoveredObject._Key;
                    }
                }

                if (edgeDeleteFirst != -1 && edgeDeleteSecond != -1)
                {
                    Console.WriteLine("Removed edge");
                    graph.RemoveEdge(edgeDeleteFirst, edgeDeleteSecond);
                    edgeDeleteFirst = -1;
                    edgeDeleteSecond = -1;
                    SetMode("view");
                    PanelInvalidate();
                }
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastMousePosition = e.Location;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            _Map.Draw(g);
            graph.Draw(g);

            if (mode == "add")
            {
                newObject.Data.Draw(g);
            }



            if (edge != null && edge.StartVertex != null)
            {
                Point endEdgePoint = mousePosition;
                if (magneticObject != null)
                {
                    endEdgePoint = new Point(magneticObject.Data.GetCenterX(), magneticObject.Data.GetCenterY());
                }

                g.DrawLine(
                    new Pen(Color.Blue),
                    new Point(edge.StartVertex.Data.GetCenterX(), edge.StartVertex.Data.GetCenterY()),
                    new Point(endEdgePoint.X, endEdgePoint.Y)
                    );
            }
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            bool foundMagnetic = false;
            bool foundRectangle = false;

            if (mode == "view" && isDragging)
            {
                OnDrag(e);
            }
            else
            {
                foreach (var obj in graph)
                {
                    var vertexData = obj.Value.Data;
                    var vertex = obj.Value;

                    if (vertexData.RectangleContains(mousePosition))
                    {
                        foundRectangle = true;

                        if (hoveredObject == null)
                        {
                            // vertex je null - nastva hover object
                            hoveredObject = new VertexNNPG2(vertex._Key, vertexData);
                            hoveredObject.Data.SetHovered(true);
                            PanelInvalidate();
                            break;
                        }
                        else if (hoveredObject.Data != vertexData)
                        {
                            // vertex je jiný než hover object - přenastav hover objekt
                            hoveredObject.Data.SetHovered(true);
                            PanelInvalidate();
                            break;
                        }
                        else
                        {
                            // vertex je stejný jako aktuálně nastavený hover objekt - žadná akce
                        }
                    }

                    if (vertexData.MagneticRectangleContains(mousePosition))
                    {
                        foundMagnetic = true;

                        if (magneticObject == null)
                        {
                            // vertex je null - nastav magnetic object
                            magneticObject = new VertexNNPG2(vertex._Key, vertexData);
                            PanelInvalidate();
                            break;

                        }
                        else if (magneticObject.Data != vertexData)
                        {
                            // vertex je jiný než magnetic object - přenastav magnetic objekt
                            magneticObject = new VertexNNPG2(vertex._Key, vertexData);
                            PanelInvalidate();
                            break;
                        }
                        else
                        {
                            // vertex je stejný jako aktuálně nastavený magnetic objekt - žadná akce
                        }
                    }
                }




                if (!foundMagnetic && magneticObject != null)
                {
                    //Console.WriteLine("Err");
                    magneticObject = null;
                    PanelInvalidate();
                }

                if (!foundRectangle && hoveredObject != null)
                {
                    hoveredObject.Data.SetHovered(false);
                    hoveredObject = null;
                    PanelInvalidate();
                }
            }

            if (mode == "add")
            {
                newObject.Data.SetRectXY(mousePosition.X, mousePosition.Y);
                PanelInvalidate();
            }

            if (edge != null && edge.StartVertex != null)
            {
                PanelInvalidate();
            }
        }

        private void OnDrag(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: OnDragButtonLeft(e); break;
                case MouseButtons.Middle: OnDragButtonMiddle(e); break;
                case MouseButtons.Right: OnDragButtonRight(e); break;
            }
            PanelInvalidate();
        }

        private void OnDragButtonLeft(MouseEventArgs e)
        {
            if (hoveredObject != null)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                int newX = hoveredObject.Data.GetX() + deltaX;
                int newY = hoveredObject.Data.GetY() + deltaY;

                hoveredObject.Data.SetRectXY(newX, newY);

                lastMousePosition = e.Location;

            }
        }

        private void OnDragButtonMiddle(MouseEventArgs e)
        {
            int deltaX = e.X - lastMousePosition.X;
            int deltaY = e.Y - lastMousePosition.Y;

            graph.UpdateCoordinates(deltaX, deltaY);
            _Map.UpdateCoordinates(deltaX, deltaY);

            lastMousePosition = e.Location;
        }
        private void OnDragButtonRight(MouseEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            this.panel1.Size = splitContainer1.Size;
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {

        }

        private void HandleCombobox1Change(string value)
        {
            if (value != "")
            {

                if (int.TryParse(value, out int result))
                {
                    StartPathKey = int.Parse(value);
                    Console.WriteLine(StartPathKey);
                    UpdateAllPaths();
                }
                else
                {
                    MessageBox.Show("Hodnota pole Bod A není validní.");
                    comboBox1.Text = "";
                }
            }
        }

        private void HandleCombobox2Change(string value)
        {
            if (value != "")
            {
                if (value != "" && int.TryParse(value, out int result))
                {
                    EndPathKey = int.Parse(value);
                    Console.WriteLine(EndPathKey);
                    UpdateAllPaths();
                }
                else
                {
                    MessageBox.Show("Hodnota pole Bod B není validní.");
                    comboBox2.Text = "";
                }
            }

        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            HandleCombobox1Change(comboBox1.Text);
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            HandleCombobox2Change(comboBox2.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleCombobox1Change(comboBox1.Items[comboBox1.SelectedIndex].ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleCombobox2Change(comboBox2.Items[comboBox2.SelectedIndex].ToString());
        }
    }
}
