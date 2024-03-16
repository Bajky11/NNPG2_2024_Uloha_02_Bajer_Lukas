using System.Drawing;

internal class Map
{
    private Image image;
    public Point coordinates;
    

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
        int width = image.Width;
        int height = image.Height;
        Rectangle destinationRect = new Rectangle(coordinates.X, coordinates.Y, width, height);
        g.DrawImage(image, destinationRect);
    }
}
