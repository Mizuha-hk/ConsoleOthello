using OthelloApp.ConsoleApp.ViewModels;

namespace OthelloApp.ConsoleApp.Subjects;

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
