using System;
using ConsoleOthello.Entities;
using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Interfaces.Repositories;
using ConsoleOthello.Interfaces.UseCases;
using ConsoleOthello.Models;
using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interactors;

public class RegisterPlayerInteractor : IRegisterPlayerUseCase
{
    private readonly IRegisterPlayerPresenter _presenter;
    private readonly IRoomStateRepository _repository;

    public RegisterPlayerInteractor(
        IRegisterPlayerPresenter presenter,
        IRoomStateRepository repository)
    {
        if(presenter == null)
        {
            throw new ArgumentNullException(nameof(presenter), "プレゼンターはnullであってはいけません。");
        }
        if(repository == null)
        {
            throw new ArgumentNullException(nameof(repository), "リポジトリはnullであってはいけません。");
        }

        _presenter = presenter;
        _repository = repository;
    }

    public void Handle(RegisterPlayerInputData input)
    {
        if(input == null)
        {
            throw new ArgumentNullException(nameof(input), "入力データはnullであってはいけません。");
        }
        if(string.IsNullOrWhiteSpace(input.Player1Name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。", nameof(input.Player1Name));
        }
        if (string.IsNullOrEmpty(input.Player2Name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。", nameof(input.Player2Name));
        }

        var player1 = new Player(input.Player1Name, Piece.Player1);
        var player2 = new Player(input.Player2Name, Piece.Player2);
        var id = _repository.CreateRoom();
        _repository.JoinRoom(id, player1);
        _repository.JoinRoom(id, player2);

        var output = new RegisterPlayerOutputData(id, player1, player2);
        _presenter.Complete(output);
    }
}
