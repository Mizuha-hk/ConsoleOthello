using System;
using ConsoleOthello.Exceptions;
using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Interfaces.Repositories;
using ConsoleOthello.Interfaces.UseCases;
using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interactors;

public class MovePieceInteractor : IMovePieceUseCase
{
    private readonly IInGamePresenter _presenter;
    private readonly IRoomStateRepository _repository;
    public MovePieceInteractor(
        IInGamePresenter presenter,
        IRoomStateRepository repository)
    {
        if (presenter == null)
        {
            throw new ArgumentNullException(nameof(presenter), "プレゼンターはnullであってはいけません。");
        }
        if (repository == null)
        {
            throw new ArgumentNullException(nameof(repository), "リポジトリはnullであってはいけません。");
        }
        _presenter = presenter;
        _repository = repository;
    }

    public void Handle(MovePieceInputData input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input), "入力データはnullであってはいけません。");
        }
        if (input.RoomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません。", nameof(input.RoomId));
        }
        if (input.Row < 0 || input.Row >= 8)
        {
            throw new BoardOutOfRangeException();
        }
        if (input.Column < 0 || input.Column >= 8)
        {
            throw new BoardOutOfRangeException();
        }
        if (input.PieceType != Entities.Piece.Player1 && input.PieceType != Entities.Piece.Player2)
        {
            throw new ArgumentException("プレイヤーが設置するコマのタイプである必要があります。", nameof(input.PieceType));
        }

        var room = _repository.MovePiece(input.RoomId, input.PieceType, input.Row, input.Column);

        var output = new MovePieceOutputData(room);

        _presenter.MoveComplete(output);
    }
}
