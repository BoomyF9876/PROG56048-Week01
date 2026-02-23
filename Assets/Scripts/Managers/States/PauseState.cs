using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        SceneLoader.Instance.LoadScene("_PauseScene", LoadSceneMode.Additive);
        //SceneLoader.Instance.SetActiveScene("_PauseScene");
    }

    public void UpdateState(GameManager gameManager)
    {}

    public void ExitState(GameManager gameManager)
    {
        SceneLoader.Instance.UnloadScene("_PauseScene");
    }
}
