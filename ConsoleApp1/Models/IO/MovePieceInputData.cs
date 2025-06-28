using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models.IO;

public class MovePieceInputData
{
    public Guid RoomId { get; set; }
    public Piece PieceType { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public MovePieceInputData(Guid roomId, Piece pieceType, int row, int column)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません．", nameof(roomId));
        }
        if (pieceType != Piece.Player1 && pieceType != Piece.Player2)
        {
            throw new ArgumentException("プレイヤーが設置するコマのタイプである必要があります．");
        }

        RoomId = roomId;
        Row = row;
        Column = column;
        PieceType = pieceType;
    }
}
