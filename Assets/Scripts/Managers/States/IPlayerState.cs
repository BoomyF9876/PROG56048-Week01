using UnityEngine;

public interface IPlayerState
{
    public void EnterState(PlayerManager playerManager);
    public void UpdateState(PlayerManager playerManager);
    public void ExitState(PlayerManager playerManager);
}
