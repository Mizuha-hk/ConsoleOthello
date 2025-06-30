namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module ValidMovesTests =

    [<Fact>]
    let ``ValidMoves初期化`` () =
        let validMoves = ValidMoves()
        
        Assert.Empty(validMoves.MovableCells)
        Assert.Equal(DiscType.Player1, validMoves.Player) // デフォルト値

    [<Fact>]
    let ``ValidMovesプレイヤー設定`` () =
        let validMoves = ValidMoves()
        validMoves.Player <- DiscType.Player2
        
        Assert.Equal(DiscType.Player2, validMoves.Player)

    [<Fact>]
    let ``ValidMovesMovableCells追加`` () =
        let validMoves = ValidMoves()
        let validMove = ValidMove()
        validMove.Row <- 2
        validMove.Column <- 3
        
        validMoves.MovableCells.Add(validMove)
        
        Assert.Single(validMoves.MovableCells)
        Assert.Equal(validMove, validMoves.MovableCells.[0])