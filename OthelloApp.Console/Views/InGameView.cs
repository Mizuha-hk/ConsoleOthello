using OthelloApp.ConsoleApp.Controllers;
using OthelloApp.ConsoleApp.Interfaces;
using OthelloApp.ConsoleApp.Subjects;
using OthelloApp.ConsoleApp.ViewModels;
using OthelloApp.Core.Entities;
using OthelloApp.Core.Exceptions;

namespace OthelloApp.ConsoleApp.Views;

public class InGameView : ViewBase
{
    private readonly InGameController _controller;
    private readonly BoardSubject _subject;
    private readonly INavigationService _navigationService;

    public InGameView(
        InGameController controller,
        BoardSubject subject,
        INavigationService navigationService)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller), "InGameControllerはnullであってはいけません。");
        _subject = subject ?? throw new ArgumentNullException(nameof(subject), "BoardSubjectはnullであってはいけません。");
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "MainViewはnullであってはいけません。");
        _subject.OnInitialized += OnInitialized;
        _subject.OnMovePieceCompleted += OnViewChanged;
    }
    public override void Dispose()
    {
        if(_subject != null)
        {
            _subject.OnInitialized -= OnInitialized;
            _subject.OnMovePieceCompleted -= OnViewChanged;
        }
    }

    public void Initialize(Guid roomId)
    {
        _controller.GetRoom(roomId);
    }

    protected override void Show()
    {
        Console.WriteLine("ゲームが開始されました。");
    }
    protected override void ShowError(string message)
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
            return;
        }
        Console.Clear();

        if (!viewModel.IsGameOver && viewModel.PassedLastTurn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{viewModel.TurnPlayer.Name} さんがパスされました。");
        }

        ShowBoard(viewModel);

        if (viewModel.IsGameOver)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ゲーム終了！");
            Console.WriteLine($"勝者: {viewModel.Winner?.Name ?? "ドロー"}");
            Console.WriteLine($"{viewModel.Player1.Name}: {viewModel.Player1Count} コマ");
            Console.WriteLine($"{viewModel.Player2.Name}: {viewModel.Player2Count} コマ");
            EndGame();
        }
        else
        {
            PieceInput(viewModel);
        }
    }

    private void ShowBoard(InGameViewModel viewModel)
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("-------------------------------------");
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
                    case CellType.Player1:
                        Console.Write("| ● ");
                        break;
                    case CellType.Player2:
                        Console.Write("| ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("● ");
                        break;
                    case CellType.None:
                        var isMoveAble 
                            = viewModel.ValidMoves?.MovableCells
                                .Find(c => c.Row == row && c.Column == col) != null;

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
        if (!viewModel.IsGameOver)
        {
            var color = viewModel.ValidMoves.Player == DiscType.Player1
                ? ConsoleColor.Black
                : ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {viewModel.TurnPlayer.Name}");
            Console.ResetColor();
            Console.Write($" さんのターン:");
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" ● ");
            Console.ResetColor();
        }
    }

    private void EndGame()
    {
        Console.ResetColor();
        Console.WriteLine("任意のキー入力でタイトルに戻る...");
        Console.ReadLine();
        Console.Clear();
        _navigationService.NavigateToMainView();
    }

    private void PieceInput(InGameViewModel viewModel)
    {
        bool isValidInput = false;
        while (!isValidInput)
        {
            Console.WriteLine("行と列を入力してください（例: 3 4）。終了するには 'quit' を入力してください。");
            Console.WriteLine();
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
                var disc = viewModel.TurnPlayer.DiscType;
                isValidInput = true;
                _controller.MovePiece(viewModel.RoomId, disc, row, column);
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
