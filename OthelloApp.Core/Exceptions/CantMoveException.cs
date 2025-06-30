using System;

namespace OthelloApp.Core.Exceptions;

public class CantMoveException : Exception
{
    public CantMoveException() : base("その場所には駒を置けません。") { }
}
