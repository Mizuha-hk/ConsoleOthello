using ConsoleOthello.Controllers;
using ConsoleOthello.Entities;
using ConsoleOthello.Exceptions;
using ConsoleOthello.Models.ViewModels;
using ConsoleOthello.Subjects;
using System;

namespace ConsoleOthello.Views;

public class InGameView : ViewBase
{
    private readonly InGameController _controller;
    private readonly BoardSubject _subject;
    private readonly MainView _mainView;

    public InGameView(
        InGameController controller,
        BoardSubject subject,
        MainView mainView)
    {
        _controller = controller ?? throw new System.ArgumentNullException(nameof(controller), "InGameControllerはnullであってはいけません。");
        _subject = subject ?? throw new System.ArgumentNullException(nameof(subject), "BoardSubjectはnullであってはいけません。");
        _mainView = mainView ?? throw new ArgumentNullException(nameof(mainView), "MainViewはnullであってはいけません。");
        _subject.OnInitialized += OnInitialized;
        _subject.OnMovePieceCompleted += OnViewChanged;
    }
    public override void Dispose()
    {
        _subject.OnMovePieceCompleted -= OnViewChanged;
    }

    public void Show(Guid roomId)
    {
        _controller.GetRoom(roomId);
    }

    public override void Show()
    {
        Console.WriteLine("ゲームが開始されました。");
    }

    public override void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"エラー: {message}");
        Console.ResetColor();
    }

    private void OnInitialized(InGameViewModel viewModel)
    {
        ShowBoard(viewModel);
        PieceInput(viewModel);
    }

    private void OnViewChanged(InGameViewModel viewModel)
    {
        if (viewModel == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("予期せぬエラーが発生しました．");
            EndGame();

        }
        Console.Clear();

        ShowBoard(viewModel);

        if (viewModel.IsGameOver)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ゲーム終了！");
            Console.WriteLine($"勝者: {viewModel.Winner?.Name ?? "ドロー"}");
            EndGame();
        }
        else
        {
            PieceInput(viewModel);
        }
    }

    private void ShowBoard(InGameViewModel viewModel)
    {
        Piece piece;
        string tuernPlayerName;
        string tuernPlayerPieve;
        if (viewModel.IsPlayer1Turn)
        {
            piece = Piece.Player1;
            tuernPlayerName = viewModel.Player1.Name;
            tuernPlayerPieve = "●";
        }
        else
        {
            piece = Piece.Player2;
            tuernPlayerName = viewModel.Player2.Name;
            tuernPlayerPieve = "○";
        }
        var moveAvalable = viewModel.Board.IsMoveAvailable(piece);
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("|   | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |");
        Console.WriteLine("-------------------------------------");
        for (int row = 0; row < Board.Size; row++)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"| {row} ");
            for (int col = 0; col < Board.Size; col++)
            {
                var currentPiece = viewModel.Board[row, col];
                Console.ForegroundColor = ConsoleColor.Black;
                switch (currentPiece)
                {
                    case Piece.Player1:
                        Console.Write("| ● ");
                        break;
                    case Piece.Player2:
                        Console.Write("| ○ ");
                        break;
                    case Piece.None:
                        bool isMoveAble = false;
                        for (int i = 0; i < moveAvalable.AvailableMoves.Count; i++)
                        {
                            if (moveAvalable.AvailableMoves[i][0] == row && moveAvalable.AvailableMoves[i][1] == col)
                            {
                                isMoveAble = true;
                                break;
                            }
                        }
                        if (isMoveAble)
                        {
                            Console.Write("| ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("+ ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("|   ");
                        }
                        break;
                    default:
                        Console.Write("|   ");
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("|");
            Console.WriteLine("-------------------------------------");

        }
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" {tuernPlayerName}");
        Console.ResetColor();
        Console.WriteLine($" さんのターン: {tuernPlayerPieve}");
    }

    private void EndGame()
    {
        Console.ResetColor();
        Console.WriteLine("任意のキー入力でタイトルに戻る...");
        Console.ReadLine();
        Console.Clear();
        _mainView.Show();
    }

    private void PieceInput(InGameViewModel viewModel)
    {
        bool isValidInput = false;
        while (!isValidInput)
        {
            Console.WriteLine("行と列を入力してください（例: 3 4）。終了するには 'quit' を入力してください。");
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                ShowError("入力が空です。行と列を入力してください。");
                continue;
            }
            if (input.Trim() == "quit")
            {
                EndGame();
                return;
            }
            var parts = input.Split(' ');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int row) || !int.TryParse(parts[1], out int column))
            {
                ShowError("無効な入力です。行と列を正しく入力してください。");
                continue;
            }
            try
            {
                var piece = viewModel.IsPlayer1Turn ? Piece.Player1 : Piece.Player2;
                isValidInput = true;
                _controller.MovePiece(viewModel.RoomId, piece, row, column);
            }
            catch (BoardOutOfRangeException ex)
            {
                isValidInput = false;
                ShowError(ex.Message);
                continue;
            }
            catch (CantMoveException ex)
            {
                isValidInput = false;
                ShowError(ex.Message);
                continue;
            }
            catch (Exception ex)
            {
                ShowError($"予期しないエラーが発生しました: {ex.Message}");
                EndGame();
            }
        }
    }
}
