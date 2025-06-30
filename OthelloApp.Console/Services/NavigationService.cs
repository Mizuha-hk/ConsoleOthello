using Microsoft.Extensions.DependencyInjection;
using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.ConsoleApp.Views;

namespace OthelloApp.ConsoleApp.Services;

internal class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;
    private ViewBase? _currentView;
    public NavigationService(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services), "サービスプロバイダーはnullであってはいけません。");
    }
    public void NavigateToInGameView(Guid roomId)
    {
        DisposeCurrentView();

        var inGameView = _services.GetRequiredService<InGameView>();
        _currentView = inGameView;
        inGameView.Initialize(roomId);
    }

    public void NavigateToMainView()
    {
        DisposeCurrentView();
        var mainView = _services.GetRequiredService<MainView>();
        mainView.Initialize();
    }

    public void NavigateToRegisterPlayerView()
    {
        DisposeCurrentView();
        var registerPlayerView = _services.GetRequiredService<RegisterPlayerView>();
        registerPlayerView.Initialize();
    }

    private void DisposeCurrentView()
    {
        _currentView?.Dispose();
        _currentView = null;
    }
}
