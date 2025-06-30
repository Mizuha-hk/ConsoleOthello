namespace OthelloApp.Core.Test.Interactors

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Interactors
open OthelloApp.Core.Models.IO
open OthelloApp.Core.Interfaces.Presenters
open OthelloApp.Core.Interfaces.Repositories
open OthelloApp.Core.Models

module RegisterPlayerInteractorTests =

    // テスト用のモッククラス
    type MockRegisterPlayerPresenter() =
        member val CompleteCallCount = 0 with get, set
        member val LastOutputData : RegisterPlayerOutputData = null with get, set
        
        interface IRegisterPlayerPresenter with
            member this.Complete(output) =
                this.CompleteCallCount <- this.CompleteCallCount + 1
                this.LastOutputData <- output

    type MockRoomStateRepository() =
        member val CreateRoomCallCount = 0 with get, set
        member val JoinRoomCallCount = 0 with get, set
        member val CreatedRoomId = Guid.NewGuid() with get, set
        
        interface IRoomStateRepository with
            member this.CreateRoom() =
                this.CreateRoomCallCount <- this.CreateRoomCallCount + 1
                this.CreatedRoomId
            
            member this.JoinRoom(_, _) =
                this.JoinRoomCallCount <- this.JoinRoomCallCount + 1
            
            member this.GetRoom(_) = Room()
            member this.MovePiece(_, _, _, _) = Room()
            member this.DeleteRoom(_) = ()

    [<Fact>]
    let ``RegisterPlayerInteractor正常なケース`` () =
        let mockPresenter = MockRegisterPlayerPresenter()
        let mockRepository = MockRoomStateRepository()
        
        let interactor = RegisterPlayerInteractor(mockPresenter, mockRepository)
        let player1Name = "Player1"
        let player2Name = "Player2"
        let input = RegisterPlayerInputData(player1Name, player2Name)
        
        interactor.Handle(input)
        
        Assert.Equal(1, mockPresenter.CompleteCallCount)
        Assert.Equal(1, mockRepository.CreateRoomCallCount)
        Assert.Equal(2, mockRepository.JoinRoomCallCount)
        Assert.NotNull(mockPresenter.LastOutputData)

    [<Fact>]
    let ``RegisterPlayerInteractorでnullプレゼンター例外`` () =
        let mockRepository = MockRoomStateRepository()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            RegisterPlayerInteractor(null, mockRepository) |> ignore)

    [<Fact>]
    let ``RegisterPlayerInteractorでnullリポジトリ例外`` () =
        let mockPresenter = MockRegisterPlayerPresenter()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            RegisterPlayerInteractor(mockPresenter, null) |> ignore)

    [<Fact>]
    let ``RegisterPlayerInteractorで空のプレイヤー1名例外`` () =
        let mockPresenter = MockRegisterPlayerPresenter()
        let mockRepository = MockRoomStateRepository()
        
        let interactor = RegisterPlayerInteractor(mockPresenter, mockRepository)
        let player1Name = ""  // 空の名前
        let player2Name = "Player2"
        
        Assert.Throws<ArgumentException>(fun () -> 
            let input = RegisterPlayerInputData(player1Name, player2Name)
            interactor.Handle(input))

    [<Fact>]
    let ``RegisterPlayerInteractorで空のプレイヤー2名例外`` () =
        let mockPresenter = MockRegisterPlayerPresenter()
        let mockRepository = MockRoomStateRepository()
        
        let interactor = RegisterPlayerInteractor(mockPresenter, mockRepository)
        let player1Name = "Player1"
        let player2Name = ""  // 空の名前
        
        Assert.Throws<ArgumentException>(fun () -> 
            let input = RegisterPlayerInputData(player1Name, player2Name)
            interactor.Handle(input))