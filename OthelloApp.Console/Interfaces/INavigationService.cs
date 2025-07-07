namespace OthelloApp.ConsoleApp.Interfaces;

public interface INavigationService
{
    void NavigateToMainView();
    void NavigateToRegisterPlayerView();
    void NavigateToInGameView(Guid roomId);
    void NavigateToCpuGameView();
}
