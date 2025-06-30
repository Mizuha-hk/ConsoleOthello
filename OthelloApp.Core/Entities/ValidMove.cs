using OthelloApp.Core.Exceptions;
using System.Collections.Generic;

namespace OthelloApp.Core.Entities;

public class ValidMove
{
    private int _row;
    public int Row
    {
        get => _row;
        set
        {
            if (value < 0 || value >= Board.Size)
            {
                throw new BoardOutOfRangeException();
            }
            _row = value;
        }
    }
    private int _column;
    public int Column 
    {
        get => _column;
        set
        {
            if (value < 0 || value >= Board.Size)
            {
                throw new BoardOutOfRangeException();
            }
            _column = value;
        } 
    }

    public List<Vector2> Directions { get; set; } = [];
}
