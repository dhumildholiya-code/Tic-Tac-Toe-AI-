using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonPattern<GameManager>
{

    //player 1 : X
    //Player 2 : O

    public event Action OnGameWin = delegate { };
    public event Action OnGameStart = delegate { };
    public event Action OnBotTurn = delegate { };

    public Token token;
    public bool isPlayer1 = false;
    public int winner;

    [Header("Game Area")]
    [SerializeField] private Transform gameArea;

    public XOHolder[,] ticTacField = new XOHolder[3, 3];
    private List<XOHolder> _oneDArray = new List<XOHolder>();
    public int turnCounter;

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform _transform in gameArea)
        {
            XOHolder _xoHolder = _transform.GetComponent<XOHolder>();
            _oneDArray.Add(_xoHolder);
        }

        Init();

    }

    private void Start()
    {
        ClearField();
        // OnGameStart();
        // if (!isPlayer1)
        // {
        // }
    }

    private void OnEnable()
    {
        XOHolder.OnPlayerTurn += ChangeTurn;
        AIplayer.OnAiTurn += AiTurn;
    }
    private void OnDisable()
    {
        XOHolder.OnPlayerTurn -= ChangeTurn;
        AIplayer.OnAiTurn -= AiTurn;
    }

    private void AiTurn(Vector2Int _move)
    {
        ticTacField[_move.x, _move.y].data = Player.AI;
        ticTacField[_move.x, _move.y].image.sprite = GameManager.Instance.token.AI;
        ChangeTurn();
    }

    private void ChangeTurn()
    {
        turnCounter++;
        isPlayer1 = !isPlayer1;

        if (!isPlayer1)
        {
            //its AI Turn.
            OnBotTurn();
        }

        winner = CheckWinner(ticTacField);
        // Debug.Log("winner --> " + winner);
        if (winner != 2)
        {
            OnGameWin();
        }
    }
    public int CheckWinner(XOHolder[,] _board)
    {
        int _winner = 2;

        //check Horizontal
        for (int i = 0; i < 3; i++)
        {
            if (is3Equal(_board[i, 0].data, _board[i, 1].data, _board[i, 2].data))
            {
                _winner = _board[i, 0].data;
            }
        }

        //check vertical
        for (int i = 0; i < 3; i++)
        {
            if (is3Equal(_board[0, i].data, _board[1, i].data, _board[2, i].data))
            {
                _winner = _board[0, i].data;
            }
        }

        //check Diagonal
        if (is3Equal(_board[0, 0].data, _board[1, 1].data, _board[2, 2].data))
        {
            _winner = _board[0, 0].data;
        }
        if (is3Equal(_board[2, 0].data, _board[1, 1].data, _board[0, 2].data))
        {
            _winner = _board[2, 0].data;
        }

        if (_winner == 2 && Available(_board) == 0)
        {
            _winner = 3;
        }

        // Debug.Log("asdf  " + _winner);

        return _winner;
    }
    public void GameRestart()
    {
        OnGameStart();
        ClearField();
    }

    private int Available(XOHolder[,] _board)
    {
        int _empty = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_board[i, j].data == 2)
                {
                    _empty++;
                }
            }
        }

        return _empty;
    }
    private bool is3Equal(int a, int b, int c)
    {
        return (a == b && b == c && a == c && a != 2);
    }

    private void Init()
    {
        int _counter = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ticTacField[i, j] = _oneDArray[_counter];
                _counter++;
            }
        }
    }

    private void ClearField()
    {
        turnCounter = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ticTacField[i, j].data = 2;
                ticTacField[i, j].image.sprite = null;
                // Debug.Log(ticTacField[i, j].image);
            }
        }
    }


}//Game Manager class end.

[System.Serializable]
public class Token
{
    public Sprite human;
    public Sprite AI;
}

public class Player
{
    public static int human = 1;
    public static int AI = 0;
}
