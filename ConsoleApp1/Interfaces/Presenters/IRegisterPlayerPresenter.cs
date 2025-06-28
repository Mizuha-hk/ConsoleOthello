using ConsoleOthello.Models.IO;
using ConsoleOthello.Models.ViewModels;

namespace ConsoleOthello.Interfaces.Presenters;

public interface IRegisterPlayerPresenter
{
    void Complete(RegisterPlayerOutputData output);
}
