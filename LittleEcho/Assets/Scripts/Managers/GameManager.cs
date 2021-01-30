using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { MainMenu, Playing, Paused, GameOver}


    private GameState _currentGameState = GameState.Playing;
    public GameState CurrentGameState
    {
        set { _currentGameState = value; }
        get { return _currentGameState; }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    // will include transitions
    public void LoadScene(string scene, GameState newState)
    {
        CurrentGameState = newState;
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.GameOver:
                // player has died, change scene to gameover
                break;
        }
    }



}
