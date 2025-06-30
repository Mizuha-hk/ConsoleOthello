using OthelloApp.Core.Models.IO;

namespace OthelloApp.Core.Interfaces.UseCases;

public interface IMovePieceUseCase
{
    void Handle(MovePieceInputData input);
}
