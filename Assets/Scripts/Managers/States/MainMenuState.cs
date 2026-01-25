using UnityEngine;

public class MainMenuState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Enter Main Menu State...");
    }

    public void UpdateState(GameManager gameManager)
    {
        //Debug.Log("Update Main Menu State...");
    }

    public void ExitState(GameManager gameManager)
    {
        //Debug.Log("Exit Main Menu State...");
    }
}
