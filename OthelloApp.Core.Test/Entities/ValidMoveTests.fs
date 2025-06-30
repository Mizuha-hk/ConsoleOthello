namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Exceptions

module ValidMoveTests =

    [<Fact>]
    let ``ValidMove正常な作成`` () =
        let validMove = ValidMove()
        validMove.Row <- 2
        validMove.Column <- 3
        
        Assert.Equal(2, validMove.Row)
        Assert.Equal(3, validMove.Column)
        Assert.Empty(validMove.Directions)

    [<Fact>]
    let ``ValidMove範囲外の座標で例外が発生する`` () =
        let validMove = ValidMove()
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> validMove.Row <- -1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> validMove.Row <- 8) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> validMove.Column <- -1) |> ignore
        Assert.Throws<BoardOutOfRangeException>(fun () -> validMove.Column <- 8) |> ignore

    [<Fact>]
    let ``ValidMoveDirections初期化`` () =
        let validMove = ValidMove()
        
        Assert.NotNull(validMove.Directions)
        Assert.Empty(validMove.Directions)

    [<Fact>]
    let ``ValidMoveDirections追加`` () =
        let validMove = ValidMove()
        let direction = Vector2(1, 0)
        
        validMove.Directions.Add(direction)
        
        Assert.Single(validMove.Directions)
        Assert.Equal(direction, validMove.Directions.[0])