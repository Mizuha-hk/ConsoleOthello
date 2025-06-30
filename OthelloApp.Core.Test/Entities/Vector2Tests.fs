namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module Vector2Tests =

    [<Fact>]
    let ``Vector2正常な作成`` () =
        let vector = Vector2(3, 4)
        
        Assert.Equal(3, vector.Row)
        Assert.Equal(4, vector.Column)

    [<Fact>]
    let ``Vector2等価性`` () =
        let vector1 = Vector2(1, 2)
        let vector2 = Vector2(1, 2)
        let vector3 = Vector2(2, 1)
        
        Assert.Equal(vector1, vector2)
        Assert.NotEqual(vector1, vector3)