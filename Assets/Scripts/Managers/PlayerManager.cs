using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private IPlayerState currentState;
    private IPlayerState lastState;

    public void SwitchState(IPlayerState newState)
    {
        currentState?.ExitState(this);

        lastState = currentState;
        currentState = newState;

        currentState.EnterState(this);
    }

    public void Update()
    {
        currentState.UpdateState(this);
    }
    public void Start()
    {
        SwitchState(new ExplorationState());
    }

    public IPlayerState GetLastState()
    {
        return lastState;
    }
}
