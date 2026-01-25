using UnityEngine;

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
        currentState.UpdateState(this);
    }

    public bool IsGamePlaying()
    {
        return currentState is PlayingState;
    }

    public IGameState GetCurrentState()
    {
        return currentState;
    }
}
