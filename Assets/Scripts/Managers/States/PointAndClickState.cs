using UnityEngine;

public class PointAndClickState : IPlayerState
{
    public void EnterState(PlayerManager playerManager)
    {
        playerManager.GetComponent<PointAndClickMotor>().enabled = true;
    }

    public void UpdateState(PlayerManager playerManager)
    {
        //Debug.Log("Update Exploration State...");
    }

    public void ExitState(PlayerManager playerManager)
    {
        playerManager.GetComponent<PointAndClickMotor>().enabled = false;
    }
}
