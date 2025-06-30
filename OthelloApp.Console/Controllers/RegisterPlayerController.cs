using OthelloApp.Core.Models.IO;
using OthelloApp.Core.Interfaces.UseCases;
using System;

namespace OthelloApp.ConsoleApp.Controllers;

public class RegisterPlayerController
{
    private readonly IRegisterPlayerUseCase _registerPlayerUseCase;

    public RegisterPlayerController(IRegisterPlayerUseCase registerPlayerUseCase)
    {
        if (registerPlayerUseCase == null)
        {
            throw new ArgumentNullException(nameof(registerPlayerUseCase), "登録プレイヤーのユースケースはnullであってはいけません。");
        }
        _registerPlayerUseCase = registerPlayerUseCase;
    }

    public void RegisterPlayer(string player1Name, string player2Name)
    {
        var inputData = new RegisterPlayerInputData(player1Name, player2Name);
        _registerPlayerUseCase.Handle(inputData);
    }
}
