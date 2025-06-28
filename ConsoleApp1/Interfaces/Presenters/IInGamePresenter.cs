using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interfaces.Presenters;

public interface IInGamePresenter
{
    void GetComplete(GetRoomOutputData output);
    void MoveComplete(MovePieceOutputData output);
}
