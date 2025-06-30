using System;
namespace OthelloApp.Core.Entities;

public class Player
{
    public string Name { get; set; }
    public DiscType DiscType { get; set; }

    public Player(string name, DiscType piece)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。");
        }

        Name = name;
        DiscType = piece;
    }
}
