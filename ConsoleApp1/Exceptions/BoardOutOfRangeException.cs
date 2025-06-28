using System;
namespace ConsoleOthello.Exceptions;

public class BoardOutOfRangeException : Exception
{
    public BoardOutOfRangeException() : base("盤面の範囲外です。") { }
}
