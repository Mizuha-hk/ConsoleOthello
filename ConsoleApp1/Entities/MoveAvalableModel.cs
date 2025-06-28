using System.Collections.Generic;

namespace ConsoleOthello.Entities;

public class MoveAvalableModel
{
    public bool IsMoveAvailable { get; set; } = false;
    public List<int[]> AvailableMoves { get; set; } = new List<int[]>();
}
