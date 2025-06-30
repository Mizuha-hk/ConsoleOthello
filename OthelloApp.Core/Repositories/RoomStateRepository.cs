using OthelloApp.Core.Entities;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Models;
using System;
using System.Collections.Generic;

namespace OthelloApp.Core.Repositories;

public class RoomStateRepository : IRoomStateRepository
{
    private readonly List<Room> rooms = [];

    public Guid CreateRoom()
    {
        var room = new Room();
        rooms.Add(room);
        return room.Id;
    }

    public void DeleteRoom(Guid roomId)
    {
        rooms.RemoveAll(r => r.Id == roomId);
    }

    public Room GetRoom(Guid roomId)
    {
        var room = rooms.Find(r => r.Id == roomId);
        if (room == null)
        {
            throw new KeyNotFoundException($"Room with ID {roomId} not found.");
        }
        return room;
    }

    public void JoinRoom(Guid roomId, Player player)
    {
        var room = GetRoom(roomId);
        if (room.IsFull)
        {
            throw new InvalidOperationException("Room is already full.");
        }
        if (room.Player1 == null)
        {
            room.Player1 = player;
        }
        else if (room.Player2 == null)
        {
            room.Player2 = player;
        }
        else
        {
            throw new InvalidOperationException("Room is already full.");
        }
    }

    public Room MovePiece(Guid roomId, DiscType discType, int row, int column)
    {
        var room = GetRoom(roomId);
        if (room.IsGameOver)
        {
            throw new InvalidOperationException("Game is already over.");
        }

        room.Board.PlaceDisc(row, column, discType);

        var opponentDiscType = discType == DiscType.Player1 ? DiscType.Player2 : DiscType.Player1;

        var oponentValidMoves = room.Board.AllValidMoves(opponentDiscType);
        if (oponentValidMoves.MovableCells.Count == 0) {
            room.IsPlayer1Turn = !room.IsPlayer1Turn; 

            var currentPlayerValidMoves = room.Board.AllValidMoves(discType);
            if (currentPlayerValidMoves.MovableCells.Count == 0)
            {
                room.IsGameOver = true;
                if (room.Board.GetPlayer1Count() == room.Board.GetPlayer2Count())
                {
                    room.Winner = null; // Draw
                }
                else if (room.Board.GetPlayer1Count() > room.Board.GetPlayer2Count())
                {
                    room.Winner = room.Player1;
                }
                else
                {
                    room.Winner = room.Player2;
                }
                room.NextPlayerValidMoves = null;
            }
            else
            {
                room.NextPlayerValidMoves = currentPlayerValidMoves;
            }
        }
        else
        {
            room.NextPlayerValidMoves = oponentValidMoves;
            room.IsPlayer1Turn = !room.IsPlayer1Turn; 
            room.PassedLastTurn = false;
        }

        return room;
    }
}
