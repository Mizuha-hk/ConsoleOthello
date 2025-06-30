namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module DiscTypeTests =

    [<Fact>]
    let ``DiscType—ñ‹“’l‚ÌŠm”F`` () =
        Assert.Equal(0, int DiscType.Player1)
        Assert.Equal(1, int DiscType.Player2)

    [<Fact>]
    let ``DiscType”äŠr`` () =
        Assert.True(DiscType.Player1 <> DiscType.Player2)
        Assert.True(DiscType.Player1 = DiscType.Player1)