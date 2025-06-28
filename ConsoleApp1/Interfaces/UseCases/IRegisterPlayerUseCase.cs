using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interfaces.UseCases;

public interface IRegisterPlayerUseCase
{
    void Handle(RegisterPlayerInputData input);
}
