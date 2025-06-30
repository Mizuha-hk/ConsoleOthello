namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Exceptions

module BoardTests =

    [<Fact>]
    let ``Board初期化時の状態が正しい`` () =
        let board = Board()
        
        // 初期配置の確認
        Assert.Equal(CellType.Player1, board.[3, 3])
        Assert.Equal(CellType.Player2, board.[3, 4])
        Assert.Equal(CellType.Player2, board.[4, 3])
        Assert.Equal(CellType.Player1, board.[4, 4])
        
        // プレイヤーカウントの確認
        Assert.Equal(2, board.GetPlayer1Count())
        Assert.Equal(2, board.GetPlayer2Count())
        
        // 他のセルは空であることの確認
        Assert.Equal(CellType.None, board.[0, 0])
        Assert.Equal(CellType.None, board.[7, 7])

    [<Fact>]
    let ``Indexer範囲外アクセスで例外が発生する`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[-1, 0] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, -1] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[8, 0] |> ignore) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, 8] |> ignore) |> ignore

    [<Fact>]
    let ``Indexer設定で範囲外アクセス時に例外が発生する`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[-1, 0] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, -1] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[8, 0] <- CellType.Player1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.[0, 8] <- CellType.Player1) |> ignore

    [<Fact>]
    let ``PlaceDisc範囲外座標で例外が発生する`` () =
        let board = Board()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(-1, 0, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(0, -1, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(8, 0, DiscType.Player1)) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> board.PlaceDisc(0, 8, DiscType.Player1)) |> ignore

    [<Fact>]
    let ``PlaceDisc無効な位置で例外が発生する`` () =
        let board = Board()
        
        // 既に駒が置かれている場所
        Assert.Throws<CantMoveException>(fun () -> board.PlaceDisc(3, 3, DiscType.Player1)) |> ignore
        
        // 有効でない空のセル
        Assert.Throws<CantMoveException>(fun () -> board.PlaceDisc(0, 0, DiscType.Player1)) |> ignore

    [<Fact>]
    let ``PlaceDisc有効な位置に駒を置ける`` () =
        let board = Board()
        
        // Player1の有効な手: (2,4)
        board.PlaceDisc(2, 4, DiscType.Player1)
        
        Assert.Equal(CellType.Player1, board.[2, 4])
        Assert.Equal(CellType.Player1, board.[3, 3]) // 既存
        Assert.Equal(CellType.Player1, board.[3, 4]) // フリップされた
        
        // カウントの確認
        Assert.Equal(4, board.GetPlayer1Count())
        Assert.Equal(1, board.GetPlayer2Count())

    [<Fact>]
    let ``AllValidMoves初期状態でPlayer1の有効手を取得`` () =
        let board = Board()
        let validMoves = board.AllValidMoves(DiscType.Player1)
        
        Assert.Equal(DiscType.Player1, validMoves.Player)
        Assert.Equal(4, validMoves.MovableCells.Count)
        
        // 初期状態の有効手: (2,4), (3,5), (4,2), (5,3)
        let moves = validMoves.MovableCells |> Seq.map (fun m -> (m.Row, m.Column)) |> Seq.toList |> List.sort
        let expected = [(2, 4); (3, 5); (4, 2); (5, 3)] |> List.sort
        Assert.Equal<int * int>(expected, moves)

    [<Fact>]
    let ``AllValidMoves初期状態でPlayer2の有効手を取得`` () =
        let board = Board()
        let validMoves = board.AllValidMoves(DiscType.Player2)
        
        Assert.Equal(DiscType.Player2, validMoves.Player)
        Assert.Equal(4, validMoves.MovableCells.Count)
        
        // 初期状態の有効手: (2,3), (3,2), (4,5), (5,4)
        let moves = validMoves.MovableCells |> Seq.map (fun m -> (m.Row, m.Column)) |> Seq.toList |> List.sort
        let expected = [(2, 3); (3, 2); (4, 5); (5, 4)] |> List.sort
        Assert.Equal<int * int>(expected, moves)

    [<Fact>]
    let ``PlaceDisc後のカウント変更が正しい`` () =
        let board = Board()
        
        // 初期状態: Player1=2, Player2=2
        Assert.Equal(2, board.GetPlayer1Count())
        Assert.Equal(2, board.GetPlayer2Count())
        
        // Player1が(2,4)に置く（Player2の(3,4)をフリップ）
        board.PlaceDisc(2, 4, DiscType.Player1)
        
        Assert.Equal(4, board.GetPlayer1Count())
        Assert.Equal(1, board.GetPlayer2Count())

    [<Fact>]
    let ``複数方向のディスクフリップが正しく動作する`` () =
        let board = Board()
        
        // テストのために特定の状態を設定
        board.[2, 4] <- CellType.Player1
        board.[3, 3] <- CellType.Player1
        board.[3, 4] <- CellType.Player1
        board.[2, 5] <- CellType.Player2
        board.[3, 5] <- CellType.Player2
        
        // Player2が(2,3)に置く（複数方向のフリップ）
        let initialP1Count = board.GetPlayer1Count()
        let initialP2Count = board.GetPlayer2Count()
        
        board.PlaceDisc(2, 3, DiscType.Player2)
        
        // (3,3)に置かれ、(2,4)と(3,3)がフリップされる
        Assert.Equal(CellType.Player2, board.[2, 3])
        Assert.Equal(CellType.Player2, board.[2, 4])
        Assert.Equal(CellType.Player2, board.[3, 3])