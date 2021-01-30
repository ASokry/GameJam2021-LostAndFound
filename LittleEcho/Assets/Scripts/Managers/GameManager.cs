using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { Playing, Paused, GameOver}

    private GameState _currentGameState = GameState.Playing;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
    }


}
