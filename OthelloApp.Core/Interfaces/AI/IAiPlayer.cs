using OthelloApp.Core.Entities;

namespace OthelloApp.Core.Interfaces.AI;

/// <summary>
/// AIプレイヤーが有効手を選択するためのインターフェイス
/// </summary>
public interface IAiPlayer
{
    /// <summary>
    /// 有効手から最適な手を選択する
    /// </summary>
    /// <param name="validMoves">有効手のリスト</param>
    /// <param name="board">現在のボード状態</param>
    /// <param name="discType">AIプレイヤーのディスクタイプ</param>
    /// <returns>選択された有効手</returns>
    ValidMove SelectMove(ValidMoves validMoves, Board board, DiscType discType);
}