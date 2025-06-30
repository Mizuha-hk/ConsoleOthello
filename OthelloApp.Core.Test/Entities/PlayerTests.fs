namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module PlayerTests =

    [<Fact>]
    let ``Player正常な作成`` () =
        let player = Player("TestPlayer", DiscType.Player1)
        
        Assert.Equal("TestPlayer", player.Name)
        Assert.Equal(DiscType.Player1, player.DiscType)

    [<Fact>]
    let ``Player空の名前で例外が発生する`` () =
        Assert.Throws<ArgumentException>(fun () -> Player("", DiscType.Player1) |> ignore)
        Assert.Throws<ArgumentException>(fun () -> Player(" ", DiscType.Player1) |> ignore)
        Assert.Throws<ArgumentException>(fun () -> Player(null, DiscType.Player1) |> ignore)

    [<Fact>]
    let ``PlayerのDiscType設定が正しい`` () =
        let player1 = Player("Player1", DiscType.Player1)
        let player2 = Player("Player2", DiscType.Player2)
        
        Assert.Equal(DiscType.Player1, player1.DiscType)
        Assert.Equal(DiscType.Player2, player2.DiscType)

    [<Fact>]
    let ``Player名前のトリム処理`` () =
        let player = Player("  TestPlayer  ", DiscType.Player1)
        Assert.Equal("  TestPlayer  ", player.Name) // トリムされない仕様の確認