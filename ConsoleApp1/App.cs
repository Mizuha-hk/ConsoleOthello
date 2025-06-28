using ConsoleOthello.Controllers;
using ConsoleOthello.Interactors;
using ConsoleOthello.Interfaces.Presenters;
using ConsoleOthello.Interfaces.Repositories;
using ConsoleOthello.Interfaces.UseCases;
using ConsoleOthello.Presenters;
using ConsoleOthello.Repositories;
using ConsoleOthello.Subjects;
using ConsoleOthello.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleOthello;

public class App
{
    public IServiceCollection Services { get; private set; } = new ServiceCollection();
    public IServiceProvider ServiceProvider { get; private set; }

    private App() { }

    public static App Create()
    {
        var app = new App();
        app.ConfigureServices();
        app.ServiceProvider = app.Services.BuildServiceProvider();
        return app;
    }

    private void ConfigureServices()
    {
        // Repositories
        Services.AddScoped<IRoomStateRepository, RoomStateRepository>();
        // Presenters
        Services.AddScoped<IRegisterPlayerPresenter, RegisterPlayerPresenter>();
        Services.AddScoped<IInGamePresenter, InGamePresenter>();
        // Use Cases
        Services.AddScoped<IRegisterPlayerUseCase, RegisterPlayerInteractor>();
        Services.AddScoped<IGetRoomUseCase, GetRoomInteractor>();
        Services.AddScoped<IMovePieceUseCase, MovePieceInteractor>();
        // Controllers
        Services.AddScoped<RegisterPlayerController>();
        Services.AddScoped<InGameController>();
        // Subjects
        Services.AddScoped<RegisterPlayerSubject>();
        Services.AddScoped<BoardSubject>();
        // View
        Services.AddScoped<MainView>();
        Services.AddScoped<RegisterPlayerView>();
        Services.AddScoped<InGameView>();
    }

    public void Run()
    {
        var mainView = ServiceProvider.GetRequiredService<MainView>();
        mainView.Show();

        while (true)
        {
            var commandString = Console.ReadLine();
            Command command;
            try
            {
                command = ParseCommand(commandString);
            }
            catch (ArgumentException ex)
            {
                mainView.ShowError(ex.Message);
                continue;
            }
            catch (Exception ex)
            {
                mainView.ShowError($"予期しないエラーが発生しました: {ex.Message}");
                continue;
            }

            switch (command)
            {
                case Command.start:
                    var registerPlayerView = ServiceProvider.GetRequiredService<RegisterPlayerView>();
                    registerPlayerView.Show();
                    continue;
                case Command.quit:
                    Console.WriteLine("ゲームを終了します。");
                    return;
                default:
                    mainView.ShowError($"無効なコマンド: {commandString}");
                    break;
            }
        }
    }

    private Command ParseCommand(string commandString)
    {
        if (string.IsNullOrWhiteSpace(commandString))
        {
            throw new ArgumentException("コマンドを入力してください．");
        }
        if (Enum.TryParse(commandString.Trim(), true, out Command command))
        {
            return command;
        }
        else
        {
            throw new ArgumentException($"無効なコマンド: {commandString}");
        }
    }

    private enum Command
    {
        start,
        quit
    }
}
