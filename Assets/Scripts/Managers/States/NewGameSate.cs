using UnityEngine;

public class NewGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter New Game State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        Debug.Log("Update New Game State...");
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exit New Game State...");
    }
}
