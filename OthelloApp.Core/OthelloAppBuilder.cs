using Microsoft.Extensions.DependencyInjection;
using OthelloApp.Core.Interactors;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Interfaces.UseCases;
using OthelloApp.Core.Interfaces.View;
using OthelloApp.Core.Repositories;

namespace OthelloApp.Core;

public class OthelloAppBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    private OthelloAppBuilder()
    {
        Services.AddSingleton(this);
    
        // Repositories
        Services.AddScoped<IRoomStateRepository, RoomStateRepository>();
        // Use Cases
        Services.AddScoped<IRegisterPlayerUseCase, RegisterPlayerInteractor>();
        Services.AddScoped<IGetRoomUseCase, GetRoomInteractor>();
        Services.AddScoped<IMovePieceUseCase, MovePieceInteractor>();
    }

    public static OthelloAppBuilder CreateBuilder()
    {
        return new OthelloAppBuilder();
    }

    public OthelloAppBuilder AddMainView<T>() where T : class, IMainView
    {
        Services.AddTransient<IMainView, T>();
        return this;
    }

    public OthelloApp Build()
    {
        Services.AddSingleton<OthelloApp>();
        var provider = Services.BuildServiceProvider();

        return provider.GetService<OthelloApp>();
    }
}
