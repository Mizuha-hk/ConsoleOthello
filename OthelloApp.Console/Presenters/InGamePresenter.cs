using OthelloApp.Core.Interfaces.Presenters;
using OthelloApp.Core.Models.IO;
using OthelloApp.ConsoleApp.Subjects;
using OthelloApp.ConsoleApp.ViewModels;

namespace OthelloApp.ConsoleApp.Presenters;

public class InGamePresenter : IInGamePresenter
{
    private readonly BoardSubject _boardSubject;
    public InGamePresenter(BoardSubject subject)
    {
        if (subject == null)
        {
            throw new ArgumentNullException(nameof(subject), "BoardSubjectはnullであってはいけません。");
        }
        _boardSubject = subject;
    }

    public void GetComplete(GetRoomOutputData output)
    {
        if (output == null)
        {
            throw new ArgumentNullException(nameof(output), "出力データはnullであってはいけません。");
        }

        _boardSubject.Initialize(new InGameViewModel
        {
            RoomId = output.Room.Id,
            TurnPlayer = output.Room.IsPlayer1Turn ? output.Room.Player1 : output.Room.Player2,
            Player1 = output.Room.Player1,
            Player2 = output.Room.Player2,
            ValidMoves = output.Room.NextPlayerValidMoves,
            IsGameOver = output.Room.IsGameOver,
            PassedLastTurn = output.Room.PassedLastTurn,
            Board = output.Room.Board,
            Winner = output.Room.Winner
        });
    }

    public void MoveComplete(MovePieceOutputData output)
    {
        if (output == null)
        {
            throw new ArgumentNullException(nameof(output), "出力データはnullであってはいけません。");
        }

        _boardSubject.ViewModel = new InGameViewModel
        {
            RoomId = output.Room.Id,
            TurnPlayer = output.Room.IsPlayer1Turn ? output.Room.Player1 : output.Room.Player2,
            Player1 = output.Room.Player1,
            Player2 = output.Room.Player2,
            ValidMoves = output.Room.NextPlayerValidMoves,
            PassedLastTurn = output.Room.PassedLastTurn,
            IsGameOver = output.Room.IsGameOver,
            Board = output.Room.Board,
            Winner = output.Room.Winner
        };
    }
}