using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models.ViewModels;

public class InGameViewModel
{
    public Guid RoomId { get; set; }
    public Board Board { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public bool IsPlayer1Turn { get; set; }
    public bool PassedLastTurn { get; set; } = false;
    public bool IsGameOver { get; set; } = false;
    public Player Winner { get; internal set; }
}
