using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void UpdateState(GameManager gameManager)
    {
        //Debug.Log("Update New Game State...");
    }

    public void ExitState(GameManager gameManager)
    {
        //Debug.Log("Exit New Game State...");
    }
}
