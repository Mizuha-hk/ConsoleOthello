using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.Core.Interfaces.View;

namespace OthelloApp.ConsoleApp.Views;

public class MainView : ViewBase, IMainView
{
    private readonly INavigationService _navigationService;
    public MainView(INavigationService navigationService)
    {
       _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    private string errorMessage = string.Empty;

    public void Initialize()
    {
        Show();
        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                ShowError("入力が空です。コマンドを入力してください。");
                continue;
            }
            try
            {
                var command = ParseCommand(input);
                switch (command)
                {
                    case Command.start:
                        Console.WriteLine("ローカル2人対戦を開始します。");
                        _navigationService.NavigateToRegisterPlayerView();
                        return;
                    case Command.cpu:
                        Console.WriteLine("CPU対戦を開始します。");
                        _navigationService.NavigateToCpuGameView();
                        return;
                    case Command.quit:
                        Console.WriteLine("ゲームを終了します。");
                        return;
                    default:
                        ShowError("無効なコマンドです。もう一度入力してください。");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
    protected override void Show()
    {
        Console.Title = "Console Othello";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==========================");
        Console.WriteLine("||    Console Othelo    ||");
        Console.WriteLine("==========================");
        Console.WriteLine("");
        Console.ResetColor();

        if (!string.IsNullOrEmpty(errorMessage))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"エラー: {errorMessage}");
            errorMessage = string.Empty; // エラーメッセージをクリア
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("");
        }

        Console.WriteLine("コマンドを入力してください");
        Console.WriteLine("start: ローカル2人対戦");
        Console.WriteLine("cpu: CPU対戦");
        Console.WriteLine("quit: 終了");
        Console.Write("> ");
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ShowError(string message)
    {
        errorMessage = message;
        Update();
    }

    public override void Dispose()
    {

    }

    private Command ParseCommand(string input)
    {
        return input.ToLower() switch
        {
            "start" => Command.start,
            "cpu" => Command.cpu,
            "quit" => Command.quit,
            _ => throw new ArgumentException("無効なコマンドです。")
        };
    }

    private enum Command
    {
        start,
        cpu,
        quit
    }
}
