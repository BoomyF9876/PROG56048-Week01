using UnityEngine;

public class ExplorationState : IPlayerState
{
    public void EnterState(PlayerManager playerManager)
    {
        playerManager.GetComponent<FreeMovementMotor>().enabled = true;
    }

    public void UpdateState(PlayerManager playerManager)
    {
        //Debug.Log("Update Exploration State...");
    }

    public void ExitState(PlayerManager playerManager)
    {
        playerManager.GetComponent<FreeMovementMotor>().enabled = false;
    }
}
