using NNPG2_2024_Uloha_02_Bajer_Lukas;
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
    class CustomObject
    {
        public string Name { get; }
        public Rectangle Rectangle { get; private set; }
        public Rectangle MagneticRectangle { get; set; }
        public bool Hovered { get; set; }
        public bool Selected { get; set; }
        private const int DefaultSize = 50;
        private const int DefaultMagneticSize = 30;

        public CustomObject(string name, int x, int y, int width = DefaultSize, int height = DefaultSize)
        {
            Name = name;
            Rectangle = new Rectangle(x, y, width, height);
            UpdateMagneticRectangle();
        }

        private void UpdateMagneticRectangle()
        {
            MagneticRectangle = new Rectangle(
                Rectangle.X - DefaultMagneticSize / 2,
                Rectangle.Y - DefaultMagneticSize / 2,
                Rectangle.Width + DefaultMagneticSize,
                Rectangle.Height + DefaultMagneticSize
            );
        }

        public void SetRectX(int x)
        {
            Rectangle = new Rectangle(x, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            UpdateMagneticRectangle();
        }

        public void SetRectY(int y)
        {
            Rectangle = new Rectangle(Rectangle.X, y, Rectangle.Width, Rectangle.Height);
            UpdateMagneticRectangle();
        }

        public void SetRectXY(int x, int y)
        {
            Rectangle = new Rectangle(x, y, Rectangle.Width, Rectangle.Height);
            UpdateMagneticRectangle();
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, Rectangle);
            g.DrawEllipse(Pens.Gray, MagneticRectangle);
            g.DrawString(Name, new Font("Arial", 10), Brushes.White, Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);

            if (Selected)
            {
                using (Pen pen = new Pen(Color.Red) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    g.DrawEllipse(pen, Rectangle);
                }
            }
            if (Hovered)
            {
                g.DrawEllipse(Pens.Yellow, Rectangle);
            }
        }
    }

    class Edge
    {
        public CustomObject Start { get; set; }
        public CustomObject End { get; set; }

        public Edge(CustomObject start = null, CustomObject end = null)
        {
            Start = start;
            End = end;
        }

        public bool IsComplete() => Start != null && End != null;

        public void Reset()
        {
            Start = null;
            End = null;
        }
    }

    public partial class Form1 : Form
    {
        private List<Edge> edges = new List<Edge>();
        private List<CustomObject> objects = new List<CustomObject>();
        private bool isDragging = false;
        private Point lastMousePosition;
        private Point mousePosition;
        private Point edgeEndPosition;
        private CustomObject hoveredObject = null;
        private CustomObject newObject = null;
        private Edge edge = new Edge();
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
            newObject = new CustomObject("temp", mousePosition.X, mousePosition.Y);
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
            objects.Add(new CustomObject("1", 50, 50));
            objects.Add(new CustomObject("2", 150, 150));
            objects.Add(new CustomObject("3", 250, 250));
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
                else
                {
                    edge.End = hoveredObject;
                }

                if (edge.IsComplete())
                {
                    edges.Add(new Edge(edge.Start, edge.End));
                    edge.Reset();
                    this.Invalidate();
                }
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
                    new Point(
                        edge.Start.Rectangle.X + edge.Start.Rectangle.Width / 2,
                        edge.Start.Rectangle.Y + edge.Start.Rectangle.Height / 2
                        ),
                    new Point(
                        edge.End.Rectangle.X + edge.End.Rectangle.Width / 2,
                        edge.End.Rectangle.Y + edge.End.Rectangle.Width / 2
                        )
                    );
            }

            if (edge.Start != null)
            {
                g.DrawLine(
                    new Pen(Color.Blue),
                    new Point(
                        edge.Start.Rectangle.X + edge.Start.Rectangle.Width / 2,
                        edge.Start.Rectangle.Y + edge.Start.Rectangle.Width / 2
                        ),
                    new Point(edgeEndPosition.X, edgeEndPosition.Y)
                );
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            edgeEndPosition = e.Location;

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
                        if (hoveredObject != obj)
                        {
                            hoveredObject = obj;
                            hoveredObject.Hovered = true;
                            this.Invalidate();
                            break;
                        }
                    }
                    else
                    {
                        if (hoveredObject == obj)
                        {
                            hoveredObject.Hovered = false;
                            hoveredObject = null;
                            this.Invalidate();
                        }
                    }

                }
            }

            if (mode == "add")
            {
                newObject = new CustomObject("temp", mousePosition.X, mousePosition.Y);
                this.Invalidate();
            }

            if (edge.Start != null)
            {
                this.Invalidate();
            }
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
