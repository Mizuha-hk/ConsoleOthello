using System.Collections.Generic;

namespace OthelloApp.Core.Entities;

public class ValidMoves
{
    public DiscType Player { get; set; }
    public List<ValidMove> MovableCells { get; set; } = new();
}
