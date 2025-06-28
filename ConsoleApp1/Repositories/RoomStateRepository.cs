using ConsoleOthello.Entities;
using ConsoleOthello.Interfaces.Repositories;
using ConsoleOthello.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleOthello.Repositories;

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

    public Room MovePiece(Guid roomId, Piece piece, int row, int column)
    {
        var room = GetRoom(roomId);
        if (room.IsGameOver)
        {
            throw new InvalidOperationException("Game is already over.");
        }

        room.Board.MovePiece(row, column, piece);

        var player1MoveAvailable = room.Board.IsMoveAvailable(Piece.Player1);
        var player2MoveAvailable = room.Board.IsMoveAvailable(Piece.Player2);

        if (player1MoveAvailable.IsMoveAvailable == false && player2MoveAvailable.IsMoveAvailable == false)
        {
            room.IsGameOver = true;
            if (room.Board.GetPlayer1Count() > room.Board.GetPlayer2Count())
            {
                room.Winner = room.Player1;
            }
            else if (room.Board.GetPlayer1Count() < room.Board.GetPlayer2Count())
            {
                room.Winner = room.Player2;
            }
            else
            {
                room.Winner = null; // Draw
            }
        }
        else if (piece == Piece.Player1)
        {
            if (!player2MoveAvailable.IsMoveAvailable)
            {
                room.PassedLastTurn = true;
                room.IsPlayer1Turn = true;
            }
            else
            {
                room.PassedLastTurn = false;
                room.IsPlayer1Turn = false;
            }
        }
        else if (piece == Piece.Player2)
        {
            if (!player1MoveAvailable.IsMoveAvailable)
            {
                room.PassedLastTurn = true;
                room.IsPlayer1Turn = false;
            }
            else
            {
                room.PassedLastTurn = false;
                room.IsPlayer1Turn = true;
            }
        }
        return room;
    }
}
