using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MainMenu
{

    protected override void Start()
    {
        base.Start();
        base.StartMenu.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        base.StartMenu.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.Paused);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        base.StartMenu.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Paused);
        Time.timeScale = 1;
    }

    public void ExitToMainMenu()
    {
        GameManager.Instance.LoadScene("MainMenu", GameManager.GameState.MainMenu);
    }
}
