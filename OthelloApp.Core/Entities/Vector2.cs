namespace OthelloApp.Core.Entities;

public class Vector2
{
    public int Row { get; set; }
    public int Column { get; set; }

    public Vector2(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
