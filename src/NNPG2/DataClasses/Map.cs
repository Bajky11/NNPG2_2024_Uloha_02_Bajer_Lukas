using System.Drawing;

internal class Map
{
    private Image image;
    private Point coordinates;
    private float scale = 1.0f; // Initial scale

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

    public void Zoom(float scaleFactor)
    {
        scale *= scaleFactor;
    }

    public void Draw(Graphics g)
    {
        int width = (int)(image.Width * scale);
        int height = (int)(image.Height * scale);
        Rectangle destinationRect = new Rectangle(coordinates.X, coordinates.Y, width, height);
        g.DrawImage(image, destinationRect);
    }
}
