using NNPG2_2024_Uloha_02_Bajer_Lukas;
using NNPG2_2024_Uloha_02_Bajer_Lukas.src;
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
        private List<VertexData> objects = new List<VertexData>();
        private bool isDragging = false;
        private Point lastMousePosition;
        private Point mousePosition;
        private VertexData hoveredObject = null;
        private VertexData magneticObject = null;
        private VertexData newObject = null;
        private EdgeData edge = new EdgeData();
        private string mode = "view";

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
            newObject = new VertexData("temp", mousePosition.X, mousePosition.Y);
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
            edge.Reset();
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
            objects.Add(new VertexData("1", 50, 50));
            objects.Add(new VertexData("2", 150, 150));
            objects.Add(new VertexData("3", 250, 250));
            graph.AddVertex(new VertexData("GV1", 100, 200));
            graph.AddVertex(new VertexData("GV2", 200, 200));
            graph.AddEdge(0, 1, new EdgeData());
          
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
                if (edge.Start == null)
                {
                    edge.Start = hoveredObject;
                }
            }

            if (magneticObject != null && magneticObject != edge.Start)
            {
                if (edge.Start != null)
                {
                    edge.End = magneticObject;
                }
            }

            if (edge.IsComplete())
            {
                edges.Add(new EdgeData(edge.Start, edge.End));
                edge.Reset();
                this.Invalidate();
            }
        }

        private void OnMouseUpButtonLeft(MouseEventArgs e)
        {
            if (mode == "add")
            {
                objects.Add(newObject);
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

            if (mode == "add")
            {
                newObject.Draw(g);
            }

            foreach (var obj in objects)
            {
                obj.Draw(g);
            }

            foreach (var edge in edges)
            {
                g.DrawLine(
                    new Pen(Color.Black, 3),
                    new Point(edge.Start.GetCenterX(), edge.Start.GetCenterY()),
                    new Point(edge.End.GetCenterX(), edge.End.GetCenterY())
                    );
            }

            if (edge.Start != null)
            {
                Point endEdgePoint = mousePosition;
                if (magneticObject != null)
                {
                    endEdgePoint = new Point(magneticObject.GetCenterX(), magneticObject.GetCenterY());
                }

                g.DrawLine(
                    new Pen(Color.Blue),
                    new Point(edge.Start.GetCenterX(), edge.Start.GetCenterY()),
                    new Point(endEdgePoint.X, endEdgePoint.Y)
                    );
            }
            graph.Draw(g);
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
                foreach (var obj in objects)
                {
                    if (obj.Rectangle.Contains(mousePosition))
                    {
                        foundRectangle = true;

                        if (hoveredObject != obj)
                        {
                            hoveredObject = obj;
                            hoveredObject.Hovered = true;
                            this.Invalidate();
                            break;
                        }
                    }

                    if (obj.MagneticRectangle.Contains(mousePosition))
                    {
                        foundMagnetic = true;

                        if (magneticObject != obj)
                        {
                            magneticObject = obj;
                            this.Invalidate();
                            break;
                        }
                    }
                }

                if (!foundMagnetic && magneticObject != null)
                {
                    Console.WriteLine("Err");
                    magneticObject = null;
                    this.Invalidate();
                }

                if (!foundRectangle && hoveredObject != null)
                {
                    hoveredObject.Hovered = false;
                    hoveredObject = null;
                    this.Invalidate();
                }
            }

            if (mode == "add")
            {
                newObject = new VertexData("temp", mousePosition.X, mousePosition.Y);
                this.Invalidate();
            }

            if (edge.Start != null)
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

                int newX = hoveredObject.Rectangle.X + deltaX;
                int newY = hoveredObject.Rectangle.Y + deltaY;

                hoveredObject.SetRectXY(newX, newY);

                lastMousePosition = e.Location;

            }
        }

        private void OnDragButtonMiddle(MouseEventArgs e)
        {
            int deltaX = e.X - lastMousePosition.X;
            int deltaY = e.Y - lastMousePosition.Y;

            foreach (var obj in objects)
            {

                int newX = obj.Rectangle.X + deltaX;
                int newY = obj.Rectangle.Y + deltaY;
                obj.SetRectXY(newX, newY);

            }

            lastMousePosition = e.Location;
        }
        private void OnDragButtonRight(MouseEventArgs e)
        {

        }
    }
}
