using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interfaces.UseCases;

public interface IMovePieceUseCase
{
    void Handle(MovePieceInputData input);
}
