namespace OthelloApp.Core.Test.Interactors

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Interactors
open OthelloApp.Core.Models.IO
open OthelloApp.Core.Interfaces.Presenters
open OthelloApp.Core.Interfaces.Repositories
open OthelloApp.Core.Models

module GetRoomInteractorTests =

    // �e�X�g�p�̃��b�N�N���X
    type MockInGamePresenter() =
        member val GetCompleteCallCount = 0 with get, set
        member val LastOutputData : GetRoomOutputData = null with get, set
        
        interface IInGamePresenter with
            member this.GetComplete(output) =
                this.GetCompleteCallCount <- this.GetCompleteCallCount + 1
                this.LastOutputData <- output
            
            member this.MoveComplete(_) = ()

    type MockRoomStateRepository() =
        member val GetRoomCallCount = 0 with get, set
        member val TestRoom : Room = Room() with get, set
        
        interface IRoomStateRepository with
            member this.GetRoom(roomId) =
                this.GetRoomCallCount <- this.GetRoomCallCount + 1
                if roomId = this.TestRoom.Id then this.TestRoom else null
            
            member this.CreateRoom() = Guid.NewGuid()
            member this.JoinRoom(_, _) = ()
            member this.MovePiece(_, _, _, _) = this.TestRoom
            member this.DeleteRoom(_) = ()

    [<Fact>]
    let ``GetRoomInteractor����ȃP�[�X`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        let roomId = mockRepository.TestRoom.Id
        
        let interactor = GetRoomInteractor(mockRepository, mockPresenter)
        let input = GetRoomInputData(roomId)
        
        interactor.Handle(input)
        
        Assert.Equal(1, mockPresenter.GetCompleteCallCount)
        Assert.Equal(1, mockRepository.GetRoomCallCount)
        Assert.NotNull(mockPresenter.LastOutputData)

    [<Fact>]
    let ``GetRoomInteractor��null���|�W�g����O`` () =
        let mockPresenter = MockInGamePresenter()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            GetRoomInteractor(null, mockPresenter) |> ignore)

    [<Fact>]
    let ``GetRoomInteractor��null�v���[���^�[��O`` () =
        let mockRepository = MockRoomStateRepository()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            GetRoomInteractor(mockRepository, null) |> ignore)

    [<Fact>]
    let ``GetRoomInteractor��null���͗�O`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        let interactor = GetRoomInteractor(mockRepository, mockPresenter)
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            interactor.Handle(null))

    [<Fact>]
    let ``GetRoomInteractor�ő��݂��Ȃ����[����O`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        let interactor = GetRoomInteractor(mockRepository, mockPresenter)
        let input = GetRoomInputData(Guid.NewGuid())
        
        Assert.Throws<InvalidOperationException>(fun () -> 
            interactor.Handle(input))