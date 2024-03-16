using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2
{
    internal class Map
    {
        private Image image;
        private Point coordinates;

        public Map(string path, int x, int y)
        {
            image = Image.FromFile(path);
            coordinates = new Point(x, y);
        }

        public void UpdateCoordinates(int deltaX, int deltaY)
        {
            int newX = coordinates.X + deltaX;
            int newY = coordinates.Y + deltaY;
            coordinates.X = newX;
            coordinates.Y = newY;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(image, coordinates);
        }
    }
}
