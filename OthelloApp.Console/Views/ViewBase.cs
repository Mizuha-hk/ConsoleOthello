using System;

namespace OthelloApp.ConsoleApp.Views;

public abstract class ViewBase : IDisposable
{
    protected abstract void Show();
    protected virtual void Update()
    {
        Console.Clear();
        Show();
    }

    protected virtual void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"エラー: {message}");
        Console.ResetColor();
    }

    public abstract void Dispose();
}
