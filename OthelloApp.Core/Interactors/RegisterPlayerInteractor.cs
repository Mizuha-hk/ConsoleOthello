using OthelloApp.Core.Entities;
using OthelloApp.Core.Interfaces.Presenters;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Interfaces.UseCases;
using OthelloApp.Core.Models.IO;
using System;

namespace OthelloApp.Core.Interactors;

public class RegisterPlayerInteractor : IRegisterPlayerUseCase
{
    private readonly IRegisterPlayerPresenter _presenter;
    private readonly IRoomStateRepository _repository;

    public RegisterPlayerInteractor(
        IRegisterPlayerPresenter presenter,
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

    public void Handle(RegisterPlayerInputData input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input), "入力データはnullであってはいけません。");
        }
        if (string.IsNullOrWhiteSpace(input.Player1Name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。", nameof(input.Player1Name));
        }
        if (string.IsNullOrEmpty(input.Player2Name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。", nameof(input.Player2Name));
        }

        var player1 = new Player(input.Player1Name, DiscType.Player1);
        var player2 = new Player(input.Player2Name, DiscType.Player2);
        var id = _repository.CreateRoom();
        _repository.JoinRoom(id, player1);
        _repository.JoinRoom(id, player2);

        var output = new RegisterPlayerOutputData(id, player1, player2);
        _presenter.Complete(output);
    }
}
