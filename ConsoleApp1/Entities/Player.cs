using System;
namespace ConsoleOthello.Entities;

public class Player
{
    public string Name { get; set; }
    public Piece Piece { get; set; }

    public Player(string name, Piece piece)
    {
        if(piece != Piece.Player1 && piece != Piece.Player2)
        {
            throw new ArgumentException("プレイヤーのコマのタイプはPlayer1またはPlayer2である必要があります。");
        }
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("プレイヤー名は空であってはいけません。");
        }

        Name = name;
        Piece = piece;
    }
}
