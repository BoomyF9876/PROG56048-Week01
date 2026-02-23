using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        SceneLoader.Instance.LoadScene("MainScene", LoadSceneMode.Single);
        //SceneLoader.Instance.SetActiveScene("MainScene");
        GameManager.Instance.SwitchState(new PlayingState());
    }

    public void UpdateState(GameManager gameManager)
    {
        
    }

    public void ExitState(GameManager gameManager)
    {
        //Debug.Log("Exit New Game State...");
    }
}
