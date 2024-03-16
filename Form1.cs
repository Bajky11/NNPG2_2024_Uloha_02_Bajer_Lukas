using NNPG2_2024_Uloha_02_Bajer_Lukas;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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

        private int keyCounter = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseMove += OnMouseMove;
            this.Paint += OnPaint;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.KeyPress += OnKeyPress;
        }

        
        

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape: Reset(); break;
                case 'a': AddNewObject(); break;
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
            this.Invalidate();
        }

        private void ResetAddNewObject()
        {
            mode = "view";
            newObject = null;
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lastMousePosition = Point.Empty;
            graph.AddVertex(new VertexNNPG2(keyCounter++, new VertexData("V1", 100, 200)));
            graph.AddVertex(new VertexNNPG2(keyCounter++, new VertexData("V2", 300, 200)));
            graph.AddEdge(0, 1, new EdgeData("edge from 0 to 1"));
            _Map = new Map("C:\\Users\\LuBajer\\Documents\\LukasBajer\\Projects\\NNPG2_2024_Uloha_02_Bajer_Lukas\\src\\NNPG2\\Resources\\czechrepublic.png", 0, 0);


        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            switch (e.Button)
            {
                case MouseButtons.Right: OnMouseUpButtonRight(e); break;
                case MouseButtons.Left: OnMouseUpButtonLeft(e); break;
            }
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
                Console.WriteLine("created");
                graph.AddEdge(edge.StartVertex._Key, edge.EndVertex._Key, new EdgeData("edge from" + edge.StartVertex._Key + " to " + edge.EndVertex._Key));
                this.edge = null;
                this.Invalidate();
            }
        }

        private void OnMouseUpButtonLeft(MouseEventArgs e)
        {
            if (mode == "add")
            {
                graph.AddVertex(newObject);
                newObject = null;
                SetMode("view");
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

                    if (vertexData.Rectangle.Contains(mousePosition))
                    {
                        foundRectangle = true;

                        if (hoveredObject == null)
                        {
                            // vertex je null - nastva hover object
                            hoveredObject = new VertexNNPG2(vertex._Key, vertexData);
                            hoveredObject.Data.Hovered = true;
                            this.Invalidate();
                            break;
                        }
                        else if (hoveredObject.Data != vertexData)
                        {
                            // vertex je jiný než hover object - přenastav hover objekt
                            hoveredObject.Data.Hovered = true;
                            this.Invalidate();
                            break;
                        }
                        else
                        {
                            // vertex je stejný jako aktuálně nastavený hover objekt - žadná akce
                        }
                    }

                    if (vertexData.MagneticRectangle.Contains(mousePosition))
                    {
                        foundMagnetic = true;

                        if (magneticObject == null)
                        {
                            // vertex je null - nastav magnetic object
                            magneticObject = new VertexNNPG2(vertex._Key, vertexData);
                            this.Invalidate();
                            break;

                        }
                        else if (magneticObject.Data != vertexData)
                        {
                            // vertex je jiný než magnetic object - přenastav magnetic objekt
                            magneticObject = new VertexNNPG2(vertex._Key, vertexData);
                            this.Invalidate();
                            break;
                        }
                        else
                        {
                            // vertex je stejný jako aktuálně nastavený magnetic objekt - žadná akce
                        }
                    }
                }
                if (magneticObject != null)
                {
                    Console.WriteLine("magnetic " + magneticObject.ToString());
                }

                if (hoveredObject != null)
                {
                    Console.WriteLine("hover " + hoveredObject.ToString());
                }



                if (!foundMagnetic && magneticObject != null)
                {
                    //Console.WriteLine("Err");
                    magneticObject = null;
                    this.Invalidate();
                }

                if (!foundRectangle && hoveredObject != null)
                {
                    hoveredObject.Data.Hovered = false;
                    hoveredObject = null;
                    this.Invalidate();
                }
            }

            if (mode == "add")
            {
                newObject.Data.SetRectXY(mousePosition.X, mousePosition.Y);
                this.Invalidate();
            }

            if (edge != null && edge.StartVertex != null)
            {
                this.Invalidate();
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
            this.Invalidate();
        }

        private void OnDragButtonLeft(MouseEventArgs e)
        {
            if (hoveredObject != null)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                int newX = hoveredObject.Data.Rectangle.X + deltaX;
                int newY = hoveredObject.Data.Rectangle.Y + deltaY;

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
    }
}
