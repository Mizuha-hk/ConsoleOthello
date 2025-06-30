using OthelloApp.Core.Models.IO;

namespace OthelloApp.Core.Interfaces.UseCases;

public interface IGetRoomUseCase
{
    void Handle(GetRoomInputData input);
}
