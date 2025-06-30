namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Exceptions

module BoardTests =

    [<Fact>]
    let ``Board���������̏�Ԃ�������`` () =
        let board = Board()
        
        // �����z�u�̊m�F
        Assert.Equal(CellType.Player1, board.[3, 3])
        Assert.Equal(CellType.Player2, board.[3, 4])
        Assert.Equal(CellType.Player2, board.[4, 3])
        Assert.Equal(CellType.Player1, board.[4, 4])
        
        // �v���C���[�J�E���g�̊m�F
        Assert.Equal(2, board.GetPlayer1Count())
        Assert.Equal(2, board.GetPlayer2Count())
        
        // ���̃Z���͋�ł��邱�Ƃ̊m�F
        Assert.Equal(CellType.None, board.[0, 0])
        Assert.Equal(CellType.None, board.[7, 7])

    [<Fact>]
    let ``Indexer�͈͊O�A�N�Z�X�ŗ�O����������`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[-1, 0] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, -1] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[8, 0] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, 8] |> ignore) |> ignore

    [<Fact>]
    let ``Indexer�ݒ�Ŕ͈͊O�A�N�Z�X���ɗ�O����������`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[-1, 0] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, -1] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[8, 0] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, 8] <- CellType.Player1) |> ignore

    [<Fact>]
    let ``PlaceDisc�͈͊O���W�ŗ�O����������`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(-1, 0, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(0, -1, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(8, 0, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(0, 8, DiscType.Player1)) |> ignore

    [<Fact>]
    let ``PlaceDisc�����Ȉʒu�ŗ�O����������`` () =
        let board = Board()
        
        // ���ɋ�u����Ă���ꏊ
        Assert.Throws<CantMoveException>(fun () -> board.PlaceDisc(3, 3, DiscType.Player1)) |> ignore
        
        // �L���łȂ���̃Z��
        Assert.Throws<CantMoveException>(fun () -> board.PlaceDisc(0, 0, DiscType.Player1)) |> ignore

    [<Fact>]
    let ``PlaceDisc�L���Ȉʒu�ɋ��u����`` () =
        let board = Board()
        
        // Player1�̗L���Ȏ�: (2,4)
        board.PlaceDisc(2, 4, DiscType.Player1)
        
        Assert.Equal(CellType.Player1, board.[2, 4])
        Assert.Equal(CellType.Player1, board.[3, 3]) // ����
        Assert.Equal(CellType.Player1, board.[3, 4]) // �t���b�v���ꂽ
        
        // �J�E���g�̊m�F
        Assert.Equal(4, board.GetPlayer1Count())
        Assert.Equal(1, board.GetPlayer2Count())

    [<Fact>]
    let ``AllValidMoves������Ԃ�Player1�̗L������擾`` () =
        let board = Board()
        let validMoves = board.AllValidMoves(DiscType.Player1)
        
        Assert.Equal(DiscType.Player1, validMoves.Player)
        Assert.Equal(4, validMoves.MovableCells.Count)
        
        // ������Ԃ̗L����: (2,4), (3,5), (4,2), (5,3)
        let moves = validMoves.MovableCells |> Seq.map (fun m -> (m.Row, m.Column)) |> Seq.toList |> List.sort
        let expected = [(2, 4); (3, 5); (4, 2); (5, 3)] |> List.sort
        Assert.Equal<int * int>(expected, moves)

    [<Fact>]
    let ``AllValidMoves������Ԃ�Player2�̗L������擾`` () =
        let board = Board()
        let validMoves = board.AllValidMoves(DiscType.Player2)
        
        Assert.Equal(DiscType.Player2, validMoves.Player)
        Assert.Equal(4, validMoves.MovableCells.Count)
        
        // ������Ԃ̗L����: (2,3), (3,2), (4,5), (5,4)
        let moves = validMoves.MovableCells |> Seq.map (fun m -> (m.Row, m.Column)) |> Seq.toList |> List.sort
        let expected = [(2, 3); (3, 2); (4, 5); (5, 4)] |> List.sort
        Assert.Equal<int * int>(expected, moves)

    [<Fact>]
    let ``PlaceDisc��̃J�E���g�ύX��������`` () =
        let board = Board()
        
        // �������: Player1=2, Player2=2
        Assert.Equal(2, board.GetPlayer1Count())
        Assert.Equal(2, board.GetPlayer2Count())
        
        // Player1��(2,4)�ɒu���iPlayer2��(3,4)���t���b�v�j
        board.PlaceDisc(2, 4, DiscType.Player1)
        
        Assert.Equal(4, board.GetPlayer1Count())
        Assert.Equal(1, board.GetPlayer2Count())

    [<Fact>]
    let ``���������̃f�B�X�N�t���b�v�����������삷��`` () =
        let board = Board()
        
        // �e�X�g�̂��߂ɓ���̏�Ԃ�ݒ�
        board.[2, 4] <- CellType.Player1
        board.[3, 3] <- CellType.Player1
        board.[3, 4] <- CellType.Player1
        board.[2, 5] <- CellType.Player2
        board.[3, 5] <- CellType.Player2
        
        // Player2��(2,3)�ɒu���i���������̃t���b�v�j
        let initialP1Count = board.GetPlayer1Count()
        let initialP2Count = board.GetPlayer2Count()
        
        board.PlaceDisc(2, 3, DiscType.Player2)
        
        // (3,3)�ɒu����A(2,4)��(3,3)���t���b�v�����
        Assert.Equal(CellType.Player2, board.[2, 3])
        Assert.Equal(CellType.Player2, board.[2, 4])
        Assert.Equal(CellType.Player2, board.[3, 3])