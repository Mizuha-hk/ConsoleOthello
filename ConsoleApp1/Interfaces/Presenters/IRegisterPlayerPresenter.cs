using ConsoleOthello.Models.IO;

namespace ConsoleOthello.Interfaces.Presenters;

public interface IRegisterPlayerPresenter
{
    void Complete(RegisterPlayerOutputData output);
}
