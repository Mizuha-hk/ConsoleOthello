using ConsoleOthello.Entities;
using ConsoleOthello.Exceptions;
using ConsoleOthello.Interfaces.UseCases;
using ConsoleOthello.Models.IO;
using System;

namespace ConsoleOthello.Controllers;

public class InGameController
{
    private readonly IGetRoomUseCase _getRoomUseCase;
    private readonly IMovePieceUseCase _movePieceUseCase;
    public InGameController(
        IGetRoomUseCase getRoomUseCase,
        IMovePieceUseCase movePieceUseCase)
    {
        if (getRoomUseCase == null)
        {
            throw new ArgumentNullException(nameof(getRoomUseCase), "ルームを取得するユースケースはnullであってはいけません。");
        }
        if (movePieceUseCase == null)
        {
            throw new ArgumentNullException(nameof(movePieceUseCase), "コマを移動するユースケースはnullであってはいけません。");
        }
        _getRoomUseCase = getRoomUseCase;
        _movePieceUseCase = movePieceUseCase;
    }

    public void GetRoom(Guid roomId)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません。", nameof(roomId));
        }
        var inputData = new GetRoomInputData(roomId);
        _getRoomUseCase.Handle(inputData);
    }

    public void MovePiece(Guid roomId, Piece pieceType, int row, int column)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません。", nameof(roomId));
        }
        if (row < 0 || row >= Board.Size)
        {
            throw new BoardOutOfRangeException();
        }
        if (column < 0 || column >= Board.Size)
        {
            throw new BoardOutOfRangeException();
        }
        if (pieceType != Piece.Player1 && pieceType != Piece.Player2)
        {
            throw new ArgumentException("プレイヤーが設置するコマのタイプである必要があります。", nameof(pieceType));
        }

        var inputData = new MovePieceInputData(roomId, pieceType, row, column);
        _movePieceUseCase.Handle(inputData);
    }
}
