using System;
using ConsoleOthello.Models.IO;
using ConsoleOthello.Models.ViewModels;

namespace ConsoleOthello.Subjects;

public class BoardSubject
{
    public event Action<InGameViewModel> OnInitialized;
    public event Action<InGameViewModel> OnMovePieceCompleted;

    public BoardSubject()
    {
        _viewModel = new InGameViewModel();
        OnInitialized?.Invoke(_viewModel);
    }

    public void Initialize(InGameViewModel viewModel)
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel), "InGameViewModelはnullであってはいけません。");
        }
        _viewModel = viewModel;
        OnInitialized?.Invoke(_viewModel);
    }

    private InGameViewModel _viewModel;
    public InGameViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            OnMovePieceCompleted?.Invoke(new InGameViewModel
            {
                RoomId = value.RoomId,
                Player1 = value.Player1,
                Player2 = value.Player2,
                IsPlayer1Turn = value.IsPlayer1Turn,
                IsGameOver = value.IsGameOver,
                Board = value.Board,
                Winner = value.Winner,
                PassedLastTurn = value.PassedLastTurn

            });
        }
    }

}
