using System;
using ConsoleOthello.Controllers;
using ConsoleOthello.Models.ViewModels;
using ConsoleOthello.Subjects;

namespace ConsoleOthello.Views;

public class RegisterPlayerView : ViewBase
{
    private readonly RegisterPlayerController _playerController;
    private readonly RegisterPlayerSubject _subject;

    private readonly InGameView _inGameView;

    private bool Player1Input = false;
    private bool Player2Input = false;
    private string Player1Name = string.Empty;
    private string Player2Name = string.Empty;

    public RegisterPlayerView(
        RegisterPlayerController playerController,
        RegisterPlayerSubject subject,
        InGameView inGameView)
    {
        if (playerController == null)
        {
            throw new ArgumentNullException(nameof(playerController), "プレイヤーコントローラーはnullであってはいけません。");
        }
        if (subject == null)
        {
            throw new ArgumentNullException(nameof(subject), "サブジェクトはnullであってはいけません。");
        }
        if (inGameView == null)
        {
            throw new ArgumentNullException(nameof(inGameView), "InGameViewはnullであってはいけません。");
        }
        _playerController = playerController;
        _subject = subject;

        _subject.OnViewModelChanged += OnViewChanged;
        _inGameView = inGameView;
    }

    public override void Dispose()
    {
        _subject.OnViewModelChanged -= OnViewChanged;
    }

    public override void Show()
    {
        if (Player1Input && Player2Input)
        {
            Console.WriteLine("プレイヤー1: " + Player1Name);
            Console.WriteLine("プレイヤー2: " + Player2Name);
            Console.WriteLine("ゲームを開始します。");
        }
        else if (!Player1Input)
        {
            Console.WriteLine("プレイヤー1の名前を入力してください: ");
            Console.Write("> ");
            Player1Name = Console.ReadLine();
            if (!string.IsNullOrEmpty(Player1Name))
            {
                Player1Input = true;
            }
            else
            {
                ShowError("プレイヤー1の名前は空にできません。もう一度入力してください。");
            }
        }
        else if (!Player2Input)
        {
            Console.WriteLine("プレイヤー2の名前を入力してください: ");
            Console.Write("> ");
            Player2Name = Console.ReadLine();
            if (!string.IsNullOrEmpty(Player2Name))
            {
                Player2Input = true;
            }
            else
            {
                ShowError("プレイヤー2の名前は空にできません。もう一度入力してください。");
            }
        }

        Update();
    }

    public override void Update()
    {
        if(Player1Input && Player2Input)
        {
            _playerController.RegisterPlayer(Player1Name, Player2Name);
            ClearInput();
        }
        else
        {
            Show();
        }
    }

    public override void ShowError(string message)
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

        _inGameView.Show(viewModel.RoomId);
    }

    private void ClearInput()
    {
        Player1Input = false;
        Player2Input = false;
        Player1Name = string.Empty;
        Player2Name = string.Empty;
    }
}
