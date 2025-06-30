using OthelloApp.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace OthelloApp.Core.Entities;

public class Board
{
    private CellType[,] board;
    private int player1Count = 0;
    private int player2Count = 0;

    public const int Size = 8;
    private static readonly Vector2[] directions =
    {
        new(-1, -1), // Top-left
        new(-1, 0),  // Top
        new(-1, 1),  // Top-right
        new(0, -1),  // Left
        new(0, 1),   // Right
        new(1, -1),  // Bottom-left
        new(1, 0),   // Bottom
        new(1, 1)    // Bottom-right
    };

    public Board()
    {
        board = new CellType[Size, Size];

        FillBoard();

        board[3, 3] = CellType.Player1;
        board[3, 4] = CellType.Player2;
        board[4, 3] = CellType.Player2;
        board[4, 4] = CellType.Player1;
        player1Count = 2;
        player2Count = 2;
    }

    public CellType this[int row, int col]
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

    public void PlaceDisc(int row, int col, DiscType discType)
    {
        if (row < 0 || row >= Size || col < 0 || col >= Size)
        {
            throw new BoardOutOfRangeException();
        }

        var validMove = IsValidMove(row, col, discType);
        if (validMove == null)
        {
            throw new CantMoveException();
        }

        FlipDiscs(row, col, discType, validMove);
    }

    public ValidMoves AllValidMoves(DiscType discType)
    {
        ValidMoves validMoves = new();
        validMoves.Player = discType;

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                var validMove = IsValidMove(row, col, discType);
                if (validMove != null)
                {
                    validMoves.MovableCells.Add(validMove);
                }
            }
        }
        return validMoves;
    }

    private ValidMove IsValidMove(int row, int col, DiscType discType)
    {
        if (board[row, col] != CellType.None)
        {
            return null;
        }
        var cellType = ConvertToCellType(discType);

        var validMove = new ValidMove();

        var oppositePiece = cellType == CellType.Player1 ? CellType.Player2 : CellType.Player1;

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int directionRow = directions[i].Row;
            int directionCol = directions[i].Column;
            int r = row + directionRow;
            int c = col + directionCol;

            while (r >= 0 && r < Size && c >= 0 && c < Size && board[r, c] == oppositePiece)
            {
                r += directionRow;
                c += directionCol;

                if (r < 0 || r >= Size || c < 0 || c >= Size)
                {
                    break;
                }

                if (board[r, c] == cellType)
                {
                    validMove.Row = row;
                    validMove.Column = col;
                    validMove.Directions.Add(new Vector2(directionRow, directionCol));
                    break;
                }
            }
        }

        return validMove.Directions.Count > 0 ? validMove : null;
    }

    private void FlipDiscs(int row, int col, DiscType discType, ValidMove validMove)
    {
        var cellType = ConvertToCellType(discType);
        board[row, col] = cellType;
        if (cellType == CellType.Player1)
        {
            player1Count++;
        }
        else if (cellType == CellType.Player2)
        {
            player2Count++;
        }

        for (int i = 0; i < validMove.Directions.Count; i++)
        {
            int directionRow = validMove.Directions[i].Row;
            int directionCol = validMove.Directions[i].Column;
            int r = row + directionRow;
            int c = col + directionCol;
            while (r >= 0 && r < Size && c >= 0 && c < Size && board[r, c] != cellType)
            {
                board[r, c] = cellType;
                r += directionRow;
                c += directionCol;

                if (cellType == CellType.Player1)
                {
                    player1Count++;
                    player2Count--;
                }
                else if (cellType == CellType.Player2)
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
                if (board[i, j] == CellType.None)
                {
                    board[i, j] = CellType.None;
                }
            }
        }
    }

    private CellType ConvertToCellType(DiscType disc)
    {
        switch (disc)
        {
            case DiscType.Player1:
                return CellType.Player1;
            case DiscType.Player2:
                return CellType.Player2;
            default:
                throw new ArgumentException();
        }
    }
}
