using OthelloApp.Core.Exceptions;
using OthelloApp.Core.Entities;
using OthelloApp.Core.Interfaces.Presenters;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Interfaces.UseCases;
using OthelloApp.Core.Models.IO;
using System;

namespace OthelloApp.Core.Interactors;

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

        var room = _repository.MovePiece(input.RoomId, input.DiscType, input.Row, input.Column);

        var output = new MovePieceOutputData(room);

        _presenter.MoveComplete(output);
    }
}
