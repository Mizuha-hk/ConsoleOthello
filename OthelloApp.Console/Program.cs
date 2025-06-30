using OthelloApp.ConsoleApp.Views;
using OthelloApp.Core;
using Microsoft.Extensions.DependencyInjection;
using OthelloApp.Core.Interfaces.Presenters;
using OthelloApp.ConsoleApp.Presenters;
using OthelloApp.ConsoleApp.Controllers;
using OthelloApp.ConsoleApp.Subjects;
using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.ConsoleApp.Services;
using OthelloApp.ConsoleApp;

var builder = OthelloAppBuilder.CreateBuilder();

// Register Controllers
builder.AddController<RegisterPlayerController>();
builder.AddController<InGameController>();

// Register Presenters
builder.AddPresenter<IRegisterPlayerPresenter, RegisterPlayerPresenter>();
builder.AddPresenter<IInGamePresenter, InGamePresenter>();

// Register View
builder.AddMainView<MainView>();
builder.AddView<RegisterPlayerView>();
builder.AddView<InGameView>();

//Register Subject
builder.AddSubject<BoardSubject>();
builder.AddSubject<RegisterPlayerSubject>();

// Register Navigation Service
builder.Services.AddScoped<INavigationService, NavigationService>();

var app = builder.Build();

app.Run();