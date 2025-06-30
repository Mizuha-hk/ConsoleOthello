using OthelloApp.Core.Exceptions;
using OthelloApp.Core.Models.IO;
using OthelloApp.Core.Entities;
using OthelloApp.Core.Interfaces.UseCases;

namespace OthelloApp.ConsoleApp.Controllers;

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

    public void MovePiece(Guid roomId, DiscType discType, int row, int column)
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

        var inputData = new MovePieceInputData(roomId, discType, row, column);
        _movePieceUseCase.Handle(inputData);
    }
}
