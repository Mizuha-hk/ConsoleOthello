using OthelloApp.Core.Entities;

namespace OthelloApp.ConsoleApp.ViewModels;

public class InGameViewModel
{
    public Guid RoomId { get; set; }
    public Board Board { get; set; }
    public Player TurnPlayer { get; set; }
    public ValidMoves ValidMoves { get; set; } 
    public bool PassedLastTurn { get; set; } = false;
    public bool IsGameOver { get; set; } = false;
    public Player Winner { get; set; }
    public int Player1Count => Board.GetPlayer1Count();
    public int Player2Count => Board.GetPlayer2Count();
}
