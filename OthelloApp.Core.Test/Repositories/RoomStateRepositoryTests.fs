namespace OthelloApp.Core.Test.Repositories

open System
open System.Collections.Generic
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Repositories
open OthelloApp.Core.Models

module RoomStateRepositoryTests =

    [<Fact>]
    let ``CreateRoom�V�������[�����쐬`` () =
        let repository = RoomStateRepository()
        
        let roomId = repository.CreateRoom()
        
        Assert.NotEqual(Guid.Empty, roomId)
        let room = repository.GetRoom(roomId)
        Assert.NotNull(room)
        Assert.Equal(roomId, room.Id)

    [<Fact>]
    let ``GetRoom���݂��郋�[�����擾`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.GetRoom(roomId)
        
        Assert.NotNull(room)
        Assert.Equal(roomId, room.Id)
        Assert.False(room.IsFull)
        Assert.True(room.IsPlayer1Turn)

    [<Fact>]
    let ``GetRoom���݂��Ȃ����[���ŗ�O`` () =
        let repository = RoomStateRepository()
        let nonExistentId = Guid.NewGuid()
        
        Assert.Throws<KeyNotFoundException>(fun () -> 
            repository.GetRoom(nonExistentId) |> ignore)

    [<Fact>]
    let ``JoinRoom�v���C���[�����[���ɒǉ�`` () =
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
    let ``JoinRoom���t�̃��[���ŗ�O`` () =
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
    let ``MovePiece�L���Ȏ�ŃQ�[����ԍX�V`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.MovePiece(roomId, DiscType.Player1, 2, 4)
        
        Assert.NotNull(room)
        Assert.Equal(CellType.Player1, room.Board.[2, 4])
        Assert.Equal(4, room.Board.GetPlayer1Count())
        Assert.Equal(1, room.Board.GetPlayer2Count())

    [<Fact>]
    let ``DeleteRoom���[�����폜`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        // ���[�������݂��邱�Ƃ��m�F
        let room = repository.GetRoom(roomId)
        Assert.NotNull(room)
        
        // ���[�����폜
        repository.DeleteRoom(roomId)
        
        // ���[�������݂��Ȃ����Ƃ��m�F
        Assert.Throws<KeyNotFoundException>(fun () -> 
            repository.GetRoom(roomId) |> ignore)

    [<Fact>]
    let ``MovePiece�^�[���؂�ւ�������������`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        
        let room = repository.MovePiece(roomId, DiscType.Player1, 2, 4)
        
        Assert.False(room.IsPlayer1Turn) // �^�[�����؂�ւ��
        Assert.NotNull(room.NextPlayerValidMoves)
        Assert.Equal(DiscType.Player2, room.NextPlayerValidMoves.Player)

    [<Fact>]
    let ``MovePiece�Q�[���I�����̏���`` () =
        let repository = RoomStateRepository()
        let roomId = repository.CreateRoom()
        let player1 = Player("Player1", DiscType.Player1)
        let player2 = Player("Player2", DiscType.Player2)
        repository.JoinRoom(roomId, player1)
        repository.JoinRoom(roomId, player2)
        
        let room = repository.GetRoom(roomId)
        
        // �{�[�h���蓮�Ŗ��t�ɂ��ăQ�[���I����Ԃ����
        for i in 0 .. 7 do
            for j in 0 .. 7 do
                room.Board.[i, j] <- if (i + j) % 2 = 0 then CellType.Player1 else CellType.Player2
        
        // �Ō�̎��ł�
        room.Board.[7, 7] <- CellType.None
        let finalRoom = repository.MovePiece(roomId, DiscType.Player1, 7, 7)
        
        Assert.True(finalRoom.IsGameOver)
        Assert.NotNull(finalRoom.Winner)