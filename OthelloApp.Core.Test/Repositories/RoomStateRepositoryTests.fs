namespace OthelloApp.Core.Test.Repositories

open System
open System.Collections.Generic
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Repositories
open OthelloApp.Core.Models

module RoomStateRepositoryTests =

    [<Fact>]
    let ``CreateRoom新しいルームを作成`` () =
        let repository = RoomStateRepository()
        
        let roomId = repository.CreateRoom()
        
        Assert.NotEqual(Guid.Empty, roomId)
        let room = repository.GetRoom(roomId)
        Assert.NotNull(room)
        Assert.Equal(roomId, room.Id)

    [<Fact>]
    let ``GetRoom存在するルームを取得`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.GetRoom(roomId)
        
        Assert.NotNull(room)
        Assert.Equal(roomId, room.Id)
        Assert.False(room.IsFull)
        Assert.True(room.IsPlayer1Turn)

    [<Fact>]
    let ``GetRoom存在しないルームで例外`` () =
        let repository = RoomStateRepository()
        let nonExistentId = Guid.NewGuid()
        
        Assert.Throws<KeyNotFoundException>(fun () -> 
            repository.GetRoom(nonExistentId) |> ignore)

    [<Fact>]
    let ``JoinRoomプレイヤーをルームに追加`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        let player1 = Player("Player1", DiscType.Player1)
        let player2 = Player("Player2", DiscType.Player2)
        
        repository.JoinRoom(roomId, player1)
        let room = repository.GetRoom(roomId)
        Assert.Equal(player1, room.Player1)
        Assert.Null(room.Player2)
        Assert.False(room.IsFull)
        
        repository.JoinRoom(roomId, player2)
        let room2 = repository.GetRoom(roomId)
        Assert.Equal(player1, room2.Player1)
        Assert.Equal(player2, room2.Player2)
        Assert.True(room2.IsFull)

    [<Fact>]
    let ``JoinRoom満杯のルームで例外`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        let player1 = Player("Player1", DiscType.Player1)
        let player2 = Player("Player2", DiscType.Player2)
        let player3 = Player("Player3", DiscType.Player1)
        
        repository.JoinRoom(roomId, player1)
        repository.JoinRoom(roomId, player2)
        
        Assert.Throws<InvalidOperationException>(fun () -> 
            repository.JoinRoom(roomId, player3))

    [<Fact>]
    let ``MovePiece有効な手でゲーム状態更新`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.MovePiece(roomId, DiscType.Player1, 2, 4)
        
        Assert.NotNull(room)
        Assert.Equal(CellType.Player1, room.Board.[2, 4])
        Assert.Equal(4, room.Board.GetPlayer1Count())
        Assert.Equal(1, room.Board.GetPlayer2Count())

    [<Fact>]
    let ``DeleteRoomルームを削除`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        // ルームが存在することを確認
        let room = repository.GetRoom(roomId)
        Assert.NotNull(room)
        
        // ルームを削除
        repository.DeleteRoom(roomId)
        
        // ルームが存在しないことを確認
        Assert.Throws<KeyNotFoundException>(fun () -> 
            repository.GetRoom(roomId) |> ignore)

    [<Fact>]
    let ``MovePieceターン切り替えが正しく動作`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.MovePiece(roomId, DiscType.Player1, 2, 4)
        
        Assert.False(room.IsPlayer1Turn) // ターンが切り替わる
        Assert.NotNull(room.NextPlayerValidMoves)
        Assert.Equal(DiscType.Player2, room.NextPlayerValidMoves.Player)

    [<Fact>]
    let ``MovePieceゲーム終了時の処理`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        let player1 = Player("Player1", DiscType.Player1)
        let player2 = Player("Player2", DiscType.Player2)
        repository.JoinRoom(roomId, player1)
        repository.JoinRoom(roomId, player2)
        
        let room = repository.GetRoom(roomId)
        
        // ボードを手動で満杯にしてゲーム終了状態を作る
        for i in 0 .. 7 do
            for j in 0 .. 7 do
                room.Board.[i, j] <- if (i + j) % 2 = 0 then CellType.Player1 else CellType.Player2
        
        // 最後の手を打つ
        room.Board.[7, 7] <- CellType.None
        let finalRoom = repository.MovePiece(roomId, DiscType.Player1, 7, 7)
        
        Assert.True(finalRoom.IsGameOver)
        Assert.NotNull(finalRoom.Winner)