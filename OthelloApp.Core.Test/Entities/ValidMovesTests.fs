namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module ValidMovesTests =

    [<Fact>]
    let ``ValidMoves������`` () =
        let validMoves = ValidMoves()
        
        Assert.Empty(validMoves.MovableCells)
        Assert.Equal(DiscType.Player1, validMoves.Player) // �f�t�H���g�l

    [<Fact>]
    let ``ValidMoves�v���C���[�ݒ�`` () =
        let validMoves = ValidMoves()
        validMoves.Player <- DiscType.Player2
        
        Assert.Equal(DiscType.Player2, validMoves.Player)

    [<Fact>]
    let ``ValidMovesMovableCells�ǉ�`` () =
        let validMoves = ValidMoves()
        let validMove = ValidMove()
        validMove.Row <- 2
        validMove.Column <- 3
        
        validMoves.MovableCells.Add(validMove)
        
        Assert.Single(validMoves.MovableCells)
        Assert.Equal(validMove, validMoves.MovableCells.[0])