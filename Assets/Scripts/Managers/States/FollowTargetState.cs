using UnityEngine;

public class FollowTargetState : IPlayerState
{
    public void EnterState(PlayerManager playerManager)
    {
        playerManager.GetComponent<FollowTargetMotor>().enabled = true;
    }

    public void UpdateState(PlayerManager playerManager)
    {
        //Debug.Log("Update Exploration State...");
    }

    public void ExitState(PlayerManager playerManager)
    {
        playerManager.GetComponent<FollowTargetMotor>().enabled = false;
    }
}
