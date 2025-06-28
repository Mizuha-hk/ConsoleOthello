using System;
using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Interfaces.Repositories;
using ConsoleOthello.Interfaces.UseCases;
using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interactors;

public class GetRoomInteractor : IGetRoomUseCase
{
    private readonly IRoomStateRepository _repository;
    private readonly IInGamePresenter _presenter;
    public GetRoomInteractor(IRoomStateRepository repository, IInGamePresenter presenter)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository), "リポジトリはnullであってはいけません。");
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter), "プレゼンターはnullであってはいけません。");
    }
    public void Handle(GetRoomInputData input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input), "入力データはnullであってはいけません。");
        }
        var room = _repository.GetRoom(input.RoomId);
        if (room == null)
        {
            throw new InvalidOperationException("指定されたルームが存在しません。");
        }
        var output = new GetRoomOutputData(room);
        if (output.Room.IsGameOver)
        {
            _repository.DeleteRoom(output.Room.Id);
        }

        _presenter.GetComplete(output);
    }
}
