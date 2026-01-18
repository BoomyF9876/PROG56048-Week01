using UnityEngine;

public class GameOverState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Game Over State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        Debug.Log("Update Game Over State...");
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exit Game Over State...");
    }
}
