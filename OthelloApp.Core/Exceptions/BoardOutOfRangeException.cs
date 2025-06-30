using System;
namespace OthelloApp.Core.Exceptions;

public class BoardOutOfRangeException : Exception
{
    public BoardOutOfRangeException() : base("盤面の範囲外です。") { }
}
