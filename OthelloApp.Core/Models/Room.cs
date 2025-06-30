using OthelloApp.Core.Entities;
using System;

namespace OthelloApp.Core.Models;

public class Room
{
    public Guid Id { get; } = Guid.NewGuid();
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public bool IsFull
        => Player1 != null && Player2 != null;
    public bool IsPlayer1Turn { get; set; } = true;
    public bool PassedLastTurn { get; set; } = false;
    public bool IsGameOver { get; set; } = false;
    public Player Winner { get; set; } = null;
    public Board Board { get; set; } = new Board();
    public ValidMoves NextPlayerValidMoves { get; set; }

    public Room()
    {
        NextPlayerValidMoves = Board.AllValidMoves(DiscType.Player1);
    }
}
