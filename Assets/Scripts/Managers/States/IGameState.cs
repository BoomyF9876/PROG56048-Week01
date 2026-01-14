using UnityEngine;

public interface IGameState
{
    public void EnterState(GameManager gameManager);
    public void UpdateState(GameManager gameManager);
    public void ExitState(GameManager gameManager);
}
