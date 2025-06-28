using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interfaces.UseCases;

public interface IGetRoomUseCase
{
    void Handle(GetRoomInputData input);
}
