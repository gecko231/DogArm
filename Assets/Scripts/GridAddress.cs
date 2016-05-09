/// <summary>
/// NOTE: we are ignoring the heck outta overflow
/// </summary>
public class GridAddress
{
    public uint width;
    public uint height;
    public uint x;
    public uint y;

    public GridAddress(uint w, uint h)
    {
        width = w;
        height = h;
        x = 0;
        y = 0;
    }

    public GridAddress(uint w, uint h, uint x, uint y)
    {
        width = w;
        height = h;
        this.x = x;
        this.y = y;
    }

    public uint Index
    {
        get
        {
            return (y * (width)) + x;
        }
    }

    public void Left()
    {
        var newX = x - 1;
        if (newX == uint.MaxValue)
        {
            newX = width-1;
            Up();
        }
        x = newX;
    }

    public void Right()
    {
        var newX = x + 1;
        if (newX > (width-1))
        {
            newX = 0;
            Down();
        }
        x = newX;
    }

    public void Up()
    {
        var newY = y - 1;
        if (newY == uint.MaxValue)
        {
            newY = height-1;
        }
        y = newY;
    }

    public void Down()
    {
        var newY = y + 1;
        if (newY > (height-1))
        {
            newY = 0;
        }
        y = newY;
    }
}
