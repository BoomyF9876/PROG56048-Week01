using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private IPlayerState currentState;
    public void SwitchState(IPlayerState newState)
    {
        currentState?.ExitState(this);
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
}
