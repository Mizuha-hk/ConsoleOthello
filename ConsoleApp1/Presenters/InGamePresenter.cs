using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Models.IO;
using ConsoleOthello.Models.ViewModels;
using ConsoleOthello.Subjects;
using System;

namespace ConsoleOthello.Presenters;

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
            Player1 = output.Room.Player1,
            Player2 = output.Room.Player2,
            IsPlayer1Turn = output.Room.IsPlayer1Turn,
            IsGameOver = output.Room.IsGameOver,
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
            Player1 = output.Room.Player1,
            Player2 = output.Room.Player2,
            IsPlayer1Turn = output.Room.IsPlayer1Turn,
            IsGameOver = output.Room.IsGameOver,
            Board = output.Room.Board,
            Winner = output.Room.Winner
        };
    }
}