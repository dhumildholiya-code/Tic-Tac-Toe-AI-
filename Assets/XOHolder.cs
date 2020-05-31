using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XOHolder : MonoBehaviour
{
    public static event Action OnPlayerTurn = delegate { };

    public int data;
    private Button button;
    public Image image { get; set; }

    private int changeValueCounter;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Draw);
    }

    private void Start()
    {
        GameManager.Instance.OnGameWin += AfterGameWin;
        GameManager.Instance.OnGameStart += OnStartGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameWin -= AfterGameWin;
        GameManager.Instance.OnGameStart -= OnStartGame;
    }

    private void Draw()
    {
        if (changeValueCounter < 1)
        {
            if (GameManager.Instance.isPlayer1)
            {
                //player 1 : X
                data = Player.human;
                image.sprite = GameManager.Instance.token.human;
                OnPlayerTurn();
            }
            // else
            // {
            //     //player 2  : O;
            //     data = Player.AI;
            //     image.sprite = GameManager.Instance.token.AI;
            //     OnPlayerTurn();
            // }

            changeValueCounter++;
        }
    }

    private void AfterGameWin()
    {
        button.interactable = false;
    }

    private void OnStartGame()
    {
        changeValueCounter = 0;
        button.interactable = true;
    }
}
