using ConsoleOthello.Models.ViewModels;
using System;

namespace ConsoleOthello.Subjects;

public class RegisterPlayerSubject
{
    private RegisterPlayerViewModel _viewModel;

    public event Action<RegisterPlayerViewModel> OnViewModelChanged;

    public RegisterPlayerViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            OnViewModelChanged?.Invoke(_viewModel);
        }
    }
}
