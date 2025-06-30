using System;

namespace OthelloApp.Core.Models.IO;

public class RegisterPlayerInputData
{
    public string Player1Name { get; set; } = string.Empty;
    public string Player2Name { get; set; } = string.Empty;
    public RegisterPlayerInputData(string player1Name, string player2Name)
    {
        if (string.IsNullOrWhiteSpace(player1Name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。", nameof(player1Name));
        }

        Player1Name = player1Name;
        Player2Name = player2Name;
    }
}
