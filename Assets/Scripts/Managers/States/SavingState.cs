using UnityEngine;

public class SavingState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Saving State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        Debug.Log("Update Saving State...");
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exit Saving State...");
    }
}
