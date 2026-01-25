using UnityEngine;

public class PlayingState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Playing State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        //Debug.Log("Update Playing State...");
    }

    public void ExitState(GameManager gameManager)
    {
        //Debug.Log("Exit Playing State...");
    }
}
