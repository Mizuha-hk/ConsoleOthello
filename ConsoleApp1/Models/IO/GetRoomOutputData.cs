using System;

namespace ConsoleOthello.Models.IO;

public class GetRoomOutputData
{
    public Room Room { get; set; }
    public GetRoomOutputData(Room room)
    {
        if (room == null)
        {
            throw new ArgumentNullException(nameof(room), "ルームはnullであってはいけません．");
        }
        Room = room;
    }
}
