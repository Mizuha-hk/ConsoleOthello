using OthelloApp.Core.Entities;
using OthelloApp.Core.Models;
using System;

namespace OthelloApp.Core.Interfaces.Repositories;
public interface IRoomStateRepository
{
    Room GetRoom(Guid roomId);
    Guid CreateRoom();
    void JoinRoom(Guid roomId, Player player);
    Room MovePiece(Guid roomId, DiscType discType, int row, int column);
    void DeleteRoom(Guid roomId);
}
