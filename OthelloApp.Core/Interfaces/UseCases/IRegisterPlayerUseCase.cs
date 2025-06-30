using OthelloApp.Core.Models.IO;

namespace OthelloApp.Core.Interfaces.UseCases;

public interface IRegisterPlayerUseCase
{
    void Handle(RegisterPlayerInputData input);
}
