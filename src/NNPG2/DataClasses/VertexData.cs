using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class VertexData
    {
        private string Name { get; }
        // Class Rectangle is not used due to Serialization problems
        // Variables are public becou private are not serialized
        public int RectangleX;
        public int RectangleY;
        public int RectangleWidth;
        public int RectangleHeight;

        public int MagneticRectangleX;
        public int MagneticRectangleY;
        public int MagneticRectangleWidth;
        public int MagneticRectangleHeight;

        private bool Hovered;
        private bool Selected;
        private const int DefaultSize = 50;
        private const int DefaultMagneticSize = 40;

        public VertexData(string name, int x, int y, int width = DefaultSize, int height = DefaultSize)
        {
            Name = name;
            RectangleX = x;
            RectangleY = y;
            RectangleWidth = width;
            RectangleHeight = height;

            UpdateMagneticRectangle();
        }

        private void UpdateMagneticRectangle()
        {
            MagneticRectangleX = RectangleX - DefaultMagneticSize / 2;
            MagneticRectangleY = RectangleY - DefaultMagneticSize / 2;
            MagneticRectangleWidth = RectangleWidth + DefaultMagneticSize;
            MagneticRectangleHeight = RectangleHeight + DefaultMagneticSize;
        }

        public void SetRectX(int x)
        {
            RectangleX = x;
            UpdateMagneticRectangle();
        }

        public void SetRectY(int y)
        {
            RectangleY = y;
            UpdateMagneticRectangle();
        }

        public void SetRectXY(int x, int y)
        {
            RectangleX = x;
            RectangleY = y;
            UpdateMagneticRectangle();
        }

        public int GetCenterX()
        {
            return RectangleX + RectangleWidth / 2;
        }

        public int GetCenterY()
        {
            return RectangleY + RectangleHeight / 2;
        }

        public int GetX()
        {
            return RectangleX;
        }

        public int GetY()
        {
            return RectangleY;
        }

        public bool GetHovered()
        {
            return Hovered;
        }

        public void SetHovered(bool hovered)
        {
            Hovered = hovered;
        }

        public bool GetSelected()
        {
            return Selected;
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
        }

        public bool RectangleContains(Point point)
        {
            return new Rectangle(RectangleX, RectangleY, RectangleWidth, RectangleHeight).Contains(point);
        }

        public bool MagneticRectangleContains(Point point)
        {
            return new Rectangle(MagneticRectangleX, MagneticRectangleY, MagneticRectangleWidth, MagneticRectangleHeight).Contains(point);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Black, new Rectangle(RectangleX, RectangleY, RectangleWidth, RectangleHeight));
            //g.DrawEllipse(Pens.Gray, MagneticRectangle);
            //g.DrawString(Name, new Font("Arial", 10), Brushes.White, RectangleX + RectangleWidth / 2, RectangleY + RectangleHeight / 2);

            if (Selected)
            {
                using (Pen pen = new Pen(Color.Red) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    g.DrawEllipse(pen, new Rectangle(RectangleX, RectangleY, RectangleWidth, RectangleHeight));
                }
            }
            if (Hovered)
            {
                g.DrawEllipse(Pens.Yellow, new Rectangle(RectangleX, RectangleY, RectangleWidth, RectangleHeight));
            }
        }

        public override string ToString()
        {
            return Name + " " + RectangleX + " " + RectangleY;
        }
    }
}
