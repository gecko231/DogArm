public class GridAddress
{
    public int width;
    public int height;
    public int x;
    public int y;

    public GridAddress(int w, int h)
    {
        width = w;
        height = h;
    }

    public GridAddress(int w, int h, int x, int y)
    {
        width = w;
        height = h;
        this.x = x;
        this.y = y;
    }

    public int Index
    {
        get
        {
            return (y * height) + x;
        }
    }

    public void Left()
    {
        var newX = x - 1;
        if (newX < 0)
        {
            newX = width-1;
            Up();
        }
        x = newX;
    }

    public void Right()
    {
        var newX = x + 1;
        if (newX >= width)
        {
            newX = 0;
            Down();
        }
        x = newX;
    }

    public void Up()
    {
        var newY = y - 1;
        if (newY < 0)
        {
            newY = height-1;
        }
        y = newY;
    }

    public void Down()
    {
        var newY = y + 1;
        if (newY >= height)
        {
            newY = 0;
        }
        y = newY;
    }
}
