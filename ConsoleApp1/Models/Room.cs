using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models;

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
}
