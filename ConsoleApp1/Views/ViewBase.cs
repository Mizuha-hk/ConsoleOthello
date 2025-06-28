using System;

namespace ConsoleOthello.Views;

public abstract class ViewBase: IDisposable
{
    public abstract void Show();
    public virtual void Update()
    {
        Console.Clear();
        Show();
    }

    public virtual void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"エラー: {message}");
        Console.ResetColor();
    }

    public abstract void Dispose();
}
