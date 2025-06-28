using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models.IO;

public class MovePieceOutputData
{
    public Room Room { get; set; }
    public MovePieceOutputData(Room room)
    {
        if (room == null)
        {
            throw new ArgumentNullException(nameof(room), "ルームはnullであってはいけません．");
        }
        Room = room;
    }
}
