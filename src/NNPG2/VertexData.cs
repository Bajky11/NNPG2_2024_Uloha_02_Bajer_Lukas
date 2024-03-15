using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class VertexData
    {
        public string Name { get; }
        public Rectangle Rectangle { get; private set; }
        public Rectangle MagneticRectangle { get; set; }
        public bool Hovered { get; set; }
        public bool Selected { get; set; }
        private const int DefaultSize = 50;
        private const int DefaultMagneticSize = 40;

        public VertexData(string name, int x, int y, int width = DefaultSize, int height = DefaultSize)
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

        public int GetCenterX()
        {
            return Rectangle.X + Rectangle.Width / 2;
        }

        public int GetCenterY()
        {
            return Rectangle.Y + Rectangle.Height / 2;
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, Rectangle);
            //g.DrawEllipse(Pens.Gray, MagneticRectangle);
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

        public override string ToString()
        {
            return Name + " " + Rectangle.X + " " + Rectangle.Y;
        }
    }
}
