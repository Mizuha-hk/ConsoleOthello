using OthelloApp.Core.Entities;
using System;

namespace OthelloApp.ConsoleApp.ViewModels;

public class RegisterPlayerViewModel
{
    public Guid RoomId { get; set; }
    public Player Player1;
    public Player Player2;
}
