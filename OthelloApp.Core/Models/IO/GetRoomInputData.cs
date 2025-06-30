using System;

namespace OthelloApp.Core.Models.IO;

public class GetRoomInputData
{
    public Guid RoomId { get; set; }
    public GetRoomInputData(Guid roomId)
    {
        if (roomId == Guid.Empty)
        {
            throw new ArgumentException("ルームIDは空であってはいけません．", nameof(roomId));
        }
        RoomId = roomId;
    }
}
