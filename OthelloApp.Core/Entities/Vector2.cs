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

    public override bool Equals(object obj)
    {
        if (obj is Vector2 other)
        {
            return Row == other.Row && Column == other.Column;
        }
        return false;
    }
}
