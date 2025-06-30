using OthelloApp.Core.Models.IO;

namespace OthelloApp.Core.Interfaces.Presenters;

public interface IRegisterPlayerPresenter
{
    void Complete(RegisterPlayerOutputData output);
}
