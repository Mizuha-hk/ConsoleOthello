using Microsoft.Extensions.DependencyInjection;
using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.ConsoleApp.Views;

namespace OthelloApp.ConsoleApp.Services;

internal class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;
    public NavigationService(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services), "サービスプロバイダーはnullであってはいけません。");
    }
    public void NavigateToInGameView(Guid roomId)
    {
        var inGameView = _services.GetService<InGameView>();
        if (inGameView == null)
        {
            throw new InvalidOperationException("InGameViewがサービスプロバイダーから取得できませんでした。");
        }
        inGameView.Initialize(roomId);
    }

    public void NavigateToMainView()
    {
        var mainView = _services.GetService<MainView>();
        if (mainView == null)
        {
            throw new InvalidOperationException("MainViewがサービスプロバイダーから取得できませんでした。");
        }
        mainView.Initialize();
    }

    public void NavigateToRegisterPlayerView()
    {
        var registerPlayerView = _services.GetService<RegisterPlayerView>();
        if (registerPlayerView == null) 
        {
            throw new InvalidOperationException("RegisterPlayerViewがサービスプロバイダーから取得できませんでした。");
        }
        registerPlayerView.Initialize();
    }
}
