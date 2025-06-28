using ConsoleOthello.Exceptions;
using System.Collections.Generic;

namespace ConsoleOthello.Entities;

public class Board
{
    private Piece[,] board;
    private int player1Count = 0;
    private int player2Count = 0;

    public const int Size = 8;
    private static readonly int[,] directions = new int[,]
    {
        {-1, -1}, // Top-left
        {-1, 0},  // Top
        {-1, 1},  // Top-right
        {0, -1},  // Left
        {0, 1},   // Right
        {1, -1},  // Bottom-left
        {1, 0},   // Bottom
        {1, 1}    // Bottom-right
    };
    private struct MovableInfo
    {
        public bool IsValidMove;
        public List<int[]> FlipableDirections;
    }

    public Board()
    {
        board = new Piece[Size, Size];

        FillBoard();

        board[3, 3] = Piece.Player1; 
        board[3, 4] = Piece.Player2;
        board[4, 3] = Piece.Player2;
        board[4, 4] = Piece.Player1;
        player1Count = 2;
        player2Count = 2;
    }

    public Piece this[int row, int col]
    {
        get
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                throw new BoardOutOfRangeException();
            }
            return board[row, col];
        }
        set
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                throw new BoardOutOfRangeException();
            }
            board[row, col] = value;
        }
    }

    public int GetPlayer1Count() => player1Count;
    public int GetPlayer2Count() => player2Count;

    public void MovePiece(int row, int col, Piece piece)
    {
        if (row < 0 || row >= Size || col < 0 || col >= Size)
        {
            throw new BoardOutOfRangeException();
        }

        var movableInfo = IsValidateMove(row, col, piece);
        if (!movableInfo.IsValidMove)
        {
            throw new CantMoveException();
        }

        FlipPieces(row, col, piece, movableInfo);
    }

    public MoveAvalableModel IsMoveAvailable(Piece piece)
    {
        MoveAvalableModel moveAvalableModel = new();
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                if (board[row, col] == Piece.None)
                {
                    var movableInfo = IsValidateMove(row, col, piece);
                    if (movableInfo.IsValidMove)
                    {
                        moveAvalableModel.IsMoveAvailable = true;
                        moveAvalableModel.AvailableMoves.Add(new int[] { row, col });
                    }
                }
            }
        }
        return moveAvalableModel;
    }

    private MovableInfo IsValidateMove(int row, int col, Piece piece)
    {
        if (board[row, col] != Piece.None)
        {
            return new MovableInfo { 
                IsValidMove = false, FlipableDirections = new List<int[]>() 
            };
        }

        bool validMove = false;
        List<int[]> flipableDirections = new();
        var oppositePiece = piece == Piece.Player1 ? Piece.Player2 : Piece.Player1;

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int directionRow = directions[i, 0];
            int directionCol = directions[i, 1];
            int r = row + directionRow;
            int c = col + directionCol;

            while (r >= 0 && r < Size && c >= 0 && c < Size && board[r, c] == oppositePiece)
            {

                r += directionRow;
                c += directionCol;

                if(r < 0 || r >= Size || c < 0 || c >= Size)
                {
                    break;
                }

                if (board[r, c] == piece)
                {
                    flipableDirections.Add(new int[] { directionRow, directionCol });
                    validMove = true;
                    break;
                }
            }
        }

        return new MovableInfo{
            IsValidMove = validMove, FlipableDirections = flipableDirections 
        };
    }

    private void FlipPieces(int row, int col, Piece piece, MovableInfo info)
    {
        board[row, col] = piece;
        if(piece == Piece.Player1)
        {
            player1Count++;
        }
        else if(piece == Piece.Player2)
        {
            player2Count++;
        }

        for (int i = 0; i < info.FlipableDirections.Count; i++)
        {
            int directionRow = info.FlipableDirections[i][0];
            int directionCol = info.FlipableDirections[i][1];
            int r = row + directionRow;
            int c = col + directionCol;
            while (r >= 0 && r < Size && c >= 0 && c < Size && board[r, c] != piece)
            {
                board[r, c] = piece;
                r += directionRow;
                c += directionCol;

                if(piece == Piece.Player1)
                {
                    player1Count++;
                    player2Count--;
                }
                else if(piece == Piece.Player2)
                {
                    player2Count++;
                    player1Count--;
                }
            }
        }
    }

    private void FillBoard()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (board[i, j] == Piece.None)
                {
                    board[i, j] = Piece.None;
                }
            }
        }
    }
}
