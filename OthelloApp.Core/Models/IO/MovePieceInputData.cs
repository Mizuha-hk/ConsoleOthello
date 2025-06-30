using OthelloApp.Core.Entities;
using System;

namespace OthelloApp.Core.Models.IO;

public class MovePieceInputData
{
    public Guid RoomId { get; set; }
    public DiscType DiscType { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public MovePieceInputData(Guid roomId, DiscType discType, int row, int column)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません．", nameof(roomId));
        }

        RoomId = roomId;
        Row = row;
        Column = column;
        DiscType = discType;
    }
}
