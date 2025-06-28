using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Models.IO;
using ConsoleOthello.Models.ViewModels;
using ConsoleOthello.Subjects;

namespace ConsoleOthello.Presenters;

public class RegisterPlayerPresenter: IRegisterPlayerPresenter
{
    private readonly RegisterPlayerSubject _subject;

    public RegisterPlayerPresenter(RegisterPlayerSubject subject)
    {
        _subject = subject;
    }

    public void Complete(RegisterPlayerOutputData output)
    {
        _subject.ViewModel = new RegisterPlayerViewModel
        {
            RoomId = output.RoomId,
            Player1 = output.Player1,
            Player2 = output.Player2
        };
    }
}
