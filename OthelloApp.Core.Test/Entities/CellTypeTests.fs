namespace OthelloApp.Core.Test.Entities

open System
open Xunit
open OthelloApp.Core.Entities

module CellTypeTests =

    [<Fact>]
    let ``CellType�񋓒l�̊m�F`` () =
        Assert.Equal(0, int CellType.None)
        Assert.Equal(1, int CellType.Player1)
        Assert.Equal(2, int CellType.Player2)

    [<Fact>]
    let ``CellType��r`` () =
        Assert.True(CellType.None <> CellType.Player1)
        Assert.True(CellType.Player1 <> CellType.Player2)
        Assert.True(CellType.None = CellType.None)