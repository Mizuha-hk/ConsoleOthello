using OthelloApp.Core.Models.IO;
using OthelloApp.Core.Interfaces.Presenters;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Interfaces.UseCases;
using System;
using OthelloApp.Core.Entities;

namespace OthelloApp.Core.Interactors;

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

        _presenter.GetComplete(output);
    }
}
