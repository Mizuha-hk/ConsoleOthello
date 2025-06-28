using ConsoleOthello.Entities;
using System;

namespace ConsoleOthello.Models.IO;

public class RegisterPlayerOutputData
{
    public Guid RoomId { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }

    public RegisterPlayerOutputData(Guid roomId, Player player1, Player player2)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません．", nameof(roomId));
        }
        if (player1 == null)
        {
            throw new ArgumentException("プレイヤーはnullであってはいけません．");
        }
        if (player2 == null)
        {
            throw new ArgumentException("プレイヤーはnullであってはいけません．");
        }

        RoomId = roomId;
        Player1 = player1;
        Player2 = player2;
    }
}
