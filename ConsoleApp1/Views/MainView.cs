using System;
namespace ConsoleOthello.Views;

public class MainView : ViewBase
{
    private string errorMessage = string.Empty;
    public override void Show()
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
        Console.WriteLine("quit: 終了");
        Console.Write("> ");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ShowError(string message)
    {
        errorMessage = message;
        Update();
    }

    public override void Dispose()
    {

    }
}
