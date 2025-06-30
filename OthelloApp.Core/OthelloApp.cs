using Microsoft.Extensions.DependencyInjection;
using OthelloApp.Core.Interactors;
using OthelloApp.Core.Interfaces.Repositories;
using OthelloApp.Core.Interfaces.UseCases;
using OthelloApp.Core.Interfaces.View;
using OthelloApp.Core.Repositories;
using System;

namespace OthelloApp.Core;

public class OthelloApp
{
    private IMainView _mainView;

    public OthelloApp(IMainView mainView) 
    {
        _mainView = mainView ?? throw new ArgumentNullException(nameof(mainView), "MainViewはnullであってはいけません。");
    }

    public void Run()
    {
        _mainView.Initialize();
    }
}
