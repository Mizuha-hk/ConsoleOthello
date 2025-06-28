using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models.ViewModels;

public class RegisterPlayerViewModel
{
    public Guid RoomId { get; set; }
    public Player Player1;
    public Player Player2;
}
