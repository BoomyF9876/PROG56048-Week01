using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        SceneLoader.Instance.LoadScene("SecondScene", LoadSceneMode.Single);
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
