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
        get { return _currentGameState; }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void ChangeGameStateOnSceneLoad(string scene)
    {
        // change state based on what scene was loaded
    }

    // will include transitions
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }



}
