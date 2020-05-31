using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIplayer : MonoBehaviour
{
    public static event Action<Vector2Int> OnAiTurn = delegate { };

    private Vector2Int _bestMove = new Vector2Int();
    private XOHolder[,] _board = new XOHolder[3, 3];

    private void Start()
    {
        GameManager.Instance.OnBotTurn += TakeTurn;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnBotTurn -= TakeTurn;
    }

    private void TakeTurn()
    {
        _board = GameManager.Instance.ticTacField;

        int _bestScore = int.MinValue;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_board[i, j].data == 2)
                {
                    _board[i, j].data = Player.AI;
                    int _moveScore = MiniMax(_board, 0, false);
                    _board[i, j].data = 2;
                    if (_moveScore > _bestScore)
                    {
                        _bestScore = _moveScore;
                        _bestMove = new Vector2Int(i, j);
                    }
                }
            }
        }

        OnAiTurn(_bestMove);
    }

    private int MiniMax(XOHolder[,] board, int depth, bool isMaximizingPlayer)
    {
        int _result = GameManager.Instance.CheckWinner(board);
        if (_result != 2)
        {
            // Debug.Log(Evaluate(_result));
            return Evaluate(_result);
        }

        if (isMaximizingPlayer)
        {
            int _maxEval = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //if Spot is Available.
                    if (board[i, j].data == 2)
                    {
                        board[i, j].data = Player.AI;
                        int _eval = MiniMax(board, depth + 1, false);
                        board[i, j].data = 2;
                        _maxEval = Mathf.Max(_eval, _maxEval);
                    }
                }
            }
            return _maxEval;
        }
        else
        {
            int _minEval = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //if Spot is Available.
                    if (board[i, j].data == 2)
                    {
                        board[i, j].data = Player.human;
                        int _eval = MiniMax(board, depth + 1, true);
                        board[i, j].data = 2;
                        _minEval = Mathf.Min(_eval, _minEval);
                    }
                }
            }
            return _minEval;
        }
    }

    private int Evaluate(int _dummy)
    {
        int _score = 0;

        if (_dummy == 0)
            _score = 1;
        else if (_dummy == 1)
            _score = -1;
        else if (_dummy == 3)
            _score = 0;

        return _score;
    }
}
