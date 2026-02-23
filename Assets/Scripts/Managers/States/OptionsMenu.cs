using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        SceneLoader.Instance.LoadScene("_OptionsScene", LoadSceneMode.Additive);
        //SceneLoader.Instance.SetActiveScene("_OptionsScene");
    }

    public void UpdateState(GameManager gameManager)
    {
        //Debug.Log("Update Options Menu State...");
    }

    public void ExitState(GameManager gameManager)
    {
        SceneLoader.Instance.UnloadScene("_OptionsScene");
    }
}
