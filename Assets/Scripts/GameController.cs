using UnityEngine;

public class GameController : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.SwitchState(new MainMenuState());
    }

    public void PlayGame()
    {
        GameManager.Instance.SwitchState(new NewGameState());
    }
}
