using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowWinText : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStart += HideText;
        GameManager.Instance.OnGameWin += SetPlayerWon;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= HideText;
        GameManager.Instance.OnGameWin -= SetPlayerWon;
    }

    private void SetPlayerWon()
    {
        text.gameObject.SetActive(true);

        if (GameManager.Instance.winner == 0)
        {
            //Player 2 : O
            text.text = string.Format("AI Player Won !!!!");
        }
        else if (GameManager.Instance.winner == 1)
        {
            text.text = string.Format("Player 1 Won !!!!");
        }
        else if (GameManager.Instance.winner == 3)
        {
            text.text = string.Format("Tie !!!");
        }
    }

    private void HideText()
    {
        text.gameObject.SetActive(false);
    }
}

