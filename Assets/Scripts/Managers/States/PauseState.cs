using UnityEngine;

public class PauseState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Pause State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        Debug.Log("Update Pause State...");
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exit Pause State...");
    }
}
