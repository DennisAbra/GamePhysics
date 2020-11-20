using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    Text text;
    public static UnityEvent ResetGame = new UnityEvent();
    public static UnityEvent<bool> InMenu = new UnityEvent<bool>();
    [SerializeField] GameObject winParent;
    [SerializeField] GameObject mainMenu;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        if(winParent == null)
        {
            winParent = GetComponentInChildren<Image>().gameObject;
        }
        if(mainMenu == null)
        {
            Image[] i = GetComponentsInChildren<Image>();
            mainMenu = i[2].gameObject;
        }
        mainMenu.SetActive(true);
        InMenu.Invoke(true);
        winParent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PoolHole.UpdateScoreEvent.AddListener(UpdateScore);
    }

    private void OnDisable()
    {
        PoolHole.UpdateScoreEvent.RemoveListener(UpdateScore);
    }

    private void UpdateScore(int scoreCount)
    {
        text.text = "Score: " + scoreCount;
        if(scoreCount == 10)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        text.text = "YOU WON";
        winParent.gameObject.SetActive(true);
        InMenu.Invoke(true);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        InMenu.Invoke(false);
    }

    public void PlayAgain()
    {
        ResetGame.Invoke();
        winParent.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
