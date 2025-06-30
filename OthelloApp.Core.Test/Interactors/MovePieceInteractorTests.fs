namespace OthelloApp.Core.Test.Interactors

open System
open Xunit
open OthelloApp.Core.Entities
open OthelloApp.Core.Interactors
open OthelloApp.Core.Models.IO
open OthelloApp.Core.Interfaces.Presenters
open OthelloApp.Core.Interfaces.Repositories
open OthelloApp.Core.Exceptions
open OthelloApp.Core.Models

module MovePieceInteractorTests =

    // �e�X�g�p�̃��b�N�N���X
    type MockInGamePresenter() =
        member val MoveCompleteCallCount = 0 with get, set
        member val LastOutputData : MovePieceOutputData = null with get, set
        
        interface IInGamePresenter with
            member this.GetComplete(_) = ()
            
            member this.MoveComplete(output) =
                this.MoveCompleteCallCount <- this.MoveCompleteCallCount + 1
                this.LastOutputData <- output

    type MockRoomStateRepository() =
        member val MovePieceCallCount = 0 with get, set
        member val TestRoom : Room = Room() with get, set
        
        interface IRoomStateRepository with
            member this.GetRoom(_) = this.TestRoom
            member this.CreateRoom() = Guid.NewGuid()
            member this.JoinRoom(_, _) = ()
            member this.MovePiece(roomId, discType, row, column) =
                this.MovePieceCallCount <- this.MovePieceCallCount + 1
                this.TestRoom.Board.PlaceDisc(row, column, discType)
                this.TestRoom
            member this.DeleteRoom(_) = ()

    [<Fact>]
    let ``MovePieceInteractor����ȃP�[�X`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        let roomId = mockRepository.TestRoom.Id
        
        let interactor = MovePieceInteractor(mockPresenter, mockRepository)
        let input = MovePieceInputData(roomId, DiscType.Player1, 2, 4)
        
        interactor.Handle(input)
        
        Assert.Equal(1, mockPresenter.MoveCompleteCallCount)
        Assert.Equal(1, mockRepository.MovePieceCallCount)
        Assert.NotNull(mockPresenter.LastOutputData)

    [<Fact>]
    let ``MovePieceInteractor��null�v���[���^�[��O`` () =
        let mockRepository = MockRoomStateRepository()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            MovePieceInteractor(null, mockRepository) |> ignore)

    [<Fact>]
    let ``MovePieceInteractor��null���|�W�g����O`` () =
        let mockPresenter = MockInGamePresenter()
        
        Assert.Throws<ArgumentNullException>(fun () -> 
            MovePieceInteractor(mockPresenter, null) |> ignore)

    [<Fact>]
    let ``MovePieceInteractor�Ŕ͈͊O���W��O`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        let roomId = mockRepository.TestRoom.Id
        
        let interactor = MovePieceInteractor(mockPresenter, mockRepository)
        let input = MovePieceInputData(roomId, DiscType.Player1, -1, 3)
        
        Assert.Throws<BoardOutOfRangeException>(fun () -> 
            interactor.Handle(input))

    [<Fact>]
    let ``MovePieceInteractor�ŋ��Guid��O`` () =
        let mockPresenter = MockInGamePresenter()
        let mockRepository = MockRoomStateRepository()
        
        let interactor = MovePieceInteractor(mockPresenter, mockRepository)
        
        Assert.Throws<ArgumentException>(fun () -> 
            let input = MovePieceInputData(Guid.Empty, DiscType.Player1, 2, 3)
            interactor.Handle(input))