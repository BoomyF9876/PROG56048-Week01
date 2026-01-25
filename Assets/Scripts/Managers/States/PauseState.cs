using UnityEngine;

public class PauseState : IGameState
{
    private GameObject obj = GameObject.FindFirstObjectByType<PauseController>().gameObject.transform.Find("PauseMenu").gameObject;

    public void EnterState(GameManager gameManager)
    {
        obj.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        //Debug.Log("Update Pause State...");
    }

    public void ExitState(GameManager gameManager)
    {
        obj.SetActive(false);
    }
}
