using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// All the states of the game
    /// </summary>
    public enum GameState { MainMenu, Playing, Paused, GameOver}

    /// <summary>
    /// Stores the current state of the game
    /// </summary>
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
    }
    private GameState _currentGameState = GameState.Playing;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Changes the game state
    /// </summary>
    /// <param name="newState">The state to change to</param>
    /// <param name="delay">Option delay to change</param>
    public void ChangeState(GameState newState, int delay = 0)
    {
        if (newState == CurrentGameState) return; // do nothing

        if(delay > 0)
        {        
            StartCoroutine(ChangeStateInTime(newState, delay));
        }
        else
        {
            _currentGameState = newState;
        }
    }

    IEnumerator ChangeStateInTime(GameState newState, int delay)
    {
        yield return new WaitForSeconds(delay);
        _currentGameState = newState;
    }

    // will include transitions
    public void LoadScene(string scene, GameState state)
    {
        ChangeState(state);
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
