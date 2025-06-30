using OthelloApp.Core.Models.IO;

namespace OthelloApp.Core.Interfaces.Presenters;

public interface IInGamePresenter
{
    void GetComplete(GetRoomOutputData output);
    void MoveComplete(MovePieceOutputData output);
}
