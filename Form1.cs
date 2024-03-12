using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas
{
    class CustomObject
    {
        public string Name { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public CustomObject(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

    }
    public partial class Form1 : Form
    {
        private List<CustomObject> objects = new List<CustomObject>();
        private CustomObject selectedObject = null;
        private bool isDragging = false;
        private Point lastMousePosition;

        public Form1()
        {
            InitializeComponent();
            this.MouseMove += OnMouseMove;
            this.Paint += OnPaint;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lastMousePosition = Point.Empty;
            objects.Add(new CustomObject("Object1", 50, 50));
            objects.Add(new CustomObject("Object2", 150, 150));
            objects.Add(new CustomObject("Object3", 250, 250));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            this.isDragging = true;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush rectanlgeBrush = new SolidBrush(Color.Black);

            foreach (var obj in objects)
            {
                g.FillRectangle(rectanlgeBrush, new Rectangle(obj.X, obj.Y, 50, 50));    
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                OnDrag(e);
            }
        }

        private void OnDrag(MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left: OnDragButtonLeft(e); break;
                case MouseButtons.Middle: OnDragButtonMiddle(e); break;
                case MouseButtons.Right: OnDragButtonRight(e); break;
            }
        }

        private void OnDragButtonLeft(MouseEventArgs e)
        {
        }
        private void OnDragButtonMiddle(MouseEventArgs e)
        {
            Console.WriteLine("Drag");
        }
        private void OnDragButtonRight(MouseEventArgs e)
        {

        }
    }
}
