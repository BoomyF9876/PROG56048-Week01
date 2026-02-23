using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private IGameState currentState;
    private Stack<IGameState> stateHistory = new Stack<IGameState>();

    public void SwitchState(IGameState newState, bool addToHistory = false)
    {
        if (addToHistory)
        {
            stateHistory.Push(currentState);
        }

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public bool IsGamePlaying()
    {
        return currentState is PlayingState;
    }

    public IGameState GetCurrentState()
    {
        return currentState;
    }

    public void ResumePreviousState()
    {
        if (stateHistory.Count > 0)
        {
            SwitchState(stateHistory.Pop(), addToHistory: false);
        }
    }
}
