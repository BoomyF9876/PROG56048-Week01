using UnityEngine;

public class OptionsMenu : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Options Menu State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        Debug.Log("Update Options Menu State...");
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exit Options Menu State...");
    }
}
