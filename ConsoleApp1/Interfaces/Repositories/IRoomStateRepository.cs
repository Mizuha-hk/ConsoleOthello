using ConsoleOthello.Entities;
using ConsoleOthello.Models;
using System;

namespace ConsoleOthello.Interfaces.Repositories;
public interface IRoomStateRepository
{
    Room GetRoom(Guid roomId);
    Guid CreateRoom();
    void JoinRoom(Guid roomId, Player player);
    Room MovePiece(Guid roomId, Piece piece, int row, int column);
    void DeleteRoom(Guid roomId);
}
