using OthelloApp.ConsoleApp.Controllers;
using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.ConsoleApp.Subjects;
using OthelloApp.ConsoleApp.ViewModels;

namespace OthelloApp.ConsoleApp.Views;

public class CpuGameView : ViewBase
{
    private readonly RegisterPlayerController _playerController;
    private readonly RegisterPlayerSubject _subject;
    private readonly INavigationService _navigationService;

    private bool PlayerNameInput = false;
    private string PlayerName = string.Empty;
    private const string CpuPlayerName = "CPU";

    public CpuGameView(
        RegisterPlayerController playerController,
        RegisterPlayerSubject subject,
        INavigationService navigationService)
    {
        if (playerController == null)
        {
            throw new ArgumentNullException(nameof(playerController), "プレイヤーコントローラーはnullであってはいけません。");
        }
        if (subject == null)
        {
            throw new ArgumentNullException(nameof(subject), "サブジェクトはnullであってはいけません。");
        }
        if (navigationService == null)
        {
            throw new ArgumentNullException(nameof(navigationService), "ナビゲーションサービスはnullであってはいけません。");
        }
        _playerController = playerController;
        _subject = subject;

        _subject.OnViewModelChanged += OnViewChanged;
        _navigationService = navigationService;
    }

    public void Initialize()
    {
        ClearInput();
        Show();
    }

    public override void Dispose()
    {
        _subject.OnViewModelChanged -= OnViewChanged;
    }

    protected override void Show()
    {
        if (PlayerNameInput)
        {
            Console.WriteLine("プレイヤー: " + PlayerName);
            Console.WriteLine("CPU: " + CpuPlayerName);
            Console.WriteLine("ゲームを開始します。");
        }
        else
        {
            Console.WriteLine("CPU対戦モード");
            Console.WriteLine("あなたの名前を入力してください: ");
            Console.Write("> ");
            PlayerName = Console.ReadLine();
            if (!string.IsNullOrEmpty(PlayerName))
            {
                PlayerNameInput = true;
            }
            else
            {
                ShowError("プレイヤーの名前は空にできません。もう一度入力してください。");
            }
        }

        Update();
    }

    protected override void Update()
    {
        if (PlayerNameInput)
        {
            _playerController.RegisterPlayer(PlayerName, CpuPlayerName);
            ClearInput();
        }
        else
        {
            Show();
        }
    }

    protected override void ShowError(string message)
    {
        base.ShowError(message);
    }

    private void OnViewChanged(RegisterPlayerViewModel viewModel)
    {
        Console.Clear();
        if (viewModel == null)
        {
            Console.WriteLine("プレイヤー登録に失敗しました。もう一度やり直してください。");
            ClearInput();
            return;
        }

        _navigationService.NavigateToInGameView(viewModel.RoomId);
    }

    private void ClearInput()
    {
        PlayerNameInput = false;
        PlayerName = string.Empty;
    }
}