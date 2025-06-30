using OthelloApp.Core.Entities;
using OthelloApp.Core.Models;
using System;

namespace OthelloApp.Core.Models.IO;

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
