using UnityEngine;

public enum GameState
{
    MainMenu,
    NewGame,
    Saving,
    Playing,
    Pause,
    OptionsMenu,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    private IGameState currentState;
    public void SwitchState(IGameState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Update()
    {
        
    }
}
