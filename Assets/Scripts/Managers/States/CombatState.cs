using UnityEngine;

public class CombatState : IPlayerState
{
    public void EnterState(PlayerManager playerManager)
    {
        playerManager.GetComponent<TankMotor>().enabled = true;
    }

    public void UpdateState(PlayerManager playerManager)
    {
        //Debug.Log("Update Combat State...");
    }

    public void ExitState(PlayerManager playerManager)
    {
        playerManager.GetComponent<TankMotor>().enabled = false;
    }
}
